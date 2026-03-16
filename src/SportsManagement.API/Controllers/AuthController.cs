using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsManagement.API.Data;
using SportsManagement.API.DTOs;
using SportsManagement.API.Enums;
using SportsManagement.API.Models;
using SportsManagement.API.Services;

namespace SportsManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwt;
    private readonly IGoogleAuthService _google;

    public AuthController(AppDbContext db, IJwtService jwt, IGoogleAuthService google)
    {
        _db = db;
        _jwt = jwt;
        _google = google;
    }

    // ── Standard sign-up ────────────────────────────────────────────────────
    /// <summary>Registers a new user. Account is placed in PendingApproval state until a manager approves it.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(new { message = "Email already in use." });

        if (await _db.Users.AnyAsync(u => u.Username == req.Username))
            return Conflict(new { message = "Username already taken." });

        var user = new User
        {
            Username = req.Username,
            Name = req.Name,
            Email = req.Email,
            PhoneNumber = req.PhoneNumber,
            DateOfBirth = req.DateOfBirth,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Status = UserStatus.PendingApproval
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Registration successful. Your account is pending manager approval.",
            userId = user.UserId
        });
    }

    // ── Standard login ───────────────────────────────────────────────────────
    /// <summary>Authenticates a registered user and returns a JWT token.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == req.Email);

        if (user is null || user.PasswordHash is null ||
            !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials." });

        if (user.Status == UserStatus.PendingApproval)
            return Forbid();

        if (user.Status == UserStatus.Rejected)
            return Forbid();

        var roles = user.UserRoles.Select(ur => ur.Role.Name);
        var token = _jwt.GenerateToken(user, roles);

        return Ok(new AuthResponse(token, user.Username, user.Email, roles, user.Status.ToString()));
    }

    // ── Google sign-up (step 1) ──────────────────────────────────────────────
    /// <summary>
    /// Initiates Google sign-up. If the Google account is new, returns a flag
    /// indicating that the user must complete the profile form (name + DOB).
    /// </summary>
    [HttpPost("google/initiate")]
    public async Task<IActionResult> GoogleInitiate([FromBody] GoogleLoginRequest req)
    {
        var payload = await _google.VerifyGoogleTokenAsync(req.GoogleIdToken);
        if (payload is null)
            return Unauthorized(new { message = "Invalid Google token." });

        var existingUser = await _db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.GoogleId == payload.Subject);

        if (existingUser is not null)
        {
            // Existing Google user — log in directly (if approved)
            if (existingUser.Status == UserStatus.PendingApproval)
                return Ok(new { requiresCompletion = false, pendingApproval = true });

            if (existingUser.Status == UserStatus.Rejected)
                return Forbid();

            var roles = existingUser.UserRoles.Select(ur => ur.Role.Name);
            var token = _jwt.GenerateToken(existingUser, roles);
            return Ok(new
            {
                requiresCompletion = false,
                pendingApproval = false,
                auth = new AuthResponse(token, existingUser.Username, existingUser.Email, roles, existingUser.Status.ToString())
            });
        }

        // New Google user — needs to complete profile form
        return Ok(new
        {
            requiresCompletion = true,
            googleEmail = payload.Email,
            googleName = payload.Name
        });
    }

    // ── Google sign-up (step 2) ──────────────────────────────────────────────
    /// <summary>
    /// Completes Google sign-up. Accepts name and date of birth (email/phone taken from Google).
    /// Account is placed in PendingApproval state.
    /// </summary>
    [HttpPost("google/complete")]
    public async Task<IActionResult> GoogleComplete([FromBody] GoogleSignupCompletionRequest req)
    {
        var payload = await _google.VerifyGoogleTokenAsync(req.GoogleIdToken);
        if (payload is null)
            return Unauthorized(new { message = "Invalid Google token." });

        if (await _db.Users.AnyAsync(u => u.GoogleId == payload.Subject))
            return Conflict(new { message = "Google account already registered." });

        if (await _db.Users.AnyAsync(u => u.Email == payload.Email))
            return Conflict(new { message = "Email already in use by another account." });

        var username = payload.Email.Split('@')[0];
        // Make username unique if it collides
        if (await _db.Users.AnyAsync(u => u.Username == username))
            username = $"{username}_{Guid.NewGuid().ToString("N")[..6]}";

        var user = new User
        {
            Username = username,
            Name = req.Name,
            Email = payload.Email,
            DateOfBirth = req.DateOfBirth,
            GoogleId = payload.Subject,
            IsGoogleUser = true,
            Status = UserStatus.PendingApproval
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Google sign-up successful. Your account is pending manager approval.",
            userId = user.UserId
        });
    }

    // ── Google login (existing user) ─────────────────────────────────────────
    /// <summary>Logs in an existing Google user. Uses the same /google/initiate endpoint.</summary>
    [HttpPost("google/login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
        => await GoogleInitiate(req);
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsManagement.API.Data;
using SportsManagement.API.DTOs;
using SportsManagement.API.Enums;
using SportsManagement.API.Models;

namespace SportsManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Manager")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db) => _db = db;

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Sid)
            ?? User.FindFirstValue("sub")!);

    // ── List pending users ───────────────────────────────────────────────────
    /// <summary>Returns all users waiting for approval.</summary>
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingUsers()
    {
        var pending = await _db.Users
            .Where(u => u.Status == UserStatus.PendingApproval)
            .Select(u => new PendingUserDto(
                u.UserId, u.Username, u.Name, u.Email,
                u.PhoneNumber, u.IsGoogleUser, u.CreatedAt))
            .ToListAsync();

        return Ok(pending);
    }

    // ── Approve user ─────────────────────────────────────────────────────────
    /// <summary>Approves a pending user account.</summary>
    [HttpPost("approve")]
    public async Task<IActionResult> ApproveUser([FromBody] ApproveUserRequest req)
    {
        var user = await _db.Users.FindAsync(req.UserId);
        if (user is null) return NotFound(new { message = "User not found." });
        if (user.Status != UserStatus.PendingApproval)
            return BadRequest(new { message = "User is not in PendingApproval state." });

        user.Status = UserStatus.Active;
        user.ApprovedByUserId = CurrentUserId;
        user.ApprovedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new { message = $"User '{user.Username}' approved." });
    }

    // ── Reject user ──────────────────────────────────────────────────────────
    /// <summary>Rejects a pending user account.</summary>
    [HttpPost("reject")]
    public async Task<IActionResult> RejectUser([FromBody] RejectUserRequest req)
    {
        var user = await _db.Users.FindAsync(req.UserId);
        if (user is null) return NotFound(new { message = "User not found." });
        if (user.Status != UserStatus.PendingApproval)
            return BadRequest(new { message = "User is not in PendingApproval state." });

        user.Status = UserStatus.Rejected;
        user.ApprovedByUserId = CurrentUserId;
        user.ApprovedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new { message = $"User '{user.Username}' rejected." });
    }

    // ── Assign role ───────────────────────────────────────────────────────────
    /// <summary>Assigns a role to an active user (optionally scoped to a team).</summary>
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest req)
    {
        var user = await _db.Users.FindAsync(req.UserId);
        if (user is null) return NotFound(new { message = "User not found." });
        if (user.Status != UserStatus.Active)
            return BadRequest(new { message = "Only active users can be assigned roles." });

        var role = await _db.Roles.FindAsync(req.RoleId);
        if (role is null) return NotFound(new { message = "Role not found." });

        // Prevent duplicate assignment for same user/role/team combination
        var exists = await _db.UserRoles.AnyAsync(ur =>
            ur.UserId == req.UserId &&
            ur.RoleId == req.RoleId &&
            ur.TeamId == req.TeamId);

        if (exists)
            return Conflict(new { message = "User already has this role." });

        _db.UserRoles.Add(new UserRole
        {
            UserId = req.UserId,
            RoleId = req.RoleId,
            TeamId = req.TeamId,
            AssignedByUserId = CurrentUserId,
            AssignedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return Ok(new { message = $"Role '{role.Name}' assigned to '{user.Username}'." });
    }

    // ── List roles ────────────────────────────────────────────────────────────
    /// <summary>Lists all available roles.</summary>
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _db.Roles
            .Select(r => new { r.RoleId, r.Name, r.Description })
            .ToListAsync();
        return Ok(roles);
    }
}

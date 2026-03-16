namespace SportsManagement.API.DTOs;

public record RegisterRequest(
    string Username,
    string Name,
    string Email,
    string Password,
    string? PhoneNumber,
    DateOnly? DateOfBirth
);

public record GoogleSignupCompletionRequest(
    string GoogleIdToken,
    string Name,
    DateOnly DateOfBirth
);

public record LoginRequest(
    string Email,
    string Password
);

public record GoogleLoginRequest(
    string GoogleIdToken
);

public record ApproveUserRequest(
    int UserId
);

public record RejectUserRequest(
    int UserId,
    string? Reason
);

public record AssignRoleRequest(
    int UserId,
    int RoleId,
    int? TeamId
);

public record AuthResponse(
    string Token,
    string Username,
    string Email,
    IEnumerable<string> Roles,
    string Status
);

public record PendingUserDto(
    int UserId,
    string Username,
    string Name,
    string Email,
    string? PhoneNumber,
    bool IsGoogleUser,
    DateTime CreatedAt
);

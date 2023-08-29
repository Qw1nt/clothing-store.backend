namespace Application;

public record AssignRoleToUserRequest(int UserId, string Role = null!);
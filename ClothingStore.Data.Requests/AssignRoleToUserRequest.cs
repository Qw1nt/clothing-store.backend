namespace ClothingStore.Data.Requests;

public record AssignRoleToUserRequest(int UserId, string Role = null!);
namespace VerticalMinimalApi.Contracts;

public class UserResponse
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }
    
    public required string Name { get; set; }

    public required string Surname { get; set; }
    
}
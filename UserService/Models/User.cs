namespace UserService.Models;

using UserService.Models.Enums;

public class User
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; } = UserRole.CUSTOMER;
    public UserState State { get; set; } = UserState.ACTIVE;

    //TODO revisar que mas campos agregar

}

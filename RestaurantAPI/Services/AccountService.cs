using Microsoft.AspNetCore.Identity;

namespace RestaurantAPI;
public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
}
public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }
    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Nationality = dto.Nationality,
            RoleId = dto.RoleId
        };

        //hashowanie hasła użytkownika
        var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPassword;
        //zapis nowego użytkownika w bazce
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }
}

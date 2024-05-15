namespace RestaurantAPI;
public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
}
public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _dbContext;
    public AccountService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Nationality = dto.Nationality,
            RoleId = dto.RoleId
            //hasło będzie dodane później z pokazaniem jak działa hashowanie
        };

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }
}

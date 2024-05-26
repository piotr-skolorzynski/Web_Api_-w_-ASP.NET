using FluentValidation;

namespace RestaurantAPI;
public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(RestaurantDbContext dbContext)
    {   
        //za pomocą metody RuleFor możemy nakłaać walidacje na poszczególne pola poniżej dla emaila
        RuleFor(x => x.Email)
            .NotEmpty() //sprawia że to pole jest wymagane
            .EmailAddress(); //sprawdza czy jest to format adresu email

        //teraz wymagania dla pola password
        RuleFor(x => x.Password).MinimumLength(6); //minimalna długość hasła

        //wymaganie dla confirm password że ma być takie same jak password
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);

        RuleFor(x => x.Email)
            //metoda custom pozwala pisać własne metody walidacji
          .Custom((value, context) => 
          {
            //sprawdzamy czy w bazie danych już isnieje podany przez użytkownika email, 
            //do tego będzie potrzebny context bazy dancyh
            var emailInUse = dbContext.Users.Any(u => u.Email == value);
            //jeśli email już istnieje to trzeba przekazać błąd walidacji
            if (emailInUse)
            {
                context.AddFailure("Email", "That email is taken");
            }
          });
    }       
}

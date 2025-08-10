using FluentValidation;
using WeatherApi.Models;

namespace WeatherApi.Validators;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Имя пользователя не может быть пустым.")
            .MinimumLength(3).WithMessage("Имя пользователя должно быть не короче 3 символов.")
            .MaximumLength(20).WithMessage("Имя пользователя должно быть не длиннее 20 символов.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не может быть пустым.")
            .MinimumLength(8).WithMessage("Пароль должен быть не короче 8 символов.");

    }
}

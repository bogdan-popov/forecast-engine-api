using WeatherApi.Validators;
using WeatherApi.Models;
using FluentValidation.TestHelper;

public class UserRegisterDtoValidatorTests
{
    private readonly UserRegisterDtoValidator _validator;

    public UserRegisterDtoValidatorTests()
    {
        _validator = new UserRegisterDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Username_Is_Null()
    {
        var model = new UserRegisterDto { Username = null, Password = "password123" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UserRegisterDto { Username = "testuser", Password = "password123" };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
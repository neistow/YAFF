using FluentValidation;

namespace YAFF.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})")
                .WithMessage(
                    "Password should have: 1 lower character, 1 upper character, 1 numeric character, 1 special symbol and be at least 8 characters long");
            ;
        }
    }
}
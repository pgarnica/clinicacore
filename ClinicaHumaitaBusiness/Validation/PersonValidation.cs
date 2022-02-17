using ClinicaHumaita.Data.Models;
using FluentValidation;

namespace ClinicaHumaita.Business.Validation
{
    public class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(person => person.name).NotEmpty().WithMessage("Name field must be provided.");
            RuleFor(person => person.email).NotEmpty().WithMessage("Email field must be provided.")
                                           .EmailAddress().WithMessage("The provided email must br a valid email address");
        }
    }
}

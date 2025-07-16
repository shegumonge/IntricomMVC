using FluentValidation;
using IntricomMVC.Models;

namespace IntricomMVC.Validations
{
    public class ClientValidator : AbstractValidator<Client>
    {
        public ClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Es necesario y no mayor de 50 caracteres");
            RuleFor(x => x.Address).NotEmpty().MaximumLength(50).WithMessage("Es necesario y no mayor de 50 caracteres");
        }
    }
}

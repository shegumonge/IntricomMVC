using FluentValidation;
using IntricomMVC.Models;

namespace IntricomMVC.Validations
{
    public class ConfigurationValidator : AbstractValidator<Configuration>
    {
        public ConfigurationValidator()
        {
            //RuleFor(x => x.FsFolder).NotEmpty().MaximumLength(50).WithMessage("Es necesario y no mayor de 50 caracteres");
            RuleFor(x => x.DataType).NotEmpty().MaximumLength(50).WithMessage("Es necesario y no mayor de 2 caracteres");
        }
    }
}

using FluentValidation;
using ClientesAPI.Core.DTOs;

namespace ClientesAPI.Business.Validators
{
    public class ClienteCreacionDtoValidator : AbstractValidator<ClienteCreacionDto>
    {
        public ClienteCreacionDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(100).WithMessage("El apellido no puede tener más de 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El formato del email no es válido")
                .MaximumLength(100).WithMessage("El email no puede tener más de 100 caracteres");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El teléfono es requerido")
                .MaximumLength(20).WithMessage("El teléfono no puede tener más de 20 caracteres");

            RuleFor(x => x.Cuit)
                .NotEmpty().WithMessage("El CUIT es requerido")
                .MaximumLength(20).WithMessage("El CUIT no puede tener más de 20 caracteres");

            RuleFor(x => x.Domicilio)
                .MaximumLength(200).WithMessage("El domicilio no puede tener más de 200 caracteres");
        }
    }
}

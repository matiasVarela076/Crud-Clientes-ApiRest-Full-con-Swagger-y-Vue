using FluentValidation;
using ClientesAPI.Core.DTOs;

namespace ClientesAPI.Business.Validators
{
    public class ClienteActualizacionDtoValidator : AbstractValidator<ClienteActualizacionDto>
    {
        public ClienteActualizacionDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().When(x => x.Nombre != null).WithMessage("El nombre no puede estar vacío")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

            RuleFor(x => x.Apellido)
                .NotEmpty().When(x => x.Apellido != null).WithMessage("El apellido no puede estar vacío")
                .MaximumLength(100).WithMessage("El apellido no puede tener más de 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().When(x => x.Email != null).WithMessage("El email no puede estar vacío")
                .EmailAddress().WithMessage("El formato del email no es válido")
                .MaximumLength(100).WithMessage("El email no puede tener más de 100 caracteres");

            RuleFor(x => x.Telefono)
                .NotEmpty().When(x => x.Telefono != null).WithMessage("El teléfono no puede estar vacío")
                .MaximumLength(20).WithMessage("El teléfono no puede tener más de 20 caracteres");

            RuleFor(x => x.Domicilio)
                .MaximumLength(200).WithMessage("El domicilio no puede tener más de 200 caracteres");
        }
    }
}

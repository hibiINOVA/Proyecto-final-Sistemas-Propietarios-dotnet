using FluentValidation;
using proyectoSistemaPropietarios.DTOs;

namespace proyectoSistemaPropietarios.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");

            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(4).WithMessage("La contraseña debe tener al menos 4 caracteres.");
        }
    }
}

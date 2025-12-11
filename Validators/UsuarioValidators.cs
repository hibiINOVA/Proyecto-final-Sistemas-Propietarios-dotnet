using FluentValidation;

namespace proyectoSistemaPropietarios.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres.");

            RuleFor(u => u.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(50).WithMessage("El apellido no puede tener más de 50 caracteres.");

            RuleFor(u => u.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");

            RuleFor(u => u.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(u => u.Foto)
                .NotEmpty().WithMessage("La foto es obligatoria.")
                .Must(f => f.EndsWith(".jpg") || f.EndsWith(".png")).WithMessage("La foto debe ser jpg o png.");
        }
    }
}

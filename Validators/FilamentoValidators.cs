using FluentValidation;

namespace proyectoSistemaPropietarios
{
    public class FilamentoValidator : AbstractValidator<Filamento>
    {
        public FilamentoValidator()
        {
            RuleFor(f => f.Tipo)
                .NotEmpty().WithMessage("El tipo de filamento es obligatorio.")
                .MaximumLength(50).WithMessage("El tipo no puede exceder 50 caracteres.");

            RuleFor(f => f.Marca)
                .MaximumLength(50).WithMessage("La marca no puede exceder 50 caracteres.")
                .When(f => !string.IsNullOrEmpty(f.Marca));

            RuleFor(f => f.Color)
                .NotEmpty().WithMessage("El color del filamento es obligatorio.")
                .MaximumLength(30).WithMessage("El color no puede exceder 30 caracteres.");

            RuleFor(f => f.Diametro)
                .GreaterThan(0).WithMessage("El diámetro debe ser mayor a 0.");

            RuleFor(f => f.PrecioPorKg)
                .GreaterThan(0).WithMessage("El precio por kg debe ser mayor a 0.");

            RuleFor(f => f.Foto)
                .MaximumLength(250).WithMessage("La ruta de la foto no puede exceder 250 caracteres.")
                .When(f => !string.IsNullOrEmpty(f.Foto));

            RuleFor(f => f.UsuarioId)
                .NotEmpty().WithMessage("El filamento debe pertenecer a un usuario.");
        }
    }
}

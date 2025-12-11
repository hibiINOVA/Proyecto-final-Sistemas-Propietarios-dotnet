using FluentValidation;

namespace proyectoSistemaPropietarios
{
    public class ImpresoraValidator : AbstractValidator<Impresora>
    {
        public ImpresoraValidator()
        {
            RuleFor(i => i.Modelo)
                .NotEmpty().WithMessage("El modelo de la impresora es obligatorio.")
                .MaximumLength(50).WithMessage("El modelo no puede exceder 50 caracteres.");

            RuleFor(i => i.Marca)
                .NotEmpty().WithMessage("La marca de la impresora es obligatoria.")
                .MaximumLength(50).WithMessage("La marca no puede exceder 50 caracteres.");

            RuleFor(i => i.TipoExtrusion)
                .NotEmpty().WithMessage("El tipo de extrusión es obligatorio.")
                .MaximumLength(50).WithMessage("El tipo de extrusión no puede exceder 50 caracteres.");

            RuleFor(i => i.Foto)
                .NotEmpty().WithMessage("La ruta de la foto es obligatoria.")
                .MaximumLength(250).WithMessage("La ruta de la foto no puede exceder 250 caracteres.");

            RuleFor(i => i.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(i => i.DiametroBoquilla)
                .GreaterThan(0).WithMessage("El diámetro de la boquilla debe ser mayor a 0.");

            RuleFor(i => i.VelocidadDeImpresion)
                .GreaterThan(0).WithMessage("La velocidad de impresión debe ser mayor a 0.");

            RuleFor(i => i.CostoDeLuzPorHora)
                .GreaterThanOrEqualTo(0).WithMessage("El costo de luz por hora debe ser 0 o mayor.");

            RuleFor(i => i.UsuarioId)
                .NotEmpty().WithMessage("La impresora debe pertenecer a un usuario.");
        }
    }
}

using FluentValidation;

namespace proyectoSistemaPropietarios
{
    public class CotizacionValidator : AbstractValidator<Cotizacion>
    {
        public CotizacionValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre de la cotización es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(c => c.Descripcion)
                .MaximumLength(250).WithMessage("La descripción no puede exceder 250 caracteres.");

            RuleFor(c => c.TiempoHoras)
                .GreaterThan(0).WithMessage("El tiempo en horas debe ser mayor a 0.");

            RuleFor(c => c.FilamentoGr)
                .GreaterThan(0).WithMessage("Los gramos de filamento deben ser mayores a 0.");

            RuleFor(c => c.CostoFilamento)
                .GreaterThanOrEqualTo(0).WithMessage("El costo del filamento no puede ser negativo.");

            RuleFor(c => c.CostoLuz)
                .GreaterThanOrEqualTo(0).WithMessage("El costo de luz no puede ser negativo.");

            RuleFor(c => c.Subtotal)
                .GreaterThanOrEqualTo(0).WithMessage("El subtotal no puede ser negativo.");

            RuleFor(c => c.MargenError15)
                .GreaterThanOrEqualTo(0).WithMessage("El margen de error no puede ser negativo.");

            RuleFor(c => c.CostoDesgarteImpresora)
                .GreaterThanOrEqualTo(0).WithMessage("El costo por desgaste de impresora no puede ser negativo.");

            RuleFor(c => c.IVA)
                .GreaterThanOrEqualTo(0).WithMessage("El IVA no puede ser negativo.");

            RuleFor(c => c.Total)
                .GreaterThanOrEqualTo(0).WithMessage("El total no puede ser negativo.");

            RuleFor(c => c.ArchivoGcode)
                .NotEmpty().WithMessage("El archivo G-code es obligatorio.");
        }
    }
}

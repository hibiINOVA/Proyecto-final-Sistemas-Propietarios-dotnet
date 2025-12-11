namespace proyectoSistemaPropietarios;

public class Impresora
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Modelo { get; set; }
    public required string Marca { get; set; }

    public decimal Precio { get; set; }

    public required string TipoExtrusion { get; set; }

    public decimal DiametroBoquilla { get; set; }

    public decimal VelocidadDeImpresion { get; set; }

    public decimal CostoDeLuzPorHora { get; set; }

    public required string Foto { get; set; }

    public Guid UsuarioId { get; set; }
}

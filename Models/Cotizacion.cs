public class Cotizacion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UsuarioId { get; set; }
    public Guid ImpresoraId { get; set; }
    public Guid FilamentoId { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public decimal TiempoHoras { get; set; }
    public decimal FilamentoGr { get; set; }
    public decimal CostoFilamento { get; set; }
    public decimal CostoLuz { get; set; }
    public decimal Subtotal { get; set; }
    public decimal MargenError15 { get; set; }
    public decimal CostoDesgarteImpresora { get; set; }
    public decimal IVA { get; set; }
    public decimal Total { get; set; }

    public string ArchivoGcode { get; set; } = string.Empty;
}

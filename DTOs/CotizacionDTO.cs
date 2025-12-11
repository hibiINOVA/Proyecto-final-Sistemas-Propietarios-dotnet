namespace proyectoSistemaPropietarios.DTOs
{
    public class CotizacionDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
    }
}
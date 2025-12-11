using System.ComponentModel.DataAnnotations;

namespace proyectoSistemaPropietarios.DTOs
{
    public class CotizacionUpdateDto
    {
        [Range(0.1, 10000, ErrorMessage = "El tiempo en horas debe ser mayor a 0.")]
        public decimal? TiempoHoras { get; set; }

        [Range(0.1, 50000, ErrorMessage = "El filamento en gramos debe ser mayor a 0.")]
        public decimal? FilamentoGr { get; set; }

        [Range(0, 100, ErrorMessage = "El IVA debe estar entre 0 y 100.")]
        public decimal? Iva { get; set; }

        [MaxLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Nombre { get; set; }

        [MaxLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
        public string? Descripcion { get; set; }

        public IFormFile? ArchivoGcode { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace proyectoSistemaPropietarios.DTOs
{
    public class CotizacionCreateDto
    {
        [Required(ErrorMessage = "El archivo G-code es obligatorio.")]
        public IFormFile ArchivoGcode { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Range(0, 100, ErrorMessage = "El IVA debe estar entre 0 y 100.")]
        public decimal? Iva { get; set; } = 16m;
    }
}
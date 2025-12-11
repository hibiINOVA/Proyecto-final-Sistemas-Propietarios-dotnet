namespace proyectoSistemaPropietarios.Services
{
    public interface ICotizacionService
    {
        Task<Cotizacion> GenerarCotizacionAsync(
            Guid usuarioId,
            Guid impresoraId,
            Guid filamentoId,
            decimal tiempoHoras,
            decimal gramosFilamento,
            string archivoGcode,
            string? nombre,
            string? descripcion,
            decimal? iva
        );

        Task<Cotizacion?> ObtenerCotizacionAsync(Guid cotizacionId, Guid usuarioId);

        Task<IEnumerable<Cotizacion>> ObtenerCotizacionesPorUsuarioAsync(Guid usuarioId);

        Task<Cotizacion?> ActualizarCotizacionAsync(
            Guid cotizacionId,
            Guid usuarioId,
            Guid impresoraId,
            Guid filamentoId,
            decimal tiempoHoras,
            decimal gramosFilamento,
            string? archivoGcode = null,
            decimal? iva = 16,
            string? nombre = null,
            string? descripcion = null
        );


        Task<bool> EliminarCotizacionAsync(Guid cotizacionId, Guid usuarioId);
    }
}

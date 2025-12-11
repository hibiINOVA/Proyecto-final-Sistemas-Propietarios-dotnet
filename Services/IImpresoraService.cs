namespace proyectoSistemaPropietarios.Services
{
    public interface IImpresoraService
    {
        Task<Impresora> CrearAsync(Impresora impresora, Guid usuarioId);

        Task<IEnumerable<Impresora>> ObtenerPorUsuarioAsync(Guid usuarioId);

        Task<Impresora?> ObtenerPorUsuarioYIdAsync(Guid usuarioId, Guid impresoraId);

        Task<Impresora?> ActualizarAsync(
            Guid usuarioId,
            Guid impresoraId,
            Impresora impresoraActualizada
        );

        Task<bool> EliminarAsync(Guid usuarioId, Guid impresoraId);
    }
}

namespace proyectoSistemaPropietarios.Services
{
    public interface IFilamentoService
    {
        Task<IEnumerable<Filamento>> ObtenerPorUsuarioAsync(Guid usuarioId);
        Task<Filamento?> ObtenerPorUsuarioYFilamentoAsync(Guid usuarioId, Guid filamentoId);
        Task<Filamento> CrearAsync(Filamento filamento);
        Task<Filamento?> ActualizarAsync(Guid usuarioId, Guid filamentoId, Filamento filamentoActualizado);
        Task<bool> EliminarAsync(Guid usuarioId, Guid filamentoId);
    }
}

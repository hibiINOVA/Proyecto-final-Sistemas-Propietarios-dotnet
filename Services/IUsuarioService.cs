namespace proyectoSistemaPropietarios.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> CrearUsuarioAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> ObtenerTodosUsuariosAsync();
        Task<Usuario?> ObtenerPorIdAsync(Guid id);
        Task<Usuario?> ActualizarAsync(Guid id, Usuario usuarioActualizado);
        Task<bool> EliminarAsync(Guid id);

        Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    }
}

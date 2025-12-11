using Microsoft.EntityFrameworkCore;
using proyectoSistemaPropietarios.Data;


namespace proyectoSistemaPropietarios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UsuarioService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
        public async Task<IEnumerable<Usuario>> ObtenerTodosUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuario?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Usuarios.FindAsync(id);
        }
        public async Task<Usuario?> ActualizarAsync(Guid id, Usuario usuarioActualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            usuario.Nombre = usuarioActualizado.Nombre;
            usuario.Apellido = usuarioActualizado.Apellido;
            usuario.Correo = usuarioActualizado.Correo;
            usuario.Contrasena = usuarioActualizado.Contrasena;

            if (!string.IsNullOrEmpty(usuarioActualizado.Foto))
                usuario.Foto = usuarioActualizado.Foto;

            await _context.SaveChangesAsync();
            return usuario;
        }
        public async Task<bool> EliminarAsync(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .Where(x => x.Correo == correo)
                .FirstOrDefaultAsync();
        }
    }
}

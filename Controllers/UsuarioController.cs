using Microsoft.AspNetCore.Mvc;
using proyectoSistemaPropietarios.Services;
using proyectoSistemaPropietarios.DTOs;
using proyectoSistemaPropietarios.Validators;
using Microsoft.AspNetCore.Authorization;

namespace proyectoSistemaPropietarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuario
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ObtenerTodosUsuariosAsync();

            var listaDTO = usuarios.Select(u => new UsuarioDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                Foto = u.Foto
            });

            return Ok(new
            {
                mensaje = "Usuarios obtenidos correctamente",
                usuarios = listaDTO
            });
        }

        // GET: api/usuario/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = $"No existe un usuario con id {id}" });

            var dto = new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Foto = usuario.Foto
            };

            return Ok(new
            {
                mensaje = "Usuario obtenido correctamente",
                usuario = dto
            });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            [FromForm] string nombre,
            [FromForm] string apellido,
            [FromForm] string correo,
            [FromForm] string contrasena,
            [FromForm] IFormFile? foto)
        {
            string rutaFoto = "";

            if (foto != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "usuariosFotos");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await foto.CopyToAsync(stream);

                rutaFoto = $"/uploads/usuarios/{nombreArchivo}";
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                Correo = correo,
                Contrasena = contrasena,
                Foto = rutaFoto
            };

            var validator = new UsuarioValidator();
            var validationResult = validator.Validate(usuario);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var nuevo = await _usuarioService.CrearUsuarioAsync(usuario);

            var dto = new UsuarioDTO
            {
                Id = nuevo.Id,
                Nombre = nuevo.Nombre,
                Apellido = nuevo.Apellido,
                Correo = nuevo.Correo,
                Foto = nuevo.Foto
            };

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, new
            {
                mensaje = "Usuario creado exitosamente",
                usuario = dto
            });
        }

        // PUT: api/usuario/{id}
        [Authorize]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromForm] string nombre,
            [FromForm] string apellido,
            [FromForm] string correo,
            [FromForm] string contrasena,
            [FromForm] IFormFile? foto)
        {
            var usuarioActual = await _usuarioService.ObtenerPorIdAsync(id);

            if (usuarioActual == null)
                return NotFound(new { mensaje = $"No existe un usuario con id {id}" });

            string rutaFoto = usuarioActual.Foto;

            if (foto != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "usuariosFotos");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await foto.CopyToAsync(stream);

                rutaFoto = $"/uploads/usuarios/{nombreArchivo}";
            }

            usuarioActual.Nombre = nombre;
            usuarioActual.Apellido = apellido;
            usuarioActual.Correo = correo;

            if (!string.IsNullOrWhiteSpace(contrasena))
                usuarioActual.Contrasena = contrasena;

            usuarioActual.Foto = rutaFoto;

            var validator = new UsuarioValidator();
            var validationResult = validator.Validate(usuarioActual);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var updated = await _usuarioService.ActualizarAsync(id, usuarioActual);

            var dto = new UsuarioDTO
            {
                Id = updated.Id,
                Nombre = updated.Nombre,
                Apellido = updated.Apellido,
                Correo = updated.Correo,
                Foto = updated.Foto
            };

            return Ok(new
            {
                mensaje = "Usuario actualizado correctamente",
                usuario = dto
            });
        }

        // DELETE: api/usuario/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var eliminado = await _usuarioService.EliminarAsync(id);

            if (!eliminado)
                return NotFound(new { mensaje = $"No existe un usuario con id {id}" });

            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }
    }
}

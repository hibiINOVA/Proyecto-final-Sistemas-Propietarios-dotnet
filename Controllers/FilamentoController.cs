using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using proyectoSistemaPropietarios.Services;
using proyectoSistemaPropietarios.Validators;
namespace proyectoSistemaPropietarios.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilamentoController : ControllerBase
    {
        private readonly IFilamentoService _filamentoService;

        public FilamentoController(IFilamentoService filamentoService)
        {
            _filamentoService = filamentoService;
        }

        // GET: api/filamento/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(Guid usuarioId)
        {
            var filamentos = await _filamentoService.ObtenerPorUsuarioAsync(usuarioId);
            return Ok(new
            {
                message = "Filamentos del usuario obtenidos correctamente",
                data = filamentos
            });
        }

        // GET: api/filamento/{usuarioId}/{filamentoId}
        [HttpGet("{usuarioId}/{filamentoId}")]
        public async Task<IActionResult> GetByUsuarioYFilamento(Guid usuarioId, Guid filamentoId)
        {
            var filamento = await _filamentoService.ObtenerPorUsuarioYFilamentoAsync(usuarioId, filamentoId);

            if (filamento == null)
                return NotFound(new { message = "No existe ese filamento para este usuario." });

            return Ok(new
            {
                message = "Filamento obtenido correctamente",
                data = filamento
            });
        }

        // POST: api/filamento/{usuarioId}
        [HttpPost("{usuarioId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            Guid usuarioId,
            [FromForm] string tipo,
            [FromForm] string? marca,
            [FromForm] string color,
            [FromForm] decimal diametro,
            [FromForm] decimal precioPorKg,
            [FromForm] IFormFile? foto
        )
        {
            string rutaFoto = "";

            if (foto != null)
            {
                var carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "uploads", "filamentosFotos"
                );

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                rutaFoto = $"/uploads/filamentosFotos/{nombreArchivo}";
            }

            var filamento = new Filamento
            {
                Tipo = tipo,
                Marca = marca,
                Color = color,
                Diametro = diametro,
                PrecioPorKg = precioPorKg,
                UsuarioId = usuarioId,
                Foto = rutaFoto
            };

            var validator = new FilamentoValidator();
            var validation = validator.Validate(filamento);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var nuevo = await _filamentoService.CrearAsync(filamento);

            return Ok(new
            {
                mensaje = "Filamento creado correctamente",
                data = nuevo
            });
        }

        // PUT: api/filamento/{usuarioId}/{filamentoId}
        [HttpPut("{usuarioId}/{filamentoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
            Guid usuarioId,
            Guid filamentoId,
            [FromForm] string tipo,
            [FromForm] string? marca,
            [FromForm] string color,
            [FromForm] decimal diametro,
            [FromForm] decimal precioPorKg,
            [FromForm] IFormFile? foto
        )
        {
            var existente = await _filamentoService.ObtenerPorUsuarioYFilamentoAsync(usuarioId, filamentoId);

            if (existente == null)
                return NotFound(new { message = "No existe este filamento para este usuario." });

            if (foto != null)
            {
                var carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "uploads", "filamentosFotos"
                );

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                existente.Foto = $"/uploads/filamentosFotos/{nombreArchivo}";
            }

            existente.Tipo = tipo;
            existente.Marca = marca;
            existente.Color = color;
            existente.Diametro = diametro;
            existente.PrecioPorKg = precioPorKg;

            var validator = new FilamentoValidator();
            var validation = validator.Validate(existente);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var actualizado = await _filamentoService.ActualizarAsync(usuarioId, filamentoId, existente);

            return Ok(new
            {
                message = "Filamento actualizado correctamente",
                data = actualizado
            });
        }

        // DELETE: api/filamento/{usuarioId}/{filamentoId}
        [HttpDelete("{usuarioId}/{filamentoId}")]
        public async Task<IActionResult> Delete(Guid usuarioId, Guid filamentoId)
        {
            var eliminado = await _filamentoService.EliminarAsync(usuarioId, filamentoId);

            if (!eliminado)
                return NotFound(new { message = "No existe ese filamento para este usuario." });

            return Ok(new { message = "Filamento eliminado correctamente" });
        }
    }
}

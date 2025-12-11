using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using proyectoSistemaPropietarios.Services;
using proyectoSistemaPropietarios.Validators;

namespace proyectoSistemaPropietarios.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ImpresoraController : ControllerBase
    {
        private readonly IImpresoraService _impresoraService;

        public ImpresoraController(IImpresoraService impresoraService)
        {
            _impresoraService = impresoraService;
        }

        // GET: api/impresora/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(Guid usuarioId)
        {
            var impresoras = await _impresoraService.ObtenerPorUsuarioAsync(usuarioId);
            return Ok(impresoras);
        }

        // GET: api/impresora/{usuarioId}/{impresoraId}
        [HttpGet("{usuarioId}/{impresoraId}")]
        public async Task<IActionResult> GetById(Guid usuarioId, Guid impresoraId)
        {
            var impresora = await _impresoraService.ObtenerPorUsuarioYIdAsync(usuarioId, impresoraId);

            if (impresora == null)
                return NotFound($"No se encontró la impresora con Id: {impresoraId} y el usuario con Id: {usuarioId}");

            return Ok(impresora);
        }

        // POST: api/impresora/{usuarioId}
        [HttpPost("{usuarioId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            Guid usuarioId,
            [FromForm] string modelo,
            [FromForm] string marca,
            [FromForm] decimal precio,
            [FromForm] string tipoExtrusion,
            [FromForm] decimal diametroBoquilla,
            [FromForm] decimal velocidadDeImpresion,
            [FromForm] decimal costoDeLuzPorHora,
            [FromForm] IFormFile? foto
        )
        {
            string rutaFoto = "";

            if (foto != null)
            {
                var carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "uploads", "impresorasFotos"
                );

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var archivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, archivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await foto.CopyToAsync(stream);

                rutaFoto = $"/uploads/impresorasFotos/{archivo}";
            }

            var impresora = new Impresora
            {
                Modelo = modelo,
                Marca = marca,
                Precio = precio,
                TipoExtrusion = tipoExtrusion,
                DiametroBoquilla = diametroBoquilla,
                VelocidadDeImpresion = velocidadDeImpresion,
                CostoDeLuzPorHora = costoDeLuzPorHora,
                Foto = rutaFoto,
                UsuarioId = usuarioId
            };

            var validator = new ImpresoraValidator();
            var validation = validator.Validate(impresora);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var nueva = await _impresoraService.CrearAsync(impresora, usuarioId);

            return Ok(new
            {
                message = "Impresora creada correctamente",
                impresora = nueva
            });
        }


        // PUT: api/impresora/{usuarioId}/{impresoraId}
        [HttpPut("{usuarioId}/{impresoraId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
            Guid usuarioId,
            Guid impresoraId,
            [FromForm] string modelo,
            [FromForm] string marca,
            [FromForm] decimal precio,
            [FromForm] string tipoExtrusion,
            [FromForm] decimal diametroBoquilla,
            [FromForm] decimal velocidadDeImpresion,
            [FromForm] decimal costoDeLuzPorHora,
            [FromForm] IFormFile? foto
        )
        {
            try
            {
                var existente = await _impresoraService.ObtenerPorUsuarioYIdAsync(usuarioId, impresoraId);

                if (existente == null)
                    return NotFound("No existe la impresora para ese usuario.");

                if (foto != null)
                {
                    var carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot", "uploads", "impresorasFotos"
                    );

                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    var archivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                    var rutaCompleta = Path.Combine(carpeta, archivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    existente.Foto = $"/uploads/impresorasFotos/{archivo}";
                }

                existente.Modelo = modelo;
                existente.Marca = marca;
                existente.Precio = precio;
                existente.TipoExtrusion = tipoExtrusion;
                existente.DiametroBoquilla = diametroBoquilla;
                existente.VelocidadDeImpresion = velocidadDeImpresion;
                existente.CostoDeLuzPorHora = costoDeLuzPorHora;

                var validator = new ImpresoraValidator();
                var validation = validator.Validate(existente);

                if (!validation.IsValid)
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

                await _impresoraService.ActualizarAsync(usuarioId, impresoraId, existente);

                return Ok(new
                {
                    message = "Impresora actualizada correctamente",
                    impresora = existente
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocurrió un error al actualizar la impresora.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/impresora/{usuarioId}/{impresoraId}
        [HttpDelete("{usuarioId}/{impresoraId}")]
        public async Task<IActionResult> Delete(Guid usuarioId, Guid impresoraId)
        {
            var eliminado = await _impresoraService.EliminarAsync(usuarioId, impresoraId);

            if (!eliminado)
                return NotFound($"No existe la impresora para ese usuario.");

            return Ok("Impresora eliminada correctamente.");
        }
    }
}

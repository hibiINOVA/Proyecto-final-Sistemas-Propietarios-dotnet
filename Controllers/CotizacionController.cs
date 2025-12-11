using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using proyectoSistemaPropietarios.Services;
using proyectoSistemaPropietarios.DTOs;
using proyectoSistemaPropietarios.Validators;

namespace proyectoSistemaPropietarios.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CotizacionController : ControllerBase
    {
        private readonly ICotizacionService _cotizacionService;

        public CotizacionController(ICotizacionService cotizacionService)
        {
            _cotizacionService = cotizacionService;
        }

        // POST: api/cotizacion/{usuarioId}/{impresoraId}/{filamentoId}
        [HttpPost("{usuarioId}/{impresoraId}/{filamentoId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GenerarCotizacion(
            Guid usuarioId,
            Guid impresoraId,
            Guid filamentoId,
            [FromForm] CotizacionCreateDto dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var archivo = dto.ArchivoGcode;

            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "gcodes");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            string contenido = await System.IO.File.ReadAllTextAsync(rutaCompleta);

            var tiempoHoras = ExtraerTiempoHoras(contenido);
            var gramosFilamento = ExtraerFilamentoGr(contenido);

            if (tiempoHoras <= 0 || gramosFilamento <= 0)
                return BadRequest("El G-code no contiene datos suficientes para generar la cotización.");

            var cotizacion = await _cotizacionService.GenerarCotizacionAsync(
                usuarioId,
                impresoraId,
                filamentoId,
                tiempoHoras,
                gramosFilamento,
                $"/uploads/gcodes/{nombreArchivo}",
                dto.Nombre,
                dto.Descripcion,
                dto.Iva
            );

            return Ok(new
            {
                mensaje = "Cotización generada correctamente.",
                cotizacion
            });
        }


        // GET: api/cotizacion/{usuarioId}/{cotizacionId}
        [HttpGet("{usuarioId}/{cotizacionId}")]
        public async Task<IActionResult> Obtener(Guid usuarioId, Guid cotizacionId)
        {
            var cot = await _cotizacionService.ObtenerCotizacionAsync(cotizacionId, usuarioId);
            if (cot == null)
                return NotFound("La cotización no existe o no pertenece al usuario.");

            var dto = new CotizacionDTO
            {
                Nombre = cot.Nombre,
                Descripcion = cot.Descripcion,
                Subtotal = cot.Subtotal,
                IVA = cot.IVA,
                Total = cot.Total
            };

            return Ok(dto);
        }

        // GET: api/cotizacion/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(Guid usuarioId)
        {
            var cotizaciones = await _cotizacionService.ObtenerCotizacionesPorUsuarioAsync(usuarioId);

            var dtoList = cotizaciones.Select(c => new CotizacionDTO
            {
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Subtotal = c.Subtotal,
                IVA = c.IVA,
                Total = c.Total
            }).ToList();

            return Ok(dtoList);
        }

        [HttpPut("{usuarioId}/{cotizacionId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Actualizar(
            Guid usuarioId,
            Guid cotizacionId,
            [FromForm] CotizacionUpdateDto dto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cot = await _cotizacionService.ObtenerCotizacionAsync(cotizacionId, usuarioId);
            if (cot == null)
                return NotFound("La cotización no existe o no pertenece al usuario.");

            decimal newTiempo = cot.TiempoHoras;
            decimal newGramos = cot.FilamentoGr;
            string? newGcodeRuta = cot.ArchivoGcode;

            if (dto.ArchivoGcode != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "gcodes");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(dto.ArchivoGcode.FileName)}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await dto.ArchivoGcode.CopyToAsync(stream);

                string contenido = await System.IO.File.ReadAllTextAsync(rutaCompleta);

                var tiempo = ExtraerTiempoHoras(contenido);
                var gramos = ExtraerFilamentoGr(contenido);

                if (tiempo <= 0 || gramos <= 0)
                    return BadRequest("El G-code no contiene información suficiente para recalcular la cotización.");

                newTiempo = tiempo;
                newGramos = gramos;
                newGcodeRuta = $"/uploads/gcodes/{nombreArchivo}";
            }
            else
            {
                if (dto.TiempoHoras.HasValue)
                    newTiempo = dto.TiempoHoras.Value;

                if (dto.FilamentoGr.HasValue)
                    newGramos = dto.FilamentoGr.Value;
            }

            decimal ivaFinal = dto.Iva ?? 16m;

            var cotizacionActualizada = await _cotizacionService.ActualizarCotizacionAsync(
                cotizacionId,
                usuarioId,
                cot.ImpresoraId,
                cot.FilamentoId,
                newTiempo,
                newGramos,
                newGcodeRuta,
                ivaFinal,
                dto.Nombre,
                dto.Descripcion
            );

            if (cotizacionActualizada == null)
                return BadRequest("No se pudo actualizar la cotización.");

            return Ok(new
            {
                mensaje = "Cotización actualizada correctamente.",
                cotizacion = cotizacionActualizada
            });
        }

        // DELETE: api/cotizacion/{usuarioId}/{cotizacionId}
        [HttpDelete("{usuarioId}/{cotizacionId}")]
        public async Task<IActionResult> Eliminar(Guid usuarioId, Guid cotizacionId)
        {
            var eliminado = await _cotizacionService.EliminarCotizacionAsync(cotizacionId, usuarioId);
            if (!eliminado)
                return NotFound("La cotización no existe o no pertenece al usuario.");

            return Ok("Cotización eliminada correctamente.");
        }

        private decimal ExtraerTiempoHoras(string contenido)
        {
            var lineas = contenido.Split('\n');
            decimal horas = 0;

            foreach (var l in lineas)
            {
                var linea = l.Trim().ToLower();

                if (linea.Contains("estimated printing time"))
                {
                    var tiempo = ParseTiempoHumano(linea);
                    if (tiempo > 0) return tiempo;
                }

                if (linea.Contains("estimated_time:"))
                {
                    var val = linea.Replace("estimated_time:", "")
                                .Replace("s", "")
                                .Trim();

                    if (decimal.TryParse(val, out decimal segundos))
                        return Math.Round(segundos / 3600m, 2);
                }

                if (linea.StartsWith(";time:"))
                {
                    var val = linea.Replace(";time:", "").Trim();
                    if (int.TryParse(val, out int segundos))
                        return Math.Round(segundos / 3600m, 2);
                }
            }

            return 0;
        }

        private decimal ExtraerFilamentoGr(string contenido)
        {
            var lineas = contenido.Split('\n');

            foreach (var l in lineas)
            {
                var linea = l.Trim().ToLower();

                if (linea.Contains("filament used [g]"))
                {
                    var partes = linea.Split('=');
                    if (partes.Length == 2 &&
                        decimal.TryParse(partes[1].Trim(), out decimal gramos))
                        return gramos;
                }

                if (linea.Contains("filament_weight:"))
                {
                    var val = linea.Replace("filament_weight:", "")
                                .Replace("g", "")
                                .Trim();

                    if (decimal.TryParse(val, out decimal gramos))
                        return gramos;
                }

                if (linea.StartsWith(";filament_weight"))
                {
                    var val = linea.Split('=')[1].Replace("g", "").Trim();
                    if (decimal.TryParse(val, out decimal gramos))
                        return gramos;
                }
            }

            return 0;
        }

        private decimal ParseTiempoHumano(string linea)
        {

            linea = linea.ToLower();

            var tiempo = linea.Split('=').Last().Trim().Split(' ');

            int horas = 0, minutos = 0, segundos = 0;

            foreach (var t in tiempo)
            {
                if (t.EndsWith("h") && int.TryParse(t.Replace("h", ""), out int h))
                    horas = h;

                if (t.EndsWith("m") && int.TryParse(t.Replace("m", ""), out int m))
                    minutos = m;

                if (t.EndsWith("s") && int.TryParse(t.Replace("s", ""), out int s))
                    segundos = s;
            }

            var totalHoras = horas + (minutos / 60m) + (segundos / 3600m);
            return Math.Round(totalHoras, 2);
        }
    }
}

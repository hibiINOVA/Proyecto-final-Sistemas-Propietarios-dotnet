using Microsoft.EntityFrameworkCore;

using proyectoSistemaPropietarios.Data;

namespace proyectoSistemaPropietarios.Services
{
    public class CotizacionService : ICotizacionService
    {
        private readonly AppDbContext _context;

        public CotizacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cotizacion> GenerarCotizacionAsync(
            Guid usuarioId,
            Guid impresoraId,
            Guid filamentoId,
            decimal tiempoHoras,
            decimal gramosFilamento,
            string archivoGcode,
            string? nombre,
            string? descripcion,
            decimal? iva = null
        )
        {
            var impresora = await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == impresoraId && i.UsuarioId == usuarioId);
            if (impresora == null)
                throw new Exception("La impresora no existe o no pertenece al usuario.");

            var filamento = await _context.Filamentos
                .FirstOrDefaultAsync(f => f.Id == filamentoId && f.UsuarioId == usuarioId);
            if (filamento == null)
                throw new Exception("El filamento no existe o no pertenece al usuario.");

            decimal precioPorGramo = filamento.PrecioPorKg / 1000m;
            decimal costoFilamento = precioPorGramo * gramosFilamento;
            decimal costoLuz = impresora.CostoDeLuzPorHora * tiempoHoras;
            decimal costoDesgaste = (impresora.Precio * 0.05m) / 1000m * tiempoHoras;
            decimal subtotal = costoFilamento + costoLuz + costoDesgaste;
            decimal margenError = subtotal * 0.15m;

            decimal ivaFinal = iva ?? 16m;
            decimal ivaCalculado = Math.Round((subtotal + margenError) * (ivaFinal / 100m), 2);

            decimal total = subtotal + margenError + ivaCalculado;

            var cotizacion = new Cotizacion
            {
                UsuarioId = usuarioId,
                ImpresoraId = impresoraId,
                FilamentoId = filamentoId,
                Nombre = nombre,
                Descripcion = descripcion,
                TiempoHoras = tiempoHoras,
                FilamentoGr = gramosFilamento,
                CostoFilamento = costoFilamento,
                CostoLuz = costoLuz,
                CostoDesgarteImpresora = costoDesgaste,
                Subtotal = subtotal,
                MargenError15 = margenError,
                IVA = ivaCalculado,
                Total = total,
                ArchivoGcode = archivoGcode
            };

            await _context.Cotizaciones.AddAsync(cotizacion);
            await _context.SaveChangesAsync();

            return cotizacion;
        }


        public async Task<Cotizacion?> ObtenerCotizacionAsync(Guid cotizacionId, Guid usuarioId)
        {
            return await _context.Cotizaciones
                .FirstOrDefaultAsync(c => c.Id == cotizacionId && c.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Cotizacion>> ObtenerCotizacionesPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Cotizaciones
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Cotizacion?> ActualizarCotizacionAsync(
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
        )
        {
            var cot = await _context.Cotizaciones
                .FirstOrDefaultAsync(c => c.Id == cotizacionId && c.UsuarioId == usuarioId);

            if (cot == null)
                return null;

            // Obtén impresora y filamento
            var impresora = await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == impresoraId && i.UsuarioId == usuarioId);
            if (impresora == null) throw new Exception("La impresora no existe o no pertenece al usuario.");

            var filamento = await _context.Filamentos
                .FirstOrDefaultAsync(f => f.Id == filamentoId && f.UsuarioId == usuarioId);
            if (filamento == null) throw new Exception("El filamento no existe o no pertenece al usuario.");

            // Recalcular costos
            decimal precioPorGramo = filamento.PrecioPorKg / 1000m;
            decimal costoFilamento = precioPorGramo * gramosFilamento;
            decimal costoLuz = impresora.CostoDeLuzPorHora * tiempoHoras;
            decimal costoDesgaste = (impresora.Precio * 0.05m) / 1000m * tiempoHoras;
            decimal subtotal = costoFilamento + costoLuz + costoDesgaste;
            decimal margenError = subtotal * 0.15m;
            decimal ivaTotal = (iva ?? 16m) / 100m * subtotal;
            decimal total = subtotal + margenError + ivaTotal;

            // Actualizar cotización
            cot.ImpresoraId = impresoraId;
            cot.FilamentoId = filamentoId;
            cot.TiempoHoras = tiempoHoras;
            cot.FilamentoGr = gramosFilamento;
            cot.CostoFilamento = costoFilamento;
            cot.CostoLuz = costoLuz;
            cot.CostoDesgarteImpresora = costoDesgaste;
            cot.Subtotal = subtotal;
            cot.MargenError15 = margenError;
            cot.IVA = ivaTotal;
            cot.Total = total;
            cot.ArchivoGcode = archivoGcode ?? cot.ArchivoGcode;

            if (!string.IsNullOrWhiteSpace(nombre)) cot.Nombre = nombre;
            if (!string.IsNullOrWhiteSpace(descripcion)) cot.Descripcion = descripcion;

            await _context.SaveChangesAsync();
            return cot;
        }

        public async Task<bool> EliminarCotizacionAsync(Guid cotizacionId, Guid usuarioId)
        {
            var cot = await _context.Cotizaciones
                .FirstOrDefaultAsync(c => c.Id == cotizacionId && c.UsuarioId == usuarioId);

            if (cot == null)
                return false;

            _context.Cotizaciones.Remove(cot);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

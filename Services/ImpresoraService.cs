using Microsoft.EntityFrameworkCore;
using proyectoSistemaPropietarios.Data;

namespace proyectoSistemaPropietarios.Services
{
    public class ImpresoraService : IImpresoraService
    {
        private readonly AppDbContext _context;

        public ImpresoraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Impresora> CrearAsync(Impresora impresora, Guid usuarioId)
        {
            impresora.UsuarioId = usuarioId;

            _context.Impresoras.Add(impresora);
            await _context.SaveChangesAsync();

            return impresora;
        }

        public async Task<IEnumerable<Impresora>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Impresoras
                .Where(i => i.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Impresora?> ObtenerPorUsuarioYIdAsync(Guid usuarioId, Guid impresoraId)
        {
            return await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == impresoraId && i.UsuarioId == usuarioId);
        }

        public async Task<Impresora?> ActualizarAsync(
            Guid usuarioId,
            Guid impresoraId,
            Impresora impresoraActualizada
        )
        {
            var existente = await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == impresoraId && i.UsuarioId == usuarioId);

            if (existente == null)
                return null;

            existente.Modelo = impresoraActualizada.Modelo;
            existente.Marca = impresoraActualizada.Marca;
            existente.Precio = impresoraActualizada.Precio;
            existente.TipoExtrusion = impresoraActualizada.TipoExtrusion;
            existente.DiametroBoquilla = impresoraActualizada.DiametroBoquilla;
            existente.VelocidadDeImpresion = impresoraActualizada.VelocidadDeImpresion;
            existente.CostoDeLuzPorHora = impresoraActualizada.CostoDeLuzPorHora;
            existente.Foto = impresoraActualizada.Foto;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(Guid usuarioId, Guid impresoraId)
        {
            var impresora = await _context.Impresoras
                .FirstOrDefaultAsync(i => i.Id == impresoraId && i.UsuarioId == usuarioId);

            if (impresora == null)
                return false;

            _context.Impresoras.Remove(impresora);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

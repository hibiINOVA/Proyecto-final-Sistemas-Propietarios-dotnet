using Microsoft.EntityFrameworkCore;
using proyectoSistemaPropietarios.Data;

namespace proyectoSistemaPropietarios.Services
{
    public class FilamentoService : IFilamentoService
    {
        private readonly AppDbContext _context;

        public FilamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Filamento>> ObtenerPorUsuarioAsync(Guid usuarioId)
        {
            return await _context.Filamentos
                .Where(f => f.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Filamento?> ObtenerPorUsuarioYFilamentoAsync(Guid usuarioId, Guid filamentoId)
        {
            return await _context.Filamentos
                .FirstOrDefaultAsync(f => f.Id == filamentoId && f.UsuarioId == usuarioId);
        }

        public async Task<Filamento> CrearAsync(Filamento filamento)
        {
            _context.Filamentos.Add(filamento);
            await _context.SaveChangesAsync();
            return filamento;
        }

        public async Task<Filamento?> ActualizarAsync(Guid usuarioId, Guid filamentoId, Filamento filamentoActualizado)
        {
            var existente = await _context.Filamentos
                .FirstOrDefaultAsync(f => f.Id == filamentoId && f.UsuarioId == usuarioId);

            if (existente == null)
                return null;

            existente.Tipo = filamentoActualizado.Tipo;
            existente.Marca = filamentoActualizado.Marca;
            existente.Color = filamentoActualizado.Color;
            existente.Diametro = filamentoActualizado.Diametro;
            existente.PrecioPorKg = filamentoActualizado.PrecioPorKg;

            if (!string.IsNullOrWhiteSpace(filamentoActualizado.Foto))
                existente.Foto = filamentoActualizado.Foto;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarAsync(Guid usuarioId, Guid filamentoId)
        {
            var filamento = await _context.Filamentos
                .FirstOrDefaultAsync(f => f.Id == filamentoId && f.UsuarioId == usuarioId);

            if (filamento == null)
                return false;

            _context.Filamentos.Remove(filamento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

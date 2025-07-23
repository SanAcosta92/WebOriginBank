using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebOriginBank.Data;
using WebOriginBank.Interfaces;
using WebOriginBank.Models;
using WebOriginBank.Repository;

namespace WebOriginBank.Repository
{
    public class OperacionRepository : IOperacionRepository
    {
        private readonly AppDbContext _context;

        public OperacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarOperacionAsync(Operacion operacion)
        {
            if (!_context.Tarjeta.Any(t => t.Id == operacion.TarjetaId))
                throw new Exception("La tarjeta asociada no existe.");

            await _context.Operacion.AddAsync(operacion);
            await _context.SaveChangesAsync();
        }

        public async Task<Operacion> ObtenerOperacionAsync(int tarjetaId)
        {
            return await _context.Operacion
                .Where(o => o.TarjetaId == tarjetaId)
                .OrderByDescending(o => o.FechaHora)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Operacion>> ObtenerOperacionesPorTarjetaAsync(int tarjetaId)
        {
            return await _context.Operacion
                .Include(o => o.Tarjeta)
                .Where(o => o.TarjetaId == tarjetaId)
                .OrderByDescending(o => o.FechaHora)
                .ToListAsync();
        }
    }
}

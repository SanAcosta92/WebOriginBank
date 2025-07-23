using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebOriginBank.Data;
using WebOriginBank.Interfaces;
using WebOriginBank.Models;
using WebOriginBank.Repository;

namespace WebOriginBank.Repository
{
    public class TarjetaRepository : ITarjetaRepository
    {
        private readonly AppDbContext _context;

        public TarjetaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tarjeta> ObtenerNroAsync(string numero)
        {
            return await _context.Tarjeta
                .Include(t => t.Operacion)
                .FirstOrDefaultAsync(t => t.Nro == numero);
        }

        public async Task<bool> ValidarPinAsync(int tarjetaId, string pin)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(tarjetaId);
            return tarjeta != null && tarjeta.Pin == pin;
        }

        public async Task IncrementarIntentosFallidosAsync(int tarjetaId)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(tarjetaId);
            if (tarjeta != null)
            {
                tarjeta.Intentos++;
                if (tarjeta.Intentos >= 4)
                {
                    tarjeta.Bloqueada = true;
                }
            }
        }

        public async Task BloquearTarjetaAsync(int tarjetaId)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(tarjetaId);
            if (tarjeta != null)
            {
                tarjeta.Bloqueada = true;
            }
        }

        public async Task ResetearIntentosFallidosAsync(int tarjetaId)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(tarjetaId);
            if (tarjeta != null)
            {
                tarjeta.Intentos = 0;
            }
        }

        public async Task ActualizarBalanceAsync(int tarjetaId, decimal nuevoBalance)
        {
            var tarjeta = await _context.Tarjeta.FindAsync(tarjetaId);
            if (tarjeta != null)
            {
                tarjeta.Balance = nuevoBalance;
            }
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Tarjeta> ObtenerPorIdAsync(int id)
        {
            return await _context.Tarjeta
                .Include(t => t.Operacion)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

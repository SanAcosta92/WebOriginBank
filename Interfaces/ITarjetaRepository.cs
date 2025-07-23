using WebOriginBank.Models;
using System.Threading.Tasks;

namespace WebOriginBank.Interfaces
{
    public interface ITarjetaRepository
    {
        Task<Tarjeta> ObtenerPorIdAsync(int id);
        Task<Tarjeta> ObtenerNroAsync(string numero);
        Task<bool> ValidarPinAsync(int tarjetaId, string pin);
        Task IncrementarIntentosFallidosAsync(int tarjetaId);
        Task BloquearTarjetaAsync(int tarjetaId);
        Task ResetearIntentosFallidosAsync(int tarjetaId);
        Task ActualizarBalanceAsync(int tarjetaId, decimal nuevoBalance);
        Task GuardarCambiosAsync();
    }
}

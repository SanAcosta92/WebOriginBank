using WebOriginBank.Models;
using System.Threading.Tasks;

namespace WebOriginBank.Interfaces
{
    public interface IOperacionRepository
    {
        Task RegistrarOperacionAsync(Operacion operacion);
        Task<Operacion> ObtenerOperacionAsync(int tarjetaId);
        Task<List<Operacion>> ObtenerOperacionesPorTarjetaAsync(int tarjetaId);
    }
}

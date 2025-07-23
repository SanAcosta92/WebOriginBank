using Microsoft.AspNetCore.Mvc;
using WebOriginBank.Interfaces;
using WebOriginBank.Repository;

namespace WebOriginBank.Controllers
{
    public class OperacionController : Controller
    {
        private readonly ITarjetaRepository _tarjetaRepository;
        private readonly IOperacionRepository _operacionRepository;
        public OperacionController(ITarjetaRepository tarjetaRepository, IOperacionRepository operacionRepository)
        {
            _tarjetaRepository = tarjetaRepository;
            _operacionRepository = operacionRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Home");

            TempData.Keep("TarjetaId");
            return View();
        }

        [HttpPost]
        public IActionResult Balance()
        {
            return RedirectToAction("Index", "Balance");
        }

        [HttpPost]
        public IActionResult Retiro()
        {
            return RedirectToAction("Index", "Retiro");
        }

        [HttpPost]
        public IActionResult Reporte()
        {
            return RedirectToAction("MostrarReporte");
        }


        [HttpGet]

        public async Task<IActionResult> MostrarReporte()
        {
            if (TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Operacion");

            int tarjetaId = (int)TempData["TarjetaId"];
            var operaciones = await _operacionRepository.ObtenerOperacionesPorTarjetaAsync(tarjetaId);

            TempData.Keep("TarjetaId");

            return View("Reporte", operaciones);
        }
        
        public IActionResult Volver()
        {
            TempData.Keep("TarjetaId");
            TempData.Keep("NroTarjeta");
            TempData.Keep("NuevoBalance");
            TempData.Keep("MontoRetirado");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Salir()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

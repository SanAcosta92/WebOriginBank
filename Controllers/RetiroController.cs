using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebOriginBank.Interfaces;
using WebOriginBank.Models;

namespace WebOriginBank.Controllers
{
    public class RetiroController : Controller
    {
        private readonly ITarjetaRepository _tarjetaRepository;
        private readonly IOperacionRepository _operacionRepository;

        public RetiroController(ITarjetaRepository tarjetaRepository, IOperacionRepository operacionRepository)
        {
            _tarjetaRepository = tarjetaRepository;
            _operacionRepository = operacionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(TempData["TarjetaId"] == null)
               return RedirectToAction("Index", "Home");

            TempData.Keep("TarjetaId");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarRetiro(decimal monto)
        {
            if(TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Home");

            int tarjetaId = (int)TempData["TarjetaId"];
            TempData.Keep("TarjetaId");

            var tarjeta = await _tarjetaRepository.ObtenerPorIdAsync(tarjetaId);
            if(tarjeta == null)
                return RedirectToAction("Error", "Error", new { mensaje = "Tarjeta no encontrada.", returnUrl = Url.Action("Index", "Retiro")});

            if(monto <= 0 || monto > tarjeta.Balance)
                return RedirectToAction("Error", "Error", new { mensaje = "Monto inválido o insuficiente.", returnUrl = Url.Action("Index", "Retiro")});

            // Actualizar balance
            decimal nuevoBalance = tarjeta.Balance - monto;
            await _tarjetaRepository.ActualizarBalanceAsync(tarjetaId, nuevoBalance);
            await _tarjetaRepository.GuardarCambiosAsync();


            string nroTarjeta = (string)TempData["NroTarjeta"];
            TempData.Keep("NroTarjeta");

            var op = new Operacion();

            var operacion = new Operacion
            {
                Id = op.Id,
                TarjetaId = tarjetaId,
                FechaHora = DateTime.Now,
                TipoOperacion = "Retiro",
                Monto = monto
            };

            await _operacionRepository.RegistrarOperacionAsync(operacion);

            TempData["MontoRetirado"] = monto.ToString(CultureInfo.InvariantCulture);
            TempData.Keep("TarjetaId");

            return RedirectToAction("Resultado");
        }

        [HttpGet]
        public async Task<IActionResult> Resultado()
        {
            if(TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Home");

            int tarjetaId = (int)TempData["TarjetaId"];
            TempData.Keep("TarjetaId");

            var tarjeta = await _tarjetaRepository.ObtenerPorIdAsync(tarjetaId);

            if(TempData["MontoRetirado"] != null)
            {
                ViewBag.MontoRetirado = decimal.Parse((string)TempData["MontoRetirado"], CultureInfo.InvariantCulture);
            }
            else
            {
                ViewBag.MontoRetirado = 0m;
            }

            ViewBag.NuevoBalance = tarjeta.Balance;

            return View(tarjeta);
        }
    }
}

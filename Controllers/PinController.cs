using WebOriginBank.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebOriginBank.Controllers
{
    public class PinController : Controller
    {
        private readonly ITarjetaRepository _tarjetaRepository;
        public PinController(ITarjetaRepository tarjetaRepository)
        {
            _tarjetaRepository = tarjetaRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Home");

            int tarjetaId = (int)TempData["TarjetaId"];
            TempData.Keep("TarjetaId");

            ViewBag.TarjetaId = tarjetaId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidarPin(int tarjetaId, string pin)
        {
            if(pin == null)
            {
                ViewBag.Error = "Ingrese un PIN";
                return View("Index");
            }

            if(tarjetaId == 0 && TempData["TarjetaId"] != null)
            {
                tarjetaId = (int)TempData["TarjetaId"];
            }

            TempData["TarjetaId"] = tarjetaId; 
            TempData.Keep("TarjetaId");

            var valido = await _tarjetaRepository.ValidarPinAsync(tarjetaId, pin);

            if(!valido)
            {
                await _tarjetaRepository.IncrementarIntentosFallidosAsync(tarjetaId);
                await _tarjetaRepository.GuardarCambiosAsync();

                var tarjeta = await _tarjetaRepository.ObtenerPorIdAsync(tarjetaId);

                if(tarjeta.Intentos >= 4)
                {
                    return RedirectToAction("Error", "Error", new { mensaje = "La tarjeta fue bloqueada por múltiples intentos fallidos." });
                }

                ViewBag.Error = "PIN incorrecto. Intente nuevamente.";
                ViewBag.TarjetaId = tarjetaId;
                return View("Index");
            }

            await _tarjetaRepository.ResetearIntentosFallidosAsync(tarjetaId);
            await _tarjetaRepository.GuardarCambiosAsync();
 
            return RedirectToAction("Index", "Operacion");
        }

        [HttpPost]
        public IActionResult Salir()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

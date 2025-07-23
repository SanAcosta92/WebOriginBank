using WebOriginBank.Models;
using WebOriginBank.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebOriginBank.Controllers
{
    public class BalanceController : Controller
    {
        private readonly ITarjetaRepository _tarjetaRepository;
        private readonly IOperacionRepository _operacionRepository;

        public BalanceController(ITarjetaRepository tarjetaRepository, IOperacionRepository operacionRepository)
        {
            _tarjetaRepository = tarjetaRepository;
            _operacionRepository = operacionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(TempData["TarjetaId"] == null)
                return RedirectToAction("Index", "Home");

            int tarjetaId = (int)TempData["TarjetaId"];
            TempData.Keep("TarjetaId");

            string nroTarjeta = (string)TempData["NroTarjeta"];
            TempData.Keep("NroTarjeta");


            var tar = await _tarjetaRepository.ObtenerNroAsync(nroTarjeta); 

            if(nroTarjeta == null)
                return RedirectToAction("Error", "Error", new { mensaje = "Error al recuperar tarjeta." });

            var op = new Operacion();

            var operacion = new Operacion
            {               
                Id = op.Id,
                TarjetaId = tarjetaId,
                FechaHora = DateTime.Now,
                TipoOperacion = "Balance",
                Monto = null
            };

            await _operacionRepository.RegistrarOperacionAsync(operacion);

            return View(tar);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebOriginBank.Interfaces;
using WebOriginBank.Models;
using WebOriginBank.Repository;

namespace WebOriginBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITarjetaRepository _tarjetaRepository;

        public HomeController(ITarjetaRepository tarjetaRepository)
        {
            _tarjetaRepository = tarjetaRepository;


        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidarTarjeta(string numeroTarjeta)
        {
            if(numeroTarjeta != null)
            {
                var numeroSinGuiones = numeroTarjeta.Replace("-", "");

                var tarjeta = await _tarjetaRepository.ObtenerNroAsync(numeroSinGuiones);

                if(tarjeta != null)
                {
                    if(tarjeta.Bloqueada == false)
                    {
                        TempData["TarjetaId"] = tarjeta.Id;
                        TempData["NroTarjeta"] = tarjeta.Nro;
                        return RedirectToAction("Index", "Pin");
                    }

                    return RedirectToAction("Error", "Error", new { mensaje = "Tarjeta bloqueada.", returnUrl = Url.Action("Index", "Home") });
                }

                return RedirectToAction("Error", "Error", new { mensaje = "Tarjeta inválida.", returnUrl = Url.Action("Index", "Home") });
            }
            //return RedirectToAction("Error", "Error", new { mensaje = "Tarjeta inválida.", returnUrl = Url.Action("Index", "Home") });
            ViewBag.Error = "Debe ingresar un número de tarjeta.";
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return RedirectToAction("Error", "Error", new { mensaje = "Tarjeta inválida o bloqueada." });
        }

        public IActionResult Salir()
        {
            TempData.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Volver()
        {

            return RedirectToAction("Index", "Home");
        }
    }
}


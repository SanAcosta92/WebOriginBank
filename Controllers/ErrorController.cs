using Microsoft.AspNetCore.Mvc;

namespace WebOriginBank.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Error(string mensaje, string returnUrl)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home"): returnUrl; 
            
            return View();
        }

        [HttpPost]
        public IActionResult Volver()
        {
            var returnUrl = TempData["ReturnTo"]?.ToString();
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }
    }
}

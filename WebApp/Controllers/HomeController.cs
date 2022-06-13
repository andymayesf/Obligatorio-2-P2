using Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        Restaurante r = Restaurante.GetInstancia();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string nombrePersona = HttpContext.Session.GetString("LogueadoNombre");
            string apellidoPersona = HttpContext.Session.GetString("LogueadoApellido");

            if(nombrePersona != null)
            {
                ViewBag.msg = $"Bienvenido {nombrePersona} {apellidoPersona}!";
            } else
            {
                ViewBag.msg = $"Bienvenido!\nInicie sesion o registrese en caso de no tener un usuario.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ListadoPlatos()
        {
            List<Plato> lp = r.GetPlatosPorNombre();
            return View(lp);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

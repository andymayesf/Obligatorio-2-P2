using Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class ClienteController : Controller
    {
        Restaurante r = Restaurante.GetInstancia();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AltaCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AltaCliente(Cliente c, string user, string password)
        {
            if (r.AltaCliente(c, user, password))
            {
                Persona buscada = r.LogIn(user, password);
                HttpContext.Session.SetInt32("LogueadoId", buscada.Id);
                HttpContext.Session.SetString("LogueadoNombre", buscada.Nombre);
                HttpContext.Session.SetString("LogueadoApellido", buscada.Apellido);
                HttpContext.Session.SetString("LogueadoRol", buscada.GetType().Name);
                return RedirectToAction("Index","Home");
            } else
            {
                ViewBag.msg = "Error en el ingreso de datos. Vuelva a intentarlo";
            }

            return View();
        }



    }
}

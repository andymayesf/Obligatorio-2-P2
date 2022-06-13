using Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class PersonaController : Controller
    {
        Restaurante r = Restaurante.GetInstancia();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string user, string pass)
        {
            Persona buscada = r.LogIn(user, pass);
            if (buscada != null)
            {
                HttpContext.Session.SetInt32("LogueadoId", buscada.Id);
                HttpContext.Session.SetString("LogueadoNombre", buscada.Nombre);
                HttpContext.Session.SetString("LogueadoApellido", buscada.Apellido);
                HttpContext.Session.SetString("LogueadoRol", buscada.GetType().Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.msg = "Los datos ingresados son incorrectos";
                return View();
            }
        }
        
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}

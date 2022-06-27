using Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class RepartidorController : Controller
    {
        Restaurante r = Restaurante.GetInstancia();

        public IActionResult Index()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Repartidor")
            {
                int idRep = (int)HttpContext.Session.GetInt32("LogueadoId");
                List<Servicio> misEntregas = r.GetServiciosDeRepartidor(idRep);

                if(misEntregas.Count > 0)
                {
                    return View(misEntregas);
                } else
                {
                    ViewBag.msg = "No tiene entregas por el momento";
                    return View();
                }
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

using Clases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class ServicioController : Controller
    {
        Restaurante r = Restaurante.GetInstancia();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ServiciosClienteEntreFechas()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ServiciosClienteEntreFechas(DateTime f1, DateTime f2)
        {
            int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
            List<Servicio> comprasEntreFechas = r.GetServiciosClienteEntreFechas(idPers, f1, f2);

            if (comprasEntreFechas.Count > 0)
            {
                return View(comprasEntreFechas);
            }
            else
            {
                ViewBag.msg = "No hay compras en las fechas seleccionadas.";
                return View();
            }
        }

        public IActionResult Details(int idServicio)
        {
            Servicio b = r.GetServicioXId(idServicio);

            if (b.GetType().Name == "Delivery")
            {
                Delivery d = b as Delivery;
                ViewBag.deli = d;
                return View(b);
            }
            else
            {
                Local l = b as Local;
                ViewBag.loc = l;
                return View(b);
            }

        }

        public IActionResult MisComprasMasCaras()
        {
            int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
            List<Servicio> serviciosMasCaros = r.GetServiciosMasCaros(idPers);

            if (serviciosMasCaros.Count > 0)
            {
                return View(serviciosMasCaros);
            }
            else
            {
                ViewBag.msg = "No tiene compras a su nombre por el momento";
                return View();
            }
        }

        public IActionResult ServiciosConPlato()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult ServiciosConPlato(string nomPlato)
        {
            int idPLogueada = (int)HttpContext.Session.GetInt32("LogueadoId");
            Plato plato = r.GetPlatoXNombre(nomPlato);
            List<Servicio> servsConPlato = r.ServiciosContienenPlato(plato, idPLogueada);
            return View(servsConPlato);
        }


    }
}

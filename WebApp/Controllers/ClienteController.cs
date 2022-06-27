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
            if (HttpContext.Session.GetInt32("LogueadoId") == null)
            {
                return View();
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult AltaCliente(Cliente c, string user, string password)
        {
            if (r.AltaCliente(c, user, password))
            {
                c.Email = c.Email.ToLower();
                Persona buscada = r.LogIn(user, password);
                HttpContext.Session.SetInt32("LogueadoId", buscada.Id);
                HttpContext.Session.SetString("LogueadoNombre", buscada.Nombre);
                HttpContext.Session.SetString("LogueadoApellido", buscada.Apellido);
                HttpContext.Session.SetString("LogueadoRol", buscada.GetType().Name);
                return RedirectToAction("Index","Home");
            } else
            {
                ViewBag.msg = "Error en el ingreso de datos. Vuelva a intentarlo.";
            }

            return View();
        }

        public IActionResult Menu()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
            {
                return View(r.GetPlatos());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Likear(int idPlato)
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
            {
                r.Likear(idPlato);
                return RedirectToAction("Menu");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ServiciosClienteEntreFechas()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

        public IActionResult MisComprasMasCaras()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult MisServicios()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
            {
                int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
                List<Servicio> misServicios = r.GetServiciosDeCliente(idPers);

                if (misServicios.Count > 0)
                {
                    return View(misServicios);
                }
                else
                {
                    ViewBag.msg = "No tiene compras a su nombre por el momento";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}

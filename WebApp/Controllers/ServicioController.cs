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

        public IActionResult Details(int idServicio)
        {

            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (/*rol == "Cliente"*/rol != null)
            {
                Servicio b = r.GetServicioXId(idServicio);

                int idPersona = (int)HttpContext.Session.GetInt32("LogueadoId");
                if(r.ServicioEsDePersona(idServicio, idPersona))
                {
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
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }


        public IActionResult ServiciosConPlato()
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
        public IActionResult ServiciosConPlato(string nomPlato)
        {
            int idLog = (int)HttpContext.Session.GetInt32("LogueadoId");
            Plato plato = r.GetPlatoXNombre(nomPlato);

            if (plato != null)
            {
                List<Servicio> servsConPlato = r.ServiciosContienenPlato(plato, idLog);
                if (servsConPlato.Count > 0)
                {
                    return View(servsConPlato);
                } else
                {
                    ViewBag.msg = "No hay compras que incluyan el plato ingresado";
                    return View();
                }
            } else
            {
                ViewBag.msg = "No hay compras que incluyan el plato ingresado";
                return View();
            }
        }




        public IActionResult ServiciosMozoEntreFechas()
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Mozo")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult ServiciosMozoEntreFechas(DateTime f1, DateTime f2)
        {
            int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
            List<Local> trabajosEntreFechas = r.GetServiciosMozoEntreFechas(idPers, f1, f2);

            if (trabajosEntreFechas.Count > 0)
            {
                return View(trabajosEntreFechas);
            }
            else
            {
                ViewBag.msg = "No hay trabajos en las fechas seleccionadas.";
                return View();
            }
        }

        public IActionResult CrearServicio()
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


        public IActionResult SeleccionarTipoServicio()
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
        public IActionResult SeleccionarTipoServicio(int tServ)
        {
            if(tServ == 1) // Tipo local
            {
                return RedirectToAction("CrearServicioLocal");  
            } else if (tServ == 2)
            {
                return RedirectToAction("CrearServicioDelivery");
            }
            else
            {
                ViewBag.msg = "Debe seleccionar un tipo de servicio.";
                return View();
            }
        }

        public IActionResult CrearServicioLocal()
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
        public IActionResult CrearServicioLocal(int cantComensales)
        {
            int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
            Cliente c = r.GetClienteXId(idPers);

            if (r.AltaServicioLocal(new Local(new Random().Next(1, 50), r.MozoRandom(), cantComensales, c, DateTime.Now)))
            {
                return RedirectToAction("MisServicios", "Cliente");
            } else
            {
                ViewBag.msg = "Ingrese la cantidad de comensales.";
                return View();
            }

        }
        
        public IActionResult CrearServicioDelivery()
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
        public IActionResult CrearServicioDelivery(string direccion)
        {
            int idPers = (int)HttpContext.Session.GetInt32("LogueadoId");
            Cliente c = r.GetClienteXId(idPers);
            Repartidor rep = r.RepartidorRandom();
            int distMts = new Random().Next(1, 10000);


            if (r.AltaServicioDelivery(new Delivery(c, DateTime.Now,direccion,rep,distMts)))
            {
                return RedirectToAction("MisServicios", "Cliente");
            }
            else
            {
                ViewBag.msg = "Pedido invalido. Ingrese todos los datos.";
                return View();
            }
        }


        public IActionResult AgregarPlatos(int idServicio)
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            ViewBag.platos = r.GetPlatos();
            if (rol == "Cliente")
            {
                Servicio s = r.GetServicioXId(idServicio);
                if (s.Estado.Equals("Abierto"))
                {
                    if (s.Orden.Count == 0)
                    {
                        ViewBag.msg = "La órden no tiene platos por el momento.";
                    }
                
                    return View(s);
                } else
                {
                    return RedirectToAction("MisServicios", "Cliente");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public IActionResult AgregarPlatos(int idServicio, string plato, int cant)
        {
            ViewBag.platos = r.GetPlatos();
            Servicio s = r.GetServicioXId(idServicio);

            if (s.Orden.Count == 0)
            {
                ViewBag.msg = "La órden no tiene platos por el momento.";
            }

            if (plato != null && cant > 0)
            {
                Plato p = r.GetPlatoXNombre(plato);
                s.AgregarPlatoOrden(p, cant);
                return View(s);
            } else
            {
                ViewBag.errAgregarPlato = "Error de agregado de plato.";
                return View(s);
            }
        }

        public IActionResult CerrarServicio(int idServicio)
        {
            string rol = HttpContext.Session.GetString("LogueadoRol");
            if (rol == "Cliente")
            {
                // Que pueda cerrar si el servicio es del que esta logueado
                int idLog = (int)HttpContext.Session.GetInt32("LogueadoId");
                if (r.ServicioEsDePersona(idServicio, idLog))
                {
                    Servicio s = r.GetServicioXId(idServicio);
                    s.CerrarServicio();
                    return RedirectToAction("MisServicios", "Cliente");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        
    }
}

using Clases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public abstract class Servicio : IValidacion
    {
        public static int UltimoID { get; set; } = 1;
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Fecha { get; set; }

        //Al servicio le pasamos un objeto que contiene el plato pedido y la cantidad 
        public List<CantidadPlato> Orden = new List<CantidadPlato>();
        public string Estado { get; set; }
        public Servicio(Cliente cliente, DateTime fecha)
        {
            Id = UltimoID++;
            Cliente = cliente;
            Fecha = fecha;
            // Cuando creamos un nuevo servicio lo ponemos abierto para que se puedan seguir agregando platos
            Estado = "Abierto";
        }

        public Servicio()
        {
            Id = UltimoID++;
            Estado = "Abierto";
        }

        public List<CantidadPlato> GetOrden()
        {
            return Orden;
        }

        public void CerrarServicio()
        {
            Estado = "Cerrado";
        }

        //Agrega platos y su cantidad a las ordenes tanto locales como delivery
        public bool AgregarPlatoOrden(Plato p, int cantidad)
        {
            bool ret = false;
            if (cantidad > 0 && p != null)
            {
                if (!OrdenContienePlato(p))
                {
                    Orden.Add(new CantidadPlato(p, cantidad));
                }
                else
                {
                    bool platoEncontrado = false;
                    foreach (CantidadPlato cp in Orden) if (!platoEncontrado)
                        {
                            if (cp.Plato.Equals(p))
                            {
                                cp.Cantidad += cantidad;
                                platoEncontrado = true;
                            }
                        }
                }
                ret = true;
            }

            return ret;
        }

        public bool OrdenContienePlato(Plato p)
        {
            bool encontrado = false;

            foreach (CantidadPlato cp in Orden) if (!encontrado)
                {
                    if (cp.Plato.Equals(p))
                    {
                        encontrado = true;
                    }
                }

            return encontrado;
        }

        public abstract double CalcularPrecioFinal();

        public virtual bool EsValido()
        {
            return Cliente != null && Fecha <= DateTime.Now;
        }
    }
}

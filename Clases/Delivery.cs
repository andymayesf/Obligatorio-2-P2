using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Delivery : Servicio
    {
        public string DireccionDeEnvio { get;  set; }
        public Repartidor Repartidor { get; set; }
        public double DistanciaMetros { get; set; }

        public Delivery()
        {
            
        }

        public Delivery(Cliente cliente, DateTime fecha, string direccionDeEnvio, Repartidor repartidor, double distanciaMetros) : base(cliente, fecha)
        {
            DireccionDeEnvio = direccionDeEnvio;
            Repartidor = repartidor;
            DistanciaMetros = distanciaMetros;
        }

        // Redefinimos el metodo de Servicio para calcular el precio de la orden
        public override double CalcularPrecioFinal()
        {
            double precioFinal = 0;
            foreach (CantidadPlato cp in Orden)
            {
                precioFinal += cp.Plato.Precio * cp.Cantidad;
            }

            if (DistanciaMetros < 2000) {
                precioFinal += 50;
            } else if (DistanciaMetros < 7000){
                precioFinal += 50 + 10 * Math.Truncate((DistanciaMetros - 2000)/1000);
            } else { 
                precioFinal += 100;
            }
            
            return precioFinal;
        }

        public override string ToString()
        {
            return $"Repartidor: {Repartidor}, Fecha: {Fecha}. ";
        }

        public override bool EsValido()
        {
            return base.EsValido() && !String.IsNullOrWhiteSpace(DireccionDeEnvio) && Repartidor != null && DistanciaMetros > 0;
        }
    }
}
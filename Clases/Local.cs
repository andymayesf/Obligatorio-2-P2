using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Local : Servicio
    {
        public int NumeroMesa { get; set; }
        public Mozo Persona { get; set; }
        public int CantidadComensales { get; set; }
        public static double precioCubierto { get; set; } = 50;
    
        public Local()
        {

        }
       
        public Local(int numeroMesa, Mozo persona, int cantidadComensales, Cliente cliente, DateTime fecha) : base(cliente, fecha)
        {
            NumeroMesa = numeroMesa;
            Persona = persona;
            CantidadComensales = cantidadComensales;
        }

        // Redefinimos el metodo de Servicio para calcular el precio de la orden

        public override double CalcularPrecioFinal()
        {
            double precioFinal = 0;
            foreach (CantidadPlato cp in Orden)
            {
                precioFinal = precioFinal + (cp.Plato.Precio * cp.Cantidad);
            }
            
            precioFinal = precioFinal * 1.1 + precioCubierto * CantidadComensales;
            
            return precioFinal;
        }

        
    }
}

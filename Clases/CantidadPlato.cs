using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    //En vez de usar una lista de platos en donde se puede repetir un plato varias veces en una misma orden,
    //creamos una lista de CantidadPlato que contiene el plato elegido y la cantidad.
    public class CantidadPlato
    {
        public Plato Plato { get; set; }
        public int Cantidad { get; set; }

        
        public CantidadPlato (Plato plato, int cantidad)
        {
            Plato = plato;
            Cantidad = cantidad;
        }
    }
}

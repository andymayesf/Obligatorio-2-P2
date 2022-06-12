using Clases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Plato : IValidacion
    {
        private static int UltimoId { get; set; } = 1;
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public static double PrecioMinimo { get; set; }  =  120;

        public Plato(string nombre, double precio)
        {
            Id = UltimoId++;
            Nombre = nombre;
            Precio = precio;
        }

        public Plato() { Id = UltimoId++; }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Precio: {Precio} ";
        }

        public bool EsValido()
        {
            return !String.IsNullOrEmpty(Nombre) && Precio >= Plato.PrecioMinimo;
        }
    }
}

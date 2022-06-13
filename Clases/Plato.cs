using Clases.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Clases
{
    public class Plato : IValidacion, IComparable<Plato>
    {
        private static int UltimoId { get; set; } = 1;
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Likes { get; set; }
        public static double PrecioMinimo { get; set; }  =  120;
        
        public Plato(string nombre, double precio)
        {
            Id = UltimoId++;
            Nombre = nombre;
            Precio = precio;
            Likes = 0;
        }

        public Plato() 
        { 
            Id = UltimoId++;
            Likes = 0;
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Precio: {Precio} ";
        }

        public bool EsValido()
        {
            return !String.IsNullOrEmpty(Nombre) && Precio >= Plato.PrecioMinimo;
        }

        public int CompareTo([AllowNull] Plato other)
        {
            return Nombre.CompareTo(other.Nombre);
        }
    }
}

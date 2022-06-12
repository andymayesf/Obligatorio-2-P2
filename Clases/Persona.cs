using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public abstract class Persona 
    {
        private static int UltimoId { get; set; } = 1;
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public Persona (string nombre, string apellido) {
            Id = UltimoId++;
            Nombre = nombre;
            Apellido = apellido;
        }

        public Persona() {
            Id = UltimoId++;
        }   
    }
}

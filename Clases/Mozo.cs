using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
   public class Mozo : Persona
    {
        public static int UltimoNroFuncionario { get; set; } = 1;
        public int NroFuncionario { get; set; }

        public Mozo() { NroFuncionario = UltimoNroFuncionario++; }

        public Mozo(string nombre, string apellido) : base(nombre, apellido)
        {
            NroFuncionario = UltimoNroFuncionario++;
            Nombre = nombre;
            Apellido = apellido;
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Apellido: {Apellido}, Numero de funcionario: {NroFuncionario}  ";
        }
    }
}

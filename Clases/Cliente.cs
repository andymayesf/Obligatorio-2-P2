using Clases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Cliente : Persona, IComparable<Cliente>, IValidacion
    {
        public static int UltimoId { get; set; } = 1;
        public int IdCliente { get; set; }
        public string Email { get; set; }

        public Cliente()
        {

        }
        
        public Cliente(string nombre, string apellido, string email): base(nombre, apellido)
        {
            IdCliente = UltimoId;
            UltimoId++;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
        }


        public override string ToString()
        {
            return $"Nombre: {Nombre}, Apellido: {Apellido}, Email: {Email}. ";
        }

        //Definimos el criterio de orden para cliente. Tomamos como prioridad el orden alfabetico del apellido y en su defecto el del nombre.
        public int CompareTo(Cliente other)
        {
            //Si es mayor a 0, es porque el apellido del objeto pasado por parametro precede al de la instancia de Cliente
            if (Apellido.CompareTo(other.Apellido) > 0)
            {
                return 1;
            }
            //Si es menor a 0, el que precede es el objeto de la instancia Cliente
            else if (Apellido.CompareTo(other.Apellido) < 0)
            {
                return -1;
            }
            //Si tienen el mismo apellido comparamos el nombre con el mismo criterio
            else
            {
                if (Nombre.CompareTo(other.Nombre) > 0)
                {
                    return 1;
                }
                else if (Nombre.CompareTo(other.Nombre) < 0)
                {
                    return -1;
                }
                //Si coinciden apellido y nombre tomamos como que ninguno precede al otro
                else
                {
                    return 0;
                }
            }
        }

        public bool EsValido()
        {
            return !String.IsNullOrWhiteSpace(Email);
        }
    }
}

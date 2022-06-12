using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Usuario
    {
        public Persona Persona { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public Usuario() { }

        public Usuario(Persona persona, string user, string password)
        {
            Persona = persona;
            User = user;
            Password = password;
        }
    }
}

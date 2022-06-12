using Clases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Repartidor : Persona, IValidacion
    {
        public Vehiculo TipoVehiculo { get; set; }
        
        
        public enum Vehiculo 
        {
            Moto,
            Bicicleta,
            APie
        }

        public Repartidor(string nombre, string apellido, Vehiculo tipoVehiculo) : base(nombre, apellido) {
            Nombre = nombre;
            Apellido = apellido;
            TipoVehiculo = tipoVehiculo;
        }

        public override string ToString() {
            return $"{Nombre} {Apellido}, Tipo de Vehiculo: {TipoVehiculo}.";
        }
        
        //Redefinimos el Equals para que se compare el ID de persona
        public override bool Equals(object obj)
        {
            return obj is Repartidor repartidor &&
                   Id == repartidor.Id;
        }

        public bool EsValido()
        {
            return TipoVehiculo.Equals(Vehiculo.Moto) || TipoVehiculo.Equals(Vehiculo.Bicicleta) || TipoVehiculo.Equals(Vehiculo.APie);
        }
    }
}

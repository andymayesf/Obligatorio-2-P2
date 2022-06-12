using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Restaurante
    {
        #region Singleton
        private static Restaurante instancia = null;

        public static Restaurante GetInstancia()
        {
            if (instancia == null)
            {
                instancia = new Restaurante();
            }
            return instancia;
        }
        #endregion

        //---------- LISTAS ------------

        //Lista de platos del restaurante
        private List<Plato> platos = new List<Plato>();
        //Lista de personas que contiene clientes, mozos y repartidores
        private List<Persona> personas = new List<Persona>();
        //Lista de servicios que contiene servicios locales y deliverys
        private List<Servicio> servicios = new List<Servicio>();
        //Lista de usuarios registrados en el sistema
        private List<Usuario> usuarios = new List<Usuario>();

        //-------------------------------------
        public Restaurante()
        {
            PrecargaDatos();
        }

        //-------- GETS ----------
        public List<Persona> GetPersona() { return personas; }

        public List<Plato> GetPlatos() { return platos; }

        public List<Servicio> GetServicios() { return servicios; }

        public List<Usuario> GetUsuarios() { return usuarios; }

        //Filtra los clientes de la lista de personas
        public List<Cliente> GetClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            foreach (Persona p in personas)
            {
                if (p is Cliente)
                {
                    Cliente pAux = p as Cliente;
                    clientes.Add(pAux);
                }
            }

            return clientes;
        }

        //Filtra los mozos de la lista de personas
        public List<Mozo> GetMozos()
        {
            List<Mozo> mozos = new List<Mozo>();

            foreach (Persona p in personas)
            {
                if (p is Mozo)
                {
                    Mozo pAux = p as Mozo;
                    mozos.Add(pAux);
                }
            }

            return mozos;
        }

        //Filtra los repartidores de la lista de personas
        public List<Repartidor> GetRepartidores()
        {
            List<Repartidor> repartidores = new List<Repartidor>();

            foreach (Persona p in personas)
            {
                if (p is Repartidor)
                {
                    Repartidor pAux = p as Repartidor;
                    repartidores.Add(pAux);
                }
            }

            return repartidores;
        }

        //Devuelve una lista ordenada (usando el CompareTo redefinido en la clase cliente) de clientes
        public List<Cliente> GetClientesPorApellido()
        {
            List<Cliente> clientesAp = GetClientes();
            clientesAp.Sort();
            return clientesAp;
        }

        //Filtra los servicios que son delivery, estan comprendidos en el rango de fechas y corresponden al repartidor con el nombre seleccionado

        public List<Delivery> GetOrdenDeliveryPorFechasYRepartidor(DateTime f1, DateTime f2, Repartidor repartidor)
        {
            List<Delivery> ret = new List<Delivery>();

            foreach (Servicio s in servicios)
            {
                if (s is Delivery)
                {
                    Delivery sAux = s as Delivery;
                    if (s.Fecha >= f1 && s.Fecha <= f2 && repartidor.Equals(sAux.Repartidor))
                    {
                        ret.Add(sAux);
                    }
                }
            }
            return ret;
        }

        //---------- ALTAS ------------

        //------ PLATO ------

        //Agrega platos al sistema controlando que los datos ingresados sean correctos (nombre distinto de nulo y precio mayor al precio minimo de plato)
        //public Plato AltaPlato(string nombre, double precio)
        //{
        //    Plato nuevo = null;

        //    if (!String.IsNullOrEmpty(nombre) && precio >= Plato.PrecioMinimo)
        //    {
        //        nuevo = new Plato(nombre, precio);
        //        platos.Add(nuevo);
        //    }

        //    return nuevo;
        //}

        public bool AltaPlato(Plato p)
        {
            bool ret = false;

            if (p.EsValido())
            {
                platos.Add(p);
                ret = true;
            }

            return ret;
        }


        //------ CLIENTE ------

        //Agrega clientes al sistema controlando que los datos cumplan con ciertas restricciones.
        //public Cliente AltaCliente(string nombre, string apellido, string email, string password)
        //{
        //    Cliente nuevo = null;

        //    if (ValidarNombreYApellido(nombre, apellido) && ValidarEmail(email) && ValidarPassword(password))
        //    {
        //        nuevo = new Cliente(nombre, apellido, email, password);
        //        personas.Add(nuevo);
        //    }

        //    return nuevo;
        //}

        public bool AltaCliente(Cliente c, string user, string password)
        {
            bool ret = false;

            if (c.EsValido() && ValidarNombreYApellido(c.Nombre, c.Apellido) && ValidarEmail(c.Email) && ValidarPassword(password))
            {
                //Crear usuario y agregar a usuarios
                personas.Add(c);
                usuarios.Add(new Usuario(c, user, password));
                ret = true;
            }

            return ret;
        }

        //------ MOZO ------
        //Agrega mozos controlando que nombre y apellido cumplan con las restricciones
        //public Mozo AltaMozo(string nombre, string apellido)
        //{
        //    Mozo nuevo = null;

        //    if (ValidarNombreYApellido(nombre, apellido))
        //    {
        //        nuevo = new Mozo(nombre, apellido);
        //        personas.Add(nuevo);
        //    }

        //    return nuevo;
        //}

        public bool AltaMozo(Mozo m, string user, string password)
        {
            bool ret = false;

            if (ValidarNombreYApellido(m.Nombre, m.Apellido) && ValidarUser(user) && ValidarPassword(password))
            {
                personas.Add(m);
                usuarios.Add(new Usuario(m, user, password));
                ret = true;
            }

            return ret;
        }

        //------ REPARTIDOR ------
        //Agrega repartidores cumpliendo con las reglas requeridas
        //public Repartidor AltaRepartidor(string nombre, string apellido, Repartidor.Vehiculo tp)
        //{
        //    Repartidor nuevo = null;

        //    if (ValidarNombreYApellido(nombre, apellido))
        //    {
        //        nuevo = new Repartidor(nombre, apellido, tp);
        //        personas.Add(nuevo);
        //    }

        //    return nuevo;
        //}

        public bool AltaRepartidor(Repartidor r, string user, string password)
        {
            bool ret = false;

            if (r.EsValido() && ValidarNombreYApellido(r.Nombre, r.Apellido) && ValidarUser(user) && ValidarPassword(password))
            {
                personas.Add(r);
                usuarios.Add(new Usuario(r, user, password));
                ret = true;
            }

            return ret;
        }

        //------ SERVICIO LOCAL ------
        //Agrega servicios locales controlando que los datos pasados sean correctos
        public Local AltaServicioLocal(int numeroMesa, Mozo mozo, int cantidadComensales, Cliente cliente, DateTime fecha)
        {
            Local nuevo = null;

            if (numeroMesa > 0 && mozo != null && cantidadComensales >= 1 && cliente != null && fecha <= DateTime.Now)
            {
                nuevo = new Local(numeroMesa, mozo, cantidadComensales, cliente, fecha);
                servicios.Add(nuevo);
            }

            return nuevo;
        }

        //------ SERVICIO DELIVERY ------
        //Agrega servicios delivery controlando que los datos pasados sean correctos. 
        public Delivery AltaServicioDelivery(Cliente cliente, DateTime fecha, string direccionDeEnvio, Repartidor repartidor, double distanciaMetros)
        {
            Delivery nuevo = null;

            if (cliente != null && fecha <= DateTime.Now && !String.IsNullOrEmpty(direccionDeEnvio) && repartidor != null && distanciaMetros > 0)
            {
                nuevo = new Delivery(cliente, fecha, direccionDeEnvio, repartidor, distanciaMetros);
                servicios.Add(nuevo);
            }

            return nuevo;
        }

        //-------------------------- VALIDACIONES --------------------------

        //-------- VALIDAR NOMBRE Y APELLIDO ----------

        // Valida el nombre y appellido pasados revisando que no esten vacíos y ni que contengan números.
        public bool ValidarNombreYApellido(string nombre, string apellido)
        {
            return !String.IsNullOrWhiteSpace(nombre) && !String.IsNullOrWhiteSpace(apellido) && !TieneNumero(nombre) && !TieneNumero(apellido);
        }

        //-------- VALIDAR EMAIL ----------

        // Valida el user para la clase usuario
        public bool ValidarUser(string user)
        {
            return !String.IsNullOrWhiteSpace(user);
        }


        // Valida el email ingresado. Debe contener un arroba y que este no se encuentre en la primera ni en la ultima posicion.
        public bool ValidarEmail(string email)
        {
            int posArroba = email.IndexOf("@");
            return posArroba != email.Length - 1 && posArroba != -1 && posArroba != 0;
        }

        //-------- CONTRASEÑA ----------

        // Valida la password. La password debe tener como mínimo 6 caracteres, incluyendo al menos una mayúscula, una minúscula y un número.
        public bool ValidarPassword(string password)
        {
            return TieneMayuscula(password) && TieneMinuscula(password) && TieneNumero(password) && password.Length >= 6;
        }

        // Revisa que el texto pasado por parametro contenga un numero
        public bool TieneNumero(string texto)
        {
            bool tieneNum = false;

            foreach (Char c in texto) if (!tieneNum)
                {
                    if (Char.IsDigit(c))
                    {
                        tieneNum = true;
                    }
                }

            return tieneNum;
        }

        // Revisa que el texto pasado por parametro contenga una letra mayuscula
        public bool TieneMayuscula(string texto)
        {
            bool tieneMay = false;

            foreach (Char c in texto) if (!tieneMay)
                {
                    if (Char.IsUpper(c))
                    {
                        tieneMay = true;
                    }
                }

            return tieneMay;
        }

        // Revisa que el texto pasado por parametro contenga una letra minuscula
        public bool TieneMinuscula(string texto)
        {
            bool tieneMin = false;

            foreach (Char c in texto) if (!tieneMin)
            {
                if (Char.IsLower(c)) //is lower confirma si el caracter es minus
                {
                    tieneMin = true;
                }
            }

            return tieneMin;
        }



        //-------- PRECARGA DE DATOS ----------
        // Realiza la precarga de platos, clientes, mozos, repartidores
        public void PrecargaDatos()
        {

            //######### PRECARGA DE PLATOS ############
            // Precarga Platos

            AltaPlato(new Plato("Fideos con tuco", 320));            /*1*/
            AltaPlato(new Plato("Metro de Pizza", 520));             /*2*/
            AltaPlato(new Plato("Hamburguesa", 450));                /*3*/
            AltaPlato(new Plato("Papas fritas", 150));               /*4*/
            AltaPlato(new Plato("Ensalada rusa", 230));              /*5*/
            AltaPlato(new Plato("Postre de dulce de leche", 160));   /*6*/
            AltaPlato(new Plato("Helado de chocolate", 130));        /*7*/
            AltaPlato(new Plato("Chivito al pan", 300));             /*8*/
            AltaPlato(new Plato("Milanesa de soja", 290));           /*9*/
            AltaPlato(new Plato("Chivito vegetariano", 350));        /*10*/

            //Precarga Clientes (string nombre, string apellido, string email, string password) 

            AltaCliente(new Cliente("Elvis", "Presley", "elvispres77@gmail.com"), "epresley", "Elvis35");       /*1*/
            AltaCliente(new Cliente("Luis", "Miguel", "luismi70@gmail.com"), "lmiguel", "Luismi70");            /*2*/
            AltaCliente(new Cliente("Katy", "Perry", "katyp84@gmail.com"), "kperry", "Katy84");                 /*3*/
            AltaCliente(new Cliente("Anuel", "AA", "anuel@adinet.com.uy"), "anuelaa", "Manu123");               /*4*/
            AltaCliente(new Cliente("Don", "Omar", "domar@gmail.com"), "domar", "AltoCliente5");                /*5*/
            AltaCliente(new Cliente("Karol", "Giraldo", "karolg@gmail.com"), "kgiraldo", "Cgir123");            /*6*/
            AltaCliente(new Cliente("Kurt", "Cobain", "curcobain@gmail.com"), "kcobain", "Kurtcob94");          /*7*/
            AltaCliente(new Cliente("Amy", "Winehouse", "amiwin3@gmail.com"), "awinehouse", "Aminwn2");         /*8*/
            AltaCliente(new Cliente("Harry", "Styles", "harrsty@hotmail.com"), "hestilos", "Harry10");          /*9*/
            AltaCliente(new Cliente("Jose", "Balvin", "jbalvin@hotmail.com"), "jbalvin", "Guti15");             /*10*/

            //Precarga Mozos (nombre, apellido)

            AltaMozo(new Mozo("Mario", "Bros"), "mbros", "maritoB1");             /*1*/
            AltaMozo(new Mozo("Princesa", "Peach"), "ppeach", "princesaP2");      /*2*/
            AltaMozo(new Mozo("Toad", "Kinopio"), "tkinopio", "tKinop3");         /*3*/
            AltaMozo(new Mozo("Coopa", "Troopa"), "ctroopa", "cTroop4");          /*4*/
            AltaMozo(new Mozo("Bowser", "Elmalo"), "belmalo", "BMalo5");          /*5*/

            //Precarga Repartidores (nombre, apellido, tipo de vehiculo)

            AltaRepartidor(new Repartidor("Johnny", "Depp", Repartidor.Vehiculo.Moto), "jdepp", "jDepp1");             /*1*/
            AltaRepartidor(new Repartidor("Will", "Smith", Repartidor.Vehiculo.Bicicleta), "wsmith", "wSmith2");         /*2*/
            AltaRepartidor(new Repartidor("Chris", "Rock", Repartidor.Vehiculo.APie), "crock", "cRock3");              /*3*/
            AltaRepartidor(new Repartidor("Jennifer", "Lopez", Repartidor.Vehiculo.Bicicleta), "jlopez", "jLopez4");     /*4*/
            AltaRepartidor(new Repartidor("Sandra", "Bullock", Repartidor.Vehiculo.APie), "sbullock", "sBull5");          /*5*/

            //Precarga Servicios 

            //TIPO LOCAL

            //AltaMozo("Bowser", "Elmalo");   /*5*/
            //AltaCliente("Elvis", "Presley", "elvispres77@gmail.com", "Elvis35");    /*1*/
            Local l1 = AltaServicioLocal(8, GetMozos()[4], 3, GetClientes()[0], new DateTime(2022, 01, 25));
            //AltaPlato("Fideos con tuco", 320);            /*1*/
            l1.AgregarPlatoOrden(GetPlatos()[0], 1);
            //AltaPlato("Ensalada rusa", 230);              /*5*/
            l1.AgregarPlatoOrden(GetPlatos()[4], 2);
            //AltaPlato("Helado de chocolate", 130);        /*7*/
            l1.AgregarPlatoOrden(GetPlatos()[6], 2);

            //AltaMozo("Bowser", "Elmalo");   /*5*/
            //AltaCliente("Luis", "Miguel", "luismi70@gmail.com", "Luismi70");        /*2*/
            Local l2 = AltaServicioLocal(4, GetMozos()[4], 1, GetClientes()[1], new DateTime(2022, 04, 5));
            //AltaPlato("Chivito al pan", 300);             /*8*/
            l2.AgregarPlatoOrden(GetPlatos()[7], 1);
            //AltaPlato("Helado de chocolate", 130);        /*7*/
            l2.AgregarPlatoOrden(GetPlatos()[6], 1);
            //AltaPlato("Postre de dulce de leche", 160);   /*6*/
            l2.AgregarPlatoOrden(GetPlatos()[5], 1);

            //AltaMozo("Mario", "Bros");      /*1*/
            //AltaCliente("Katy", "Perry", "katyp84@gmail.com", "Katy84");            /*3*/
            Local l3 = AltaServicioLocal(3, GetMozos()[0], 3, GetClientes()[2], new DateTime(2022, 02, 2));
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            l3.AgregarPlatoOrden(GetPlatos()[1], 2);
            //AltaPlato("Papas fritas", 150);               /*4*/
            l3.AgregarPlatoOrden(GetPlatos()[3], 2);

            //AltaMozo("Coopa", "Troopa");    /*4*/
            //AltaCliente("Anuel", "AA", "anuel@adinet.com.uy", "Manu123");           /*4*/
            Local l4 = AltaServicioLocal(1, GetMozos()[3], 5, GetClientes()[3], new DateTime(2021, 03, 12));
            //AltaPlato("Chivito vegetariano", 350);        /*10*/
            l4.AgregarPlatoOrden(GetPlatos()[9], 1);
            //AltaPlato("Milanesa de soja", 290);           /*9*/
            l4.AgregarPlatoOrden(GetPlatos()[8], 1);
            //AltaPlato("Ensalada rusa", 230);              /*5*/
            l4.AgregarPlatoOrden(GetPlatos()[4], 1);
            //AltaPlato("Papas fritas", 150);               /*4*/
            l4.AgregarPlatoOrden(GetPlatos()[3], 1);
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            l4.AgregarPlatoOrden(GetPlatos()[1], 1);

            //AltaMozo("Toad", "Kinopio");    /*3*/
            //AltaCliente("Don", "Omar", "dOmar@asol.com", "AltoCliente5");           /*5*/
            Local l5 = AltaServicioLocal(5, GetMozos()[2], 2, GetClientes()[4], new DateTime(2022, 01, 14));
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            l5.AgregarPlatoOrden(GetPlatos()[1], 2);



            //TIPO DELIVERY

            //AltaRepartidor("Will", "Smith", Repartidor.Vehiculo.Bicicleta);         /*2*/
            //AltaCliente("Luis", "Miguel", "luismi70@gmail.com", "Luismi70");        /*2*/
            Delivery d1 = AltaServicioDelivery(GetClientes()[1], new DateTime(2022, 4, 22), "Mercedez y Cuareim 123", GetRepartidores()[1], 1400);
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            d1.AgregarPlatoOrden(GetPlatos()[1], 2);
            //AltaPlato("Hamburguesa", 450);                /*3*/
            d1.AgregarPlatoOrden(GetPlatos()[2], 1);
            //AltaPlato("Papas fritas", 150);               /*4*/
            d1.AgregarPlatoOrden(GetPlatos()[3], 1);

            //AltaRepartidor("Sandra", "Bullock", Repartidor.Vehiculo.APie);          /*5*/
            //AltaCliente("Katy", "Perry", "katyp84@gmail.com", "Katy84");            /*3*/
            Delivery d2 = AltaServicioDelivery(GetClientes()[2], new DateTime(2022, 3, 5), "Priamo y Rivera 1575", GetRepartidores()[4], 2500);
            //AltaPlato("Hamburguesa", 450);                /*3*/
            d2.AgregarPlatoOrden(GetPlatos()[2], 3);
            //AltaPlato("Papas fritas", 150);               /*4*/
            d2.AgregarPlatoOrden(GetPlatos()[4], 2);
            //AltaPlato("Postre de dulce de leche", 160);   /*6*/
            d2.AgregarPlatoOrden(GetPlatos()[5], 4);

            //AltaRepartidor("Sandra", "Bullock", Repartidor.Vehiculo.APie);          /*5*/
            //AltaCliente("Anuel", "AA", "anuel@adinet.com.uy", "Manu123");           /*4*/
            Delivery d3 = AltaServicioDelivery(GetClientes()[3], new DateTime(2022, 2, 28), "18 de Julio y Ejido 456", GetRepartidores()[4], 200);
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            d3.AgregarPlatoOrden(GetPlatos()[1], 1);
            //AltaPlato("Hamburguesa", 450);                /*3*/
            d3.AgregarPlatoOrden(GetPlatos()[2], 1);
            //AltaPlato("Papas fritas", 150);               /*4*/
            d3.AgregarPlatoOrden(GetPlatos()[3], 1);
            //AltaPlato("Ensalada rusa", 230);              /*5*/
            d3.AgregarPlatoOrden(GetPlatos()[4], 1);
            //AltaPlato("Postre de dulce de leche", 160);   /*6*/
            d3.AgregarPlatoOrden(GetPlatos()[5], 1);

            //AltaRepartidor("Chris", "Rock", Repartidor.Vehiculo.APie);              /*3*/
            //AltaCliente("Don", "Omar", "dOmar@asol.com", "AltoCliente5");           /*5*/
            Delivery d4 = AltaServicioDelivery(GetClientes()[4], new DateTime(2021, 4, 27), "Bv Artigas y Av Italia 789", GetRepartidores()[2], 4100);
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            d4.AgregarPlatoOrden(GetPlatos()[1], 2);
            //AltaPlato("Hamburguesa", 450);                /*3*/
            d4.AgregarPlatoOrden(GetPlatos()[2], 1);
            //AltaPlato("Papas fritas", 150);               /*4*/
            d4.AgregarPlatoOrden(GetPlatos()[3], 1);

            //AltaRepartidor("Jennifer", "Lopez", Repartidor.Vehiculo.Bicicleta);     /*4*/
            //AltaCliente("Karol", "Giraldo", "karolg@gmail.com", "Cgir123");         /*6*/
            Delivery d5 = AltaServicioDelivery(GetClientes()[5], new DateTime(2022, 1, 31), "Justicia y Amezaga 159", GetRepartidores()[3], 1200);
            //AltaPlato("Metro de Pizza", 520);             /*2*/
            d5.AgregarPlatoOrden(GetPlatos()[1], 4);
            //AltaPlato("Hamburguesa", 450);                /*3*/
            d5.AgregarPlatoOrden(GetPlatos()[2], 3);
            //AltaPlato("Papas fritas", 150);               /*4*/
            d5.AgregarPlatoOrden(GetPlatos()[3], 2);
            //AltaPlato("Ensalada rusa", 230);              /*5*/
            d5.AgregarPlatoOrden(GetPlatos()[4], 5);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Clases
{
    public class Restaurante
    {
        #region Singleton
        private static Restaurante instancia = null;

        public static Restaurante GetInstancia() {
            if (instancia == null) {
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
        public Restaurante() { PrecargaDatos(); }

        //-------- GETS ----------
        public List<Persona> GetPersonas() { return personas; }

        public List<Plato> GetPlatos() { return platos; }

        public List<Servicio> GetServicios() { return servicios; }

        public List<Usuario> GetUsuarios() { return usuarios; }

        //Filtra los clientes de la lista de personas
        public List<Cliente> GetClientes() {
            List<Cliente> clientes = new List<Cliente>();

            foreach (Persona p in personas) {

                if (p is Cliente) {
                    Cliente pAux = p as Cliente;
                    clientes.Add(pAux);
                }
            }

            return clientes;
        }

        //Filtra los mozos de la lista de personas
        public List<Mozo> GetMozos() {
            List<Mozo> mozos = new List<Mozo>();

            foreach (Persona p in personas) {
                if (p is Mozo) {
                    Mozo pAux = p as Mozo;
                    mozos.Add(pAux);
                }
            }

            return mozos;
        }

        //Filtra los repartidores de la lista de personas
        public List<Repartidor> GetRepartidores() {
            List<Repartidor> repartidores = new List<Repartidor>();

            foreach (Persona p in personas) {
                if (p is Repartidor) {
                    Repartidor pAux = p as Repartidor;
                    repartidores.Add(pAux);
                }
            }

            return repartidores;
        }

        //Devuelve una lista ordenada (usando el CompareTo redefinido en la clase cliente) de clientes
        public List<Cliente> GetClientesPorApellido() {
            List<Cliente> clientesAp = GetClientes();
            clientesAp.Sort();
            return clientesAp;
        }


        //Filtra los servicios que son delivery, estan comprendidos en el rango de fechas y corresponden al repartidor con el nombre seleccionado

        public List<Delivery> GetOrdenDeliveryPorFechasYRepartidor(DateTime f1, DateTime f2, Repartidor repartidor) {
            List<Delivery> ret = new List<Delivery>();

            foreach (Servicio s in servicios) {
                if (s is Delivery) {
                    Delivery sAux = s as Delivery;
                    if (s.Fecha >= f1 && s.Fecha <= f2 && repartidor.Equals(sAux.Repartidor)) {
                        ret.Add(sAux);
                    }
                }
            }
            return ret;
        }

        //---------- ALTAS ------------

        //------ PLATO ------

        public bool AltaPlato(Plato p) {
            bool ret = false;

            if (p.EsValido()) {
                platos.Add(p);
                ret = true;
            }

            return ret;
        }

        //------ CLIENTE ------
        // Alta de cliente con las validaciones correspondientes

        public bool AltaCliente(Cliente c, string user, string password) {
            bool ret = false;

            if (c.EsValido() && ValidarNombreYApellido(c.Nombre, c.Apellido) && ValidarEmail(c.Email) 
                && ValidarPassword(password) && !ExisteEmail(c.Email) && !ExisteUser(user)) {
                
                //Crear usuario y agregar a usuarios
                personas.Add(c);
                usuarios.Add(new Usuario(c, user, password));
                ret = true;
            }

            return ret;
        }

        // Retorna si ya existe un Usuario con un user pasado por parametro
        public bool ExisteUser(string user) {
            bool existe = false;

            foreach(Usuario u in usuarios) if (!existe) {
                if (u.User.Equals(user)) {
                    existe = true;
                }
            }

            return existe;
        }

        // Retorna si ya existe un cliente con un email pasado por parametro
        public bool ExisteEmail(string email) {
            bool existe = false;
            string emailLowerCase = email.ToLower();

            foreach (Cliente c in GetClientes()) if (!existe) {
                
                    if (c.Email.Equals(emailLowerCase)) {
                    existe = true;
                }
            }

            return existe;
        }

        //------ MOZO ------
        // Alta de mozo con las validaciones correspondientes

        public bool AltaMozo(Mozo m, string user, string password) {
            bool ret = false;

            if (ValidarNombreYApellido(m.Nombre, m.Apellido) && ValidarUser(user) && ValidarPassword(password) && !ExisteUser(user)) {
                personas.Add(m);
                usuarios.Add(new Usuario(m, user, password));
                ret = true;
            }

            return ret;
        }

        //------ REPARTIDOR ------
        // Alta de repartidor con las validaciones correspondientes
        public bool AltaRepartidor(Repartidor r, string user, string password) {
            bool ret = false;

            if (r.EsValido() && ValidarNombreYApellido(r.Nombre, r.Apellido) && ValidarUser(user) && ValidarPassword(password) && !ExisteUser(user)) {
                personas.Add(r);
                usuarios.Add(new Usuario(r, user, password));
                ret = true;
            }

            return ret;
        }

        //------ SERVICIO LOCAL ------
        //Agrega servicios locales controlando que los datos pasados sean correctos

        public bool AltaServicioLocal(Local l) {
            bool ret = false;

            if (l.EsValido()) {
                servicios.Add(l);
                ret = true;
            }

            return ret;
        }

        //------ SERVICIO DELIVERY ------
        //Agrega servicios delivery controlando que los datos pasados sean correctos. 

        public bool AltaServicioDelivery(Delivery d) {
            bool ret = false;

            if (d.EsValido()) {
                servicios.Add(d);
                ret = true;
            }

            return ret;
        }

        //-------------------------- VALIDACIONES --------------------------

        //-------- VALIDAR NOMBRE Y APELLIDO ----------

        // Valida el nombre y appellido pasados revisando que no esten vacíos y ni que contengan números.
        public bool ValidarNombreYApellido(string nombre, string apellido) {
            return !String.IsNullOrWhiteSpace(nombre) && !String.IsNullOrWhiteSpace(apellido) && !TieneNumero(nombre) && !TieneNumero(apellido);
        }

        //-------- VALIDAR EMAIL ----------

        // Valida el user para la clase usuario
        public bool ValidarUser(string user) {
            return !String.IsNullOrWhiteSpace(user);
        }


        // Valida el email ingresado. Debe contener un arroba y que este no se encuentre en la primera ni en la ultima posicion.
        public bool ValidarEmail(string email) {
            int posArroba = email.IndexOf("@");
            return posArroba != email.Length - 1 && posArroba != -1 && posArroba != 0;
        }

        //-------- CONTRASEÑA ----------

        // Valida la password. La password debe tener como mínimo 6 caracteres, incluyendo al menos una mayúscula, una minúscula y un número.
        public bool ValidarPassword(string password) {
            return TieneMayuscula(password) && TieneMinuscula(password) && TieneNumero(password) && password.Length >= 6;
        }

        // Revisa que el texto pasado por parametro contenga un numero
        public bool TieneNumero(string texto) {
            bool tieneNum = false;

            foreach (Char c in texto) if (!tieneNum) {

                    if (Char.IsDigit(c)) {
                        tieneNum = true;
                    }
                }

            return tieneNum;
        }

        // Revisa que el texto pasado por parametro contenga una letra mayuscula
        public bool TieneMayuscula(string texto) {
            bool tieneMay = false;

            foreach (Char c in texto) if (!tieneMay) {

                    if (Char.IsUpper(c)) {
                        tieneMay = true;
                    }
                }

            return tieneMay;
        }

        // Revisa que el texto pasado por parametro contenga una letra minuscula
        public bool TieneMinuscula(string texto) {
            bool tieneMin = false;

            foreach (Char c in texto) if (!tieneMin) {

                if (Char.IsLower(c)) {//is lower confirma si el caracter es minus 
                    tieneMin = true;
                }
            }

            return tieneMin;
        }



        //-------- PRECARGA DE DATOS ----------
        // Realiza la precarga de platos, clientes, mozos, repartidores y usuarios
        public void PrecargaDatos()
        {
            // Precarga Platos

            AltaPlato(new Plato("Tallarines", 240));    /*1*/
            AltaPlato(new Plato("Ñoquis", 270));        /*2*/
            AltaPlato(new Plato("Sorrentinos", 320));   /*3*/
            AltaPlato(new Plato("Raviolones", 300));    /*4*/
            AltaPlato(new Plato("Capeletis", 230));     /*5*/
            AltaPlato(new Plato("Ravioles", 200));      /*6*/
            AltaPlato(new Plato("Focaccia", 160));      /*7*/
            AltaPlato(new Plato("Canelones", 330));     /*8*/
            AltaPlato(new Plato("Lasagna", 290));       /*9*/
            AltaPlato(new Plato("Panera", 130));        /*10*/
            AltaPlato(new Plato("Pizza", 180));         /*11*/
            AltaPlato(new Plato("Mozarella", 200));     /*12*/
            AltaPlato(new Plato("Faina", 120));         /*13*/
            AltaPlato(new Plato("Agua", 80));           /*14*/
            AltaPlato(new Plato("Bebida", 90));         /*15*/
            AltaPlato(new Plato("Cerveza", 150));       /*16*/
            AltaPlato(new Plato("Vino", 200));          /*17*/

            //Precarga Clientes

            AltaCliente(new Cliente("Gianluigi", "Buffon", "gianbuffon@gmail.com"), "gigibuffon", "Gigi01");            /*1*/
            AltaCliente(new Cliente("Paolo", "Maldini", "paolomaldini@gmail.com"), "pmaldini", "Paolo3");               /*2*/
            AltaCliente(new Cliente("Francesco", "Totti", "francescototti@gmail.com"), "ftotti", "Totti10");            /*3*/
            AltaCliente(new Cliente("Fabio", "Cannavaro", "fabiocannavaro@adinet.com.uy"), "fcanna", "Fabio5");         /*4*/
            AltaCliente(new Cliente("Alessandro", "Del Piero", "aledelpiero@gmail.com"), "apiero", "DelPiero7");        /*5*/
            AltaCliente(new Cliente("Mauro", "Camoranesi", "maurocamo@gmail.com"), "mcamoranesi", "MCamo7");            /*6*/
            AltaCliente(new Cliente("Daniele", "de Rossi", "derossi@gmail.com"), "derossi", "DeRossi16");               /*7*/
            AltaCliente(new Cliente("Andrea", "Pirlo", "andreapirlo@gmail.com"), "apirlo", "Pirlo21");                  /*8*/
            AltaCliente(new Cliente("Fabio", "Grosso", "fabiogrosso@hotmail.com"), "fgrosso", "Grosso3");               /*9*/
            AltaCliente(new Cliente("Giorgio", "Chiellini", "giorgio@hotmail.com"), "gchielinni", "Chielli3");          /*10*/

            //Precarga Mozos

            AltaMozo(new Mozo("Mario", "Bros"), "mbros", "maritoB1");               /*1*/
            AltaMozo(new Mozo("Princesa", "Peach"), "ppeach", "princesaP2");        /*2*/
            AltaMozo(new Mozo("Toad", "Kinopio"), "tkinopio", "tKinop3");           /*3*/
            AltaMozo(new Mozo("Coopa", "Troopa"), "ctroopa", "cTroop4");            /*4*/
            AltaMozo(new Mozo("Luigi", "Bros"), "lbros", "Lbros5");                 /*5*/

            //Precarga Repartidores 

            AltaRepartidor(new Repartidor("Eros", "Ramazzotti", Repartidor.Vehiculo.Moto), "erama", "eRama1");              /*1*/
            AltaRepartidor(new Repartidor("Miguel", "Angel", Repartidor.Vehiculo.Bicicleta), "mangel", "Miguel2");          /*2*/
            AltaRepartidor(new Repartidor("Andrea", "Boccelli", Repartidor.Vehiculo.APie), "aboccelli", "aBocce3");         /*3*/
            AltaRepartidor(new Repartidor("Luciano", "Pavarotti", Repartidor.Vehiculo.Bicicleta), "lpavaro", "Lucho4");     /*4*/
            AltaRepartidor(new Repartidor("Leonardo", "Da Vinci", Repartidor.Vehiculo.APie), "ldavinci", "lVinci5");        /*5*/

            //Precarga Servicios

            //TIPO LOCAL    

            Local l1 = new Local(8, GetMozos()[4], 7, GetClientes()[0], new DateTime(2022, 01, 25));
            #region l1
            AltaServicioLocal(l1);
            l1.AgregarPlatoOrden(GetPlatos()[0], 1);
            l1.AgregarPlatoOrden(GetPlatos()[4], 2);
            l1.AgregarPlatoOrden(GetPlatos()[6], 2);
            l1.AgregarPlatoOrden(GetPlatos()[9], 2);
            l1.AgregarPlatoOrden(GetPlatos()[14], 7);
            #endregion

            Local l2 = new Local(4, GetMozos()[4], 3, GetClientes()[1], new DateTime(2022, 04, 05));
            #region l2
            AltaServicioLocal(l2);
            l2.AgregarPlatoOrden(GetPlatos()[7], 1);
            l2.AgregarPlatoOrden(GetPlatos()[6], 1);
            l2.AgregarPlatoOrden(GetPlatos()[5], 1);
            l2.AgregarPlatoOrden(GetPlatos()[14], 1);
            l2.AgregarPlatoOrden(GetPlatos()[13], 2);
            #endregion
            
            Local l3 = new Local(3, GetMozos()[0], 3, GetClientes()[2], new DateTime(2022, 02, 02));
            #region l3
            AltaServicioLocal(l3);
            l3.AgregarPlatoOrden(GetPlatos()[1], 2);
            l3.AgregarPlatoOrden(GetPlatos()[3], 2);
            #endregion
            
            Local l4 = new Local(1, GetMozos()[3], 5, GetClientes()[3], new DateTime(2021, 03, 12));
            #region l4
            AltaServicioLocal(l4);
            l4.AgregarPlatoOrden(GetPlatos()[9], 1);
            l4.AgregarPlatoOrden(GetPlatos()[8], 1);
            l4.AgregarPlatoOrden(GetPlatos()[4], 1);
            l4.AgregarPlatoOrden(GetPlatos()[3], 1);
            l4.AgregarPlatoOrden(GetPlatos()[1], 1);
            #endregion
            
            Local l5 = new Local(5, GetMozos()[2], 2, GetClientes()[4], new DateTime(2022, 01, 14));
            #region l5
            AltaServicioLocal(l5);
            l5.AgregarPlatoOrden(GetPlatos()[1], 2);
            #endregion
            
            Local l6 = new Local(2, GetMozos()[1], 3, GetClientes()[0], new DateTime(2022, 03, 05));
            #region l6
            AltaServicioLocal(l6);
            l6.AgregarPlatoOrden(GetPlatos()[1], 1);
            l6.AgregarPlatoOrden(GetPlatos()[4], 1);
            l6.AgregarPlatoOrden(GetPlatos()[8], 1);
            l6.AgregarPlatoOrden(GetPlatos()[6], 1);
            #endregion
            
            Local l7 = new Local(1, GetMozos()[2], 2, GetClientes()[0], new DateTime(2021, 07, 03));
            #region l7
            AltaServicioLocal(l7);
            l7.AgregarPlatoOrden(GetPlatos()[10], 1);
            l7.AgregarPlatoOrden(GetPlatos()[11], 1);
            l7.AgregarPlatoOrden(GetPlatos()[12], 2);
            #endregion
            
            Local l8 = new Local(4, GetMozos()[3], 3, GetClientes()[1], new DateTime(2020, 11, 18));
            #region l8
            AltaServicioLocal(l8);
            l8.AgregarPlatoOrden(GetPlatos()[0], 1);
            l8.AgregarPlatoOrden(GetPlatos()[8], 2);
            l8.AgregarPlatoOrden(GetPlatos()[14], 3);
            #endregion
            
            Local l9 = new Local(7, GetMozos()[2], 1, GetClientes()[1], new DateTime(2022, 04, 5));
            #region l9
            AltaServicioLocal(l9);
            l9.AgregarPlatoOrden(GetPlatos()[7], 1);
            l9.AgregarPlatoOrden(GetPlatos()[13], 1);
            #endregion


            //TIPO DELIVERY
            Delivery d1 = new Delivery(GetClientes()[1], new DateTime(2022, 4, 22), "Mercedez y Cuareim 123", GetRepartidores()[1], 1400);
            #region d1
            AltaServicioDelivery(d1);
            d1.AgregarPlatoOrden(GetPlatos()[1], 2);
            d1.AgregarPlatoOrden(GetPlatos()[2], 1);
            d1.AgregarPlatoOrden(GetPlatos()[3], 1);
            d1.AgregarPlatoOrden(GetPlatos()[12], 4);
            #endregion

            Delivery d2 = new Delivery(GetClientes()[2], new DateTime(2022, 3, 5), "Priamo y Rivera 1575", GetRepartidores()[4], 2500);
            #region d2
            AltaServicioDelivery(d2);
            d2.AgregarPlatoOrden(GetPlatos()[2], 3);
            d2.AgregarPlatoOrden(GetPlatos()[4], 2);
            d2.AgregarPlatoOrden(GetPlatos()[5], 4);
            #endregion

            Delivery d3 = new Delivery(GetClientes()[3], new DateTime(2022, 2, 28), "18 de Julio y Ejido 456", GetRepartidores()[4], 200);
            #region d3
            AltaServicioDelivery(d3);
            d3.AgregarPlatoOrden(GetPlatos()[1], 1);
            d3.AgregarPlatoOrden(GetPlatos()[2], 1);
            d3.AgregarPlatoOrden(GetPlatos()[3], 1);
            d3.AgregarPlatoOrden(GetPlatos()[4], 1);
            d3.AgregarPlatoOrden(GetPlatos()[5], 1);
            #endregion

            Delivery d4 = new Delivery(GetClientes()[4], new DateTime(2021, 4, 27), "Bv Artigas y Av Italia 789", GetRepartidores()[2], 4100);
            #region d4
            AltaServicioDelivery(d4);
            d4.AgregarPlatoOrden(GetPlatos()[1], 2);
            d4.AgregarPlatoOrden(GetPlatos()[2], 1);
            d4.AgregarPlatoOrden(GetPlatos()[3], 1);
            #endregion

            Delivery d5 = new Delivery(GetClientes()[5], new DateTime(2022, 1, 31), "Justicia y Amezaga 159", GetRepartidores()[3], 1200);
            #region d5
            AltaServicioDelivery(d5);
            d5.AgregarPlatoOrden(GetPlatos()[1], 4);
            d5.AgregarPlatoOrden(GetPlatos()[2], 3);
            d5.AgregarPlatoOrden(GetPlatos()[3], 2);
            d5.AgregarPlatoOrden(GetPlatos()[4], 5);
            #endregion

            #region Cierre de servicios
            l1.CerrarServicio();
            l2.CerrarServicio();
            l3.CerrarServicio();
            l4.CerrarServicio();
            l5.CerrarServicio();
            l6.CerrarServicio();
            l7.CerrarServicio();
            l8.CerrarServicio();
            l9.CerrarServicio();
            d1.CerrarServicio();
            d2.CerrarServicio();
            d3.CerrarServicio();
            d4.CerrarServicio();
            d5.CerrarServicio();
            #endregion
            
        }

        // MVC
        public Persona LogIn(string user, string pass) {
            Persona pLog = null;

            foreach(Usuario u in usuarios) {

                if (u.User.Equals(user) && u.Password.Equals(pass)) {
                    pLog = u.Persona;
                }
            }

            return pLog;
        }

        // Devuelve los platos ordenados por nombre de forma ascendente.
        public List<Plato> GetPlatosPorNombre() {
            List<Plato> lp = GetPlatos();
            lp.Sort();
            return lp;
        }


        // Devuelve la persona con Id pasado por parametro
        public Cliente GetClienteXId(int id) {
            Cliente cliBuscado = null;

            foreach (Cliente c in GetClientes()) {
                
                if (c.Id.Equals(id)) {
                    cliBuscado = c;
                }
            }

            return cliBuscado;
        }

        // Dar like a un plato seleccionado desde una lista que pasa por binding el Id
        public void Likear(int idPlato)
        {
            Plato p = GetPlatoXId(idPlato);
            p.Likes++;
        }

        // Retorna un plato pasandole el id del plato buscado
        public Plato GetPlatoXId(int idPlato) {
            Plato buscado = null;

            foreach(Plato p in platos) {

                if (p.Id.Equals(idPlato)) {
                    buscado = p;
                }
            }

            return buscado;
        }

        // Retorna los servicios de un cliente dado entre fechas marcadas
        public List<Servicio> GetServiciosClienteEntreFechas(int idPersona, DateTime f1, DateTime f2) {
            List<Servicio> ret = new List<Servicio>();

            foreach(Servicio s in servicios) {
                if (s.Cliente.Id.Equals(idPersona)) {
                    if (s.Fecha >= f1 && s.Fecha <= f2) {
                        if (s.Estado == "Cerrado") {
                            ret.Add(s);
                        }
                    }
                }
            }

            return ret;
        }

        // Retorna el servicio pasandole el Id del servicio
        public Servicio GetServicioXId(int idServicio) {
            Servicio buscado = null;

            foreach (Servicio s in servicios) {
                if (s.Id.Equals(idServicio)) {
                    buscado = s;
                }
            }

            return buscado;
        }

        // Retorna los servicios mas caros de la persona pasada por parametro
        public List<Servicio> GetServiciosMasCaros(int idPers)
        {
            List<Servicio> ret = new List<Servicio>();
            double maxMontoServicios = MontoMaximoDeServicios(idPers);

            foreach(Servicio s in servicios)
            {
                if (s.Cliente.Id.Equals(idPers) && s.Estado == "Cerrado")
                {
                    if (s.PrecioFinal == maxMontoServicios)
                    {
                        ret.Add(s);
                    }
                }
            }

            return ret;
        }

        // Retorna el monto mas caro de un servicio de una persona cuyo id de persona es pasado por parametro
        public double MontoMaximoDeServicios(int idPers) {
            double max = 0;

            foreach(Servicio s in servicios) {
                
                if(s.Cliente.Id.Equals(idPers) && s.Estado == "Cerrado") {    
                    if (s.PrecioFinal > max) {
                        max = s.PrecioFinal;
                    }
                }
            }

            return max;
        }


        // Retorna el plato cuyo nombre es pasado por parametro.
        public Plato GetPlatoXNombre(string nombrePlato) {
            Plato pBuscado = null;

            foreach(Plato p in platos) {
                
                if (p.Nombre.ToLower().Equals(nombrePlato.ToLower())) {
                    pBuscado = p;
                }
            }

            return pBuscado;
        }


        // Retorna los servicios del cliente que contienen el plato cuyo nombre es ingresado por parametro
        public List<Servicio> ServiciosContienenPlato(Plato p, int idPers) {
            List<Servicio> ret = new List<Servicio>();

            foreach(Servicio s in servicios) {
                
                if (s.Cliente.Id.Equals(idPers)) {
                
                    if (OrdenContienePlato(s.Orden, p)) {
                        ret.Add(s);
                    }
                }
            }

            return ret;
        }

        // Retorna un booleano que nos determina si una orden contiene el plato que son pasados por parametros
        public bool OrdenContienePlato(List<CantidadPlato> orden, Plato p) {
            bool contiene = false;

            foreach(CantidadPlato cp in orden) {
                
                if (cp.Plato.Equals(p)) {
                    contiene = true;
                }
            }

            return contiene;
        }

        // Retorna una lista de todos los servicios de un repartidor cuyo id de persona es pasado por parametro
        public List<Servicio> GetServiciosDeRepartidor(int idRep) {
            List<Servicio> ret = new List<Servicio>();

            foreach(Servicio s in servicios) {
                
                if (s.GetType().Name.Equals("Delivery")) {
                    Delivery d = s as Delivery;
                    if (d.Repartidor.Id.Equals(idRep)) {
                        ret.Add(s);
                    }
                }
            }
            ret.Sort();
            return ret;
        }


        // Retorna los servicios en los que trabajo un mozo dado entre fechas que son pasadas por parametro.
        public List<Local> GetServiciosMozoEntreFechas(int idPers, DateTime f1, DateTime f2) {
            List<Local> ret = new List<Local>();

            foreach (Servicio s in servicios) {
                
                if (s.GetType().Name.Equals("Local")) {
                    Local l = s as Local;
                    if (l.Mozo.Id.Equals(idPers) && s.Fecha >= f1 && s.Fecha <= f2) {
                        if (s.Estado == "Cerrado") {
                            ret.Add(l);
                        }
                    }
                }
            }

            return ret;
        }


        // Devuelve un mozo random de los mozos ingresados al sistema
        public Mozo MozoRandom() {
            Random rnd = new Random();
            List<Mozo> listaMozos = GetMozos();
            int indMozo = rnd.Next(0, listaMozos.Count - 1);
            return listaMozos[indMozo];
        }


        // Devuelve un repartidor random de los repartidores ingresados al sistema
        public Repartidor RepartidorRandom() {
            Random rnd = new Random();
            List<Repartidor> listaRepartidores = GetRepartidores();
            int indRepartidor = rnd.Next(0, listaRepartidores.Count - 1);
            return listaRepartidores[indRepartidor];
        }


        // Retorna los servicios de un cliente
        public List<Servicio> GetServiciosDeCliente(int idPersona) {
            List<Servicio> ret = new List<Servicio>();

            foreach (Servicio s in servicios) {
                if (s.Cliente.Id.Equals(idPersona)) {
                    if (s.Estado.Equals("Abierto")) {
                        ret.Add(s);
                    }
                }
            }

            foreach (Servicio s in servicios) {
                if (s.Cliente.Id.Equals(idPersona)) {
                    if (s.Estado.Equals("Cerrado")) {
                        ret.Add(s);
                    }
                }
            }

            return ret;
        }


        // Retorna si un servicio esta asociado a la persona ya sea si lo solicito siendo cliente o si lo entrego siendo mozo o repartidor
        public bool ServicioEsDePersona(int idServicio, int idPersona) {
            bool ret = false;

            foreach (Servicio s in servicios) if (!ret)
            {
                if (s.Id.Equals(idServicio)) {
                    if (s.Cliente.Id.Equals(idPersona) || 
                       (s is Local l && l.Mozo.Id.Equals(idPersona)) || 
                       (s is Delivery d && d.Repartidor.Id.Equals(idPersona)))
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

    }
}
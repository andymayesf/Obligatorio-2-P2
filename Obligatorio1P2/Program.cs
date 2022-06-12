using System;
using System.Collections.Generic;
using Clases;

namespace Obligatorio1P2
{
    class Program
    {
        static void Main(string[] args)
        {
            Restaurante r = new Restaurante();

            int op = -1;

            while (op != 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                MostrarMenu();

                op = Int32.Parse(Console.ReadLine());

                switch (op)
                {

                    //1.Listar todos los platos.
                    case 1:
                        {
                            Console.Clear();
                            Console.WriteLine("~~~~~~~ LISTADO DE TODOS LOS PLATOS ~~~~~~~\n");

                            if (r.GetPlatos().Count > 0) {
                                foreach (Plato p in r.GetPlatos()) {
                                    Console.WriteLine(p);
                                }
                            } else {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("NO HAY PLATOS INGRESADOS AL SISTEMA");
                            }
                            break;
                        }

                    //2.Listado de clientes ordenado por apellido / nombre.
                    case 2:
                        {
                            Console.Clear();
                            Console.WriteLine("~~~~~~~~~~~ LISTADO DE CLIENTES ORDENADO POR APELLIDO / NOMBRE ~~~~~~~~~~~\n");
                            if (r.GetClientesPorApellido().Count > 0)
                            {
                                foreach (Cliente c in r.GetClientesPorApellido())
                                {
                                    Console.WriteLine(c);
                                }
                            }
                            else
                            {
                                Console.WriteLine("LA LISTA DE CLIENTES ESTA VACIA");
                            }
                            break;
                        }

                    //3.Listado de los servicios entregados por un repartidor en un rango de fechas dado.
                    case 3:
                        {
                            Console.Clear();
                            Console.WriteLine("~~~~~ LISTADO DE LOS SERVICIOS ENTREGADOS POR UN REPARTIDOR EN UN RANGO DE FECHAS DADO ~~~~~");
                            int i = 1;

                            //Lista de repartidores para mostrar en consola
                            foreach (Repartidor rep in r.GetRepartidores())
                            {
                                Console.WriteLine($"{i} - {rep}");
                                i++;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Ingrese el numero del repartidor");

                            Repartidor repElegido = null;
                            //Le pedimos al usuario un entero que luego lo convertiremos al repartidor correspondiente
                            int repartidorInt = Int32.Parse(Console.ReadLine());

                            if (repartidorInt > 0 && repartidorInt <= r.GetRepartidores().Count)
                            {
                                repElegido = r.GetRepartidores()[repartidorInt - 1];
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Beep();
                                Console.WriteLine("\nEL NUMERO INGRESADO NO CORRESPONDE A NINGUN REPARTIDOR");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\nPARA VISUALIZAR LA LISTA QUE USTED DESEE PRESIONE CUALQUIER TECLA Y VUELVA A INGRESAR A LA OPCION 3 EN EL MENU PRINCIPAL\n");
                                break;
                            }

                            Console.WriteLine("\nIngrese fecha inicial");
                            Console.WriteLine("(AAAA-MM-DD)");
                            DateTime fechaIni = DateTime.Parse(Console.ReadLine());

                            if (fechaIni > DateTime.Now || fechaIni < new DateTime(1980, 1, 1))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Beep();
                                Console.WriteLine("\nLA FECHA INGRESADA ES INCORRECTA.");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("PARA VISUALIZAR LA LISTA QUE USTED DESEE PRESIONE CUALQUIER TECLA Y VUELVA A INGRESAR A LA OPCION 3 EN EL MENU PRINCIPAL\n");
                                break;
                            }

                            Console.WriteLine("\nIngrese fecha final");
                            Console.WriteLine("(AAAA-MM-DD)");
                            DateTime fechaFin = DateTime.Parse(Console.ReadLine());

                            if (fechaFin > DateTime.Now || fechaFin < new DateTime(1980, 1, 1))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Beep();
                                Console.WriteLine("\nLA FECHA INGRESADA ES INCORRECTA.");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("PARA VISUALIZAR LA LISTA QUE USTED DESEE PRESIONE CUALQUIER TECLA Y VUELVA A INGRESAR A LA OPCION 3 EN EL MENU PRINCIPAL\n");
                                break;
                            }
                            //Chequea que existan deliverys que correspondan con los parametros establecidos y los presenta 
                            if (r.GetOrdenDeliveryPorFechasYRepartidor(fechaIni, fechaFin, repElegido).Count > 0)
                            {
                                int iEnvio = 1;
                                Console.WriteLine($"\n~~~~~ ENVIOS DE {repElegido.Nombre} {repElegido.Apellido} ENTRE {fechaIni} Y {fechaFin} ~~~~~\n");
                                foreach (Delivery d in r.GetOrdenDeliveryPorFechasYRepartidor(fechaIni, fechaFin, repElegido))
                                {
                                    Console.WriteLine($"{iEnvio} - {d}");
                                    iEnvio++;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"\nNO HAY ENVIOS EN LAS FECHAS INGRESADAS POR {repElegido.Nombre} {repElegido.Apellido}");
                            }

                            break;
                        }

                    //4.Modificar el valor del precio mínimo del plato.
                    case 4:
                        {
                            Console.Clear();
                            Console.WriteLine("~~~~ MODIFICADO DEL PRECIO MINIMO DEL PLATO ~~~~\n");
                            Console.WriteLine("Ingrese el precio minimo nuevo de plato");


                            double precioNuevo = 0;

                            precioNuevo = double.Parse(Console.ReadLine());

                            if (precioNuevo > 0)
                            {
                                Plato.PrecioMinimo = precioNuevo;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nEL PRECIO MINIMO DE LOS PLATOS SE HA MODIFICADO CON ÉXITO.");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Beep();
                                Console.WriteLine("\nHA INGRESADO UN PRECIO INVALIDO. RECUERDE QUE EL PRECIO DEBE SER MAYOR A 0");
                            }

                            break;
                        }

                    //5.Alta de Mozo.
                    case 5:
                        {
                            Console.Clear();
                            Console.WriteLine("~~~~ ALTA MOZO ~~~~\n");
                            
                            Console.WriteLine("Nombre: ");
                            string nombre = Console.ReadLine();
                            
                            Console.WriteLine("\nApellido: ");
                            string apellido = Console.ReadLine();
                            
                            Console.WriteLine("\nUsuario: ");
                            string user = Console.ReadLine();
                            
                            Console.WriteLine("\nContraseña: ");
                            string password = Console.ReadLine();

                            //Alta mozo controla que los datos ingresados sean correctos. Si son incorrectos devuelve null.

                            Console.WriteLine();

                            if (r.AltaMozo(new Mozo(nombre, apellido), user, password))
                            {
                                Console.WriteLine("EL MOZO SE HA DADO DE ALTA CON ÉXITO");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Beep();
                                Console.WriteLine("LOS DATOS INGRESADOS NO FUERON CORRECTOS. NI EL NOMBRE NI EL APELLIDO PUEDEN CONTENER NÚMEROS.");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("\nPRESIONE CUALQUIER TECLA Y VUELVA A PRESIONAR 5 EN EL MENU PRINCIPAL PARA INGRESAR UN NUEVO MOZO");
                            }

                            break;
                        }

                    case 0:
                        break;

                    default:

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Beep();
                        Console.WriteLine("EL NUMERO INGRESADO NO ESTABA ENTRE LAS OPCIONES");
                        break;

                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("PRESIONE CUALQUIER TECLA");

                Console.ReadKey();
            }
        }

        public static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("~~~~~~~~ SISTEMA DEL RESTAURANTE ~~~~~~~~");
            Console.WriteLine("ELIJA UNA DE LAS OPCIONES Y PRESIONE ENTER");
            Console.WriteLine("0 - Salir");
            Console.WriteLine("1 - Lista todos los platos");
            Console.WriteLine("2 - Listar clientes por apellido/nombre");
            Console.WriteLine("3 - Listar los repartos de un repartidor por fecha");
            Console.WriteLine("4 - Cambiar precio de plato");
            Console.WriteLine("5 - Alta de mozo");
            Console.WriteLine();

        }

    }

}



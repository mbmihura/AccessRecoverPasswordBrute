using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace AccessRecoverPasswordBrute
{
    class Program
    {
        static string _path;
        static OleDbConnection _con = new OleDbConnection();
        static List<string> simbolos = new List<string>();
        static string _password;

        static void Main(string[] args)
        {
            int largo;
            string unCaracter;

            //Cargar datos
            Console.Write("Path de la db. Presione Entre para continuar: ");
            _path = Console.ReadLine();
            //Cargar datos
            Console.Write("Largo contraseña. Presione Enter para continuar: ");
            largo = Convert.ToInt16(Console.ReadLine());
            //determinar chars
            Console.WriteLine("\nPropando caracteres posibles: ");
            for (int i = 32; i < 255; i++)
            {
                unCaracter = Convert.ToString(Convert.ToChar(i));
                try
                {
                    Console.Write("Probando caracter: " + unCaracter);
                    _con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + _path + ";Jet OLEDB:Database Password=" + unCaracter  + ";";
                    _con.Open();
                    Console.WriteLine("  DESCONOCIDO: Probable contraseña.");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "No es una contraseña válida.")
                    {
                        simbolos.Add(unCaracter);
                        Console.WriteLine("  Aceptado");
                    }
                    else
                    {
                        Console.WriteLine("  Rechazado" );
                    }       
                }
            }
            //TODO: ver cuanto dura
            double total = Math.Pow(simbolos.Count, largo);
            Console.WriteLine(total + " posibilidades.\n");

            if (largo > 1000000)
                Console.WriteLine("Have a coffe!\n");
            
            // INTENTO _____________________________________________________________________________________________________________________________
            Console.WriteLine("Comenzando...\n");
            for (int k = 0; k < simbolos.Count; k++)
            {
                Console.WriteLine("INTENTOS QUE COMIENZAN CON: " + simbolos.Count);
                intentar(simbolos[k], largo);
            }
            //termina no se encuentra
            Console.Write("No encontrada, presione alguna tecla para terminar.");
            Console.ReadLine();
        }
        static private void intentar(string pass, int cantidad)
        {
            Console.Write("Intentando " + pass);
            try
            {
                _con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + _path + ";Jet OLEDB:Database Password=" + pass + ";";
                _con.Open();
                Console.WriteLine("Conexión Abierta");
                _password = pass;
            }
            catch (Exception)
            {
                Console.WriteLine("  Falla");
                cantidad--;
                if (cantidad > 0)
                {
                    for (int i = 0; i < simbolos.Count; i++)
                    {
                        intentar(pass + simbolos[i], cantidad);
                    }
                }
            }           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace AccessRecoverPasswordBrute
{
    class FuerzaBruta
    {
        List<string> simbolos = new List<string>();
        double actual = 0;
        double total;
        IVictima _victima;

        public FuerzaBruta(IVictima victima)
        {
            string alfabeto;
            int largo;
            string unCaracter;

            //Cargar y configurar datos.
            _victima = victima;

            Console.Write("Escriba el largo maximo de la contraseña (Presione Enter para continuar): ");
            largo = Convert.ToInt16(Console.ReadLine());

            Console.Write("Escriba las letra con las que desea intentar (si deja vacio, se usan todas. Presione Enter para continuar):\n");
            alfabeto = Console.ReadLine();
            if (alfabeto == "")
            {
                //Intentar usar uno por uno cada simbolo, dependiendo el tipo de error los guarda para utilizar o no.
                Console.WriteLine("\nProbando caracteres:");
                for (int i = 32; i < 255; i++)
                {
                    unCaracter = Convert.ToString(Convert.ToChar(i));
                    Console.Write("Probando caracter: " + unCaracter + " (Indice " + i + "). Espere...         ");
                    try
                    {
                        //Intenta usarlos
                        victima.Probar(unCaracter);
                        simbolos.Add(unCaracter);
                        Console.WriteLine("DESCONOCIDO: Probable contraseña.");
                    }
                    catch (Exception ex)
                    {
                        //Si lo reconoce como una contraseña, guardar para usar
                        if (ex.Message == "No es una contraseña válida.")
                        {
                            simbolos.Add(unCaracter);
                            Console.WriteLine("Aceptado");
                        }
                        else
                            Console.WriteLine("Rechazado: " + ex.Message);
                    }
                }
            }
            else
            {
                //Usa un conjunto determinado por el usario (eliminando los repetidos)
                string sletra;
                foreach (char letra in alfabeto)
                {
                    sletra = Convert.ToString(letra);
                    if (!simbolos.Contains(sletra))
                        simbolos.Add(sletra);
                }
            }
            //Mostrar config. a utilizar.
            StringBuilder simbolosCadena = new StringBuilder ();
            foreach (string letra in simbolos )
            {
                simbolosCadena.Append(letra + " ");
            }
            Console.Clear();
            Console.Write("Recuperando contraseña de " + victima.Nombre + " con longitud maxima de "+largo +" simbolos y alfabeto: " + simbolosCadena + "\n\n   Calculando...    ");
            total = Math.Pow(simbolos.Count, largo);

            //Calcular cantidad de posibilidades
            Console.WriteLine(total + " posibilidades.\n");

            if (largo > 1000000)
                Console.WriteLine("Have a coffe!\n");

            Console.Write("Presione Enter para comenzar.");
            do { }
            while ("" != Console.ReadLine());

            Console.WriteLine("Comenzando...\n");

            //Comenzar intentos
            try
            {
                for (int k = 0; k < simbolos.Count; k++)
                {
                    Console.WriteLine("      ---   {0:000} / {1:000} -- INTENTOS QUE COMIENZAN CON: " + simbolos[k], k+1, simbolos.Count);
                    intentar(simbolos[k], largo);
                }
                Console.Write("\n   No encontrada, presione tecla 'q' para terminar.\n\n");
            }
            //Si se encuentra la contraseña, captura el err. levantado.
            catch (EncontradaException ex)
            {
                Console.Write("\n       Password: " + ex.Password   + "\n\nPresione 'q' para terminar.\n");
            }
            //para terminar...
            do
            {
                Console.Write(":");
            } while (Console.ReadLine() != "q");
        }
        private void intentar(string pass, int cantidad)
        {
            //Intentar con el caracter pasado por parametro.
            Console.Write("   Intentando {0:000000000000000} / {1:000000000000000}    {2}    Esperando...          ", ++actual, total, pass);
            try
            {
                _victima.Probar(pass);
            }
            catch (Exception ex)
            {
                //Si falla, muestra el mensaje y sigue intentado con strings que comienzen con el string probado
                Console.WriteLine(ex.Message);
                cantidad--;
                //Continuar mientras contraseña no exceda el largo.
                if (cantidad > 0)
                {
                    for (int i = 0; i < simbolos.Count; i++)
                    {
                        intentar(pass + simbolos[i], cantidad);
                    }
                }
                return;
            }
            //Si no falla, arrojar excepcion de encontrada.
            Console.WriteLine("Conexión Abierta");
            throw new EncontradaException(pass);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessRecoverPasswordBrute
{
    public interface  IVictima
    {
        //Intenta acceder con una determinada contraseña.
         void Probar(string password);
        //Devuelve texto para informar al usuario (no es esencial, informativo)
         string Nombre { get; }
    }
}

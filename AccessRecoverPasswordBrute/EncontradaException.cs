using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessRecoverPasswordBrute
{
    class EncontradaException:ApplicationException 
    {
        string _pass;

        public EncontradaException(string mensaje) 
        {
            _pass = mensaje;
        }
        public string Password { get { return _pass; } }
    }
}

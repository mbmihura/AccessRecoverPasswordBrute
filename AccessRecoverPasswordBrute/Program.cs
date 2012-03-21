using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace AccessRecoverPasswordBrute
{
    class Program
    {      
        static void Main(string[] args)
        {
            new FuerzaBruta(new db());
        }

        class db : IVictima
        {
            OleDbConnection _con = new OleDbConnection ();

            public void Probar(string password)
            {
                _con.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= *****************************************.mdb;Jet OLEDB:Database Password=" + password + ";";
                _con.Open();
            }
            public string Nombre
            {
                get { return "Base"; }
            }
        }
    }
}

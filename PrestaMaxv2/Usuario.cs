using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaMaxv2
{
    public class Usuario
    {
        string _login;
        string _nombre;
        string _pass;
        char[] _tipo = { 'A', 'U' };
        int _t;

        //constructores
        public Usuario()
        {
            _login = "";
            _nombre = "";
            _t = 1;
            _pass = "";
        }

        public Usuario(string log, string nom, int tip, string pa)
        {
            _login = log;
            _nombre = nom;
            _t = tip;
            _pass = pa;
        }

        //Propiedades
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Password
        {
            get { return _pass; }
            set { _pass = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public int NumTipo
        {
            get { return _t; }
            set { _t = value; }
        }

        public char[] CharTipo
        {
            get { return _tipo; }
        }
    }
}

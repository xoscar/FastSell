using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Usuario
    {
        private string _login;
        private string _nombre;
        private string _pass;
        private char[] _tipo = { 'A', 'U' };
        private bool _bloqueado;
        private int _t;

        //constructores
        public Usuario()
        {
            _login = "";
            _nombre = "";
            _t = 1;
            _pass = "";
            _bloqueado = false;
        }

        public Usuario(string log, string nom, int tip, string pa, bool bloq )
        {
            _login = log;
            _nombre = nom;
            _t = tip;
            _pass = pa;
            _bloqueado = bloq;
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

        public bool Bloqueado
        {
            get { return _bloqueado; }
            set { _bloqueado = value; }
        }
    }
}

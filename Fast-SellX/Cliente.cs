using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Fast_SellX
{
    public class Cliente
    {
        private string _cliId;
        private string _nombre;
        private string _apellido;
        private string _direccion;
        private DateTime _fechaAlta;
        private string _tel;
        private string _usrId;
        private string _negocio;
        private ArrayList _precios;
        //Constructores
        public Cliente()
        {
            _cliId = "";
            _nombre = "";
            _apellido = "";
            _direccion = "";
            _fechaAlta = new DateTime();
            _tel = "";
            _usrId = "";
            _negocio = "";
        }

        public Cliente(string cli, string nom, string ape, string dire, DateTime fa, string tel, string usr, ArrayList pre)
        {
            _cliId = cli;
            _nombre = nom;
            _direccion = dire;
            _apellido = ape;
            _fechaAlta = fa;
            _tel = tel;
            _usrId = usr;
            _precios = pre;
        }

        //Propiedades

        public string Id_Cliente
        {
            get { return _cliId; }
            set { _cliId = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }

        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }

        public DateTime Fecha_Alta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public string Telefono
        {
            get { return _tel; }
            set { _tel = value; }
        }

        public string Negocio
        {
            get { return _negocio; }
            set { _negocio = value; }
        }

        public string Id_Usuario
        {
            get { return _usrId; }
            set { _usrId = value; }
        }

        public ArrayList Precios
        {
            get { return _precios; }
            set { _precios = value; }
        }
    }
}

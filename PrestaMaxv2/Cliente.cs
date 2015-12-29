using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;

namespace PrestaMaxv2
{
    public class Cliente
    {
        string _cliId;
        string _nombre;
        string _apellido;
        string _direccion;
        DateTime _fechaAlta;
        string _tel;
        string _usrId;

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
        }

        public Cliente(string cli, string nom, string ape, string dire, DateTime fa, string tel, string usr)
        {
            _cliId = cli;
            _nombre = nom;
            _direccion = dire;
            _apellido = ape;
            _fechaAlta = fa;
            _tel = tel;
            _usrId = usr;
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

        public string Id_Usuario
        {
            get { return _usrId; }
            set { _usrId = value; }
        }
    }
}

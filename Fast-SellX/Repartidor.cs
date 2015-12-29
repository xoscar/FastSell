using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Repartidor
    {
        private string _reparId;
        private string _nombre;
        private string _apellido;
        private string _direccion;
        private string _telefono;
        private DateTime _fechaAlta;
        private string _usrId;

        public Repartidor()
        {
            _reparId = "";
            _nombre = "";
            _apellido = "";
            _direccion = "";
            _telefono = "";
            _fechaAlta = new DateTime();
            _usrId = "";
        }

        public Repartidor(string rep, string nom, string ape, string dir, string tel, DateTime fa, string usr)
        {
            _reparId = rep;
            _nombre = nom;
            _apellido = ape;
            _direccion = dir;
            _telefono = tel;
            _fechaAlta = fa;
            _usrId = usr;
        }

        public string Id_Repartidor
        {
            get { return _reparId; }
            set { _reparId = value; }
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

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public DateTime Fecha_Alta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public string Id_User
        {
            get { return _usrId; }
            set { _usrId = value; }
        }
    }
}

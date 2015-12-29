using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Proveedor
    {
        string _nombre;
        string _apellido;
        string _telefono;
        string _negocio;
        int _proveedorID;
        string _direccion;

        public Proveedor()
        {
            _nombre = "";
            _apellido = "";
            _telefono = "";
            _negocio = "";
            _proveedorID = 0;
            _direccion = "";
        }


        public int Id_Proveedor
        {
            get { return _proveedorID; }
            set { _proveedorID = value; }
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

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public string Negocio
        {
            get { return _negocio; }
            set { _negocio = value; }
        }

        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }
    }
}

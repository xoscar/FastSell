using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Producto
    {
        private int _proId;
        private string _nombre;
        private double _precioGeneral;
        private int _cantidad;
        private int _tipo;
        private string[] _acums = {"Paquete", "Kilo","Bolsa","Unidad","Caja","Rollo"};
        private DateTime _fechaIngreso;
        private string _descripcion;
        private string _usrId;

        public Producto()
        {
            _proId = 0;
            _nombre = "";
            _precioGeneral = 0.0;
            _cantidad = 0;
            _tipo = 0;
            _fechaIngreso = new DateTime();
            _descripcion = "";
            _usrId = "";
        }

        public Producto(int pro, string nom, double pec, int cant, int ti, DateTime fi, string desc, string usr)
        {
            _proId = pro;
            _nombre = nom;
            _precioGeneral = pec;
            _cantidad = cant;
            _tipo = ti;
            _fechaIngreso = fi;
            _descripcion = desc;
            _usrId = usr;
        }

        public int Id_Producto
        {
            get { return _proId; }
            set { _proId = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public double Precio_General
        {
            get { return _precioGeneral; }
            set { _precioGeneral = value; }
        }
        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }
        public int Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        public DateTime Fecha_Ingreso
        {
            get { return _fechaIngreso; }
            set { _fechaIngreso = value; }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
        public string Id_User
        {
            get { return _usrId; }
            set { _usrId = value; }
        }

        public string[] Acumulacion
        {
            get { return _acums; }
        }

        //Sobrecarga de operadores
        public static bool operator >(Producto a,Producto b)
        {
            if (a.Id_Producto > b.Id_Producto)
                return true;
            else
                return false;
        }

        public static bool operator <(Producto a, Producto b)
        {
            if (a.Id_Producto < b.Id_Producto)
                return true;
            else
                return false;
        }

        public static bool operator >=(Producto a, Producto b)
        {
            if (a.Id_Producto >= b.Id_Producto)
                return true;
            else
                return false;
        }

        public static bool operator <=(Producto a, Producto b)
        {
            if (a.Id_Producto <= b.Id_Producto)
                return true;
            else
                return false;
        }

        public static bool operator ==(Producto a, Producto b)
        {
            if (a.Id_Producto == b.Id_Producto)
                return true;
            else
                return false;
        }

        public static bool operator !=(Producto a, Producto b)
        {
            if (a.Id_Producto != b.Id_Producto)
                return true;
            else
                return false;
        }
    }
}

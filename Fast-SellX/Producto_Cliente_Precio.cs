using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Producto_Cliente_Precio
    {
        private int _preId;
        private double _precio;
        private int _prodId;
        private string _nombre;
        private string _cliId;

        public Producto_Cliente_Precio()
        {
            _preId = 0;
            _precio = 0.0;
            _prodId = 0;
            _nombre = "";
            _cliId = "";
        }

        public Producto_Cliente_Precio(int pre, double prec, int pro, string nom, string cli)
        {
            _preId = pre;
            _precio = prec;
            _prodId = pro;
            _nombre = nom;
            _cliId = cli;
        }

        public int ID_PCP
        {
            get { return _preId; }
            set { _preId = value; }
        }

        public double Precio
        {
            get { return _precio; }
            set { _precio  = value; }
        }
        public int Id_Producto
        {
            get { return _prodId; }
            set { _prodId = value; }
        }
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public string Id_Cliente
        {
            get { return _cliId; }
            set { _cliId = value; }
        }
    }
}

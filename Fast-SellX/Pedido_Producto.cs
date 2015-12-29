using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Pedido_Producto
    {
        private double _cantidad;
        private int _prodid;
        private string _nombreProd;
        private int _pedidoId;

        public Pedido_Producto()
        {
            _cantidad = 0.0f;
            _pedidoId = 0;
            _nombreProd = "";
            _pedidoId = 0;
        }

        public Pedido_Producto(float can, int pro, string nom, int ped)
        {
            _cantidad = can;
            _pedidoId = ped;
            _nombreProd = nom;
            _prodid = pro;
        }

        public double Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public int Id_Pedido
        {
            get { return _pedidoId; }
            set { _pedidoId = value; }
        }

        public string Nombre_Producto
        {
            get { return _nombreProd; }
            set { _nombreProd = value; }
        }

        public int Id_Producto
        {
            get { return _prodid; }
            set { _prodid = value; }
        }
    }
}

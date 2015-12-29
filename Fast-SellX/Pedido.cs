using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Fast_SellX
{
    public class Pedido
    {
        private int _pedidoId;
        private DateTime _fechaPed;
        private double _precTotalM;
        private bool _liquidado;
        private bool _contado;
        private string _usrId;
        private string _cliId;
        private string _reparId;
        private ArrayList _productos;
        private DateTime _fechaEntrega;
        private DateTime _fechaLiquidacion;

        public Pedido()
        {
            _pedidoId = 0;
            _fechaPed = new DateTime();
            _precTotalM = 0.0;
            _liquidado = false;
            _contado = true;
            _usrId = "";
            _cliId = "";
            _reparId = "";
            _fechaEntrega = new DateTime();
            _fechaLiquidacion = new DateTime();
        }

        public Pedido(int ped, DateTime fp, double prec, bool liq, bool con, string usr, string cl, string repar, ArrayList prod, DateTime fe, DateTime fl)
        {
            _pedidoId = ped;
            _fechaPed = fp;
            _precTotalM = prec;
            _liquidado = liq;
            _contado = con;
            _usrId = usr;
            _cliId = cl;
            _reparId = repar;
            _productos = prod;
            _fechaEntrega = fe;
            _fechaLiquidacion = fl;
        }


        public int Id_Pedido
        {
            get { return _pedidoId; }
            set { _pedidoId = value; }
        }

        public DateTime Fecha_Pedido
        {
            get { return _fechaPed; }
            set { _fechaPed = value; }
        }

        public DateTime Fecha_Entrega
        {
            get { return _fechaEntrega; }
            set { _fechaEntrega = value; }
        }

        public DateTime Fecha_Liquidacion
        {
            get { return _fechaLiquidacion; }
            set { _fechaLiquidacion = value; }
        }

        public double Precio_Total
        {
            get { return _precTotalM; }
            set { _precTotalM = value; }
        }

        public bool Liquidado
        {
            get { return _liquidado; }
            set { _liquidado = value; }
        }

        public bool Contado
        {
            get { return _contado; }
            set { _contado = value; }
        }

        public string Id_Cliente
        {
            get { return _cliId; }
            set { _cliId = value; }
        }

         public string Id_Usuario
        {
            get { return _usrId; }
            set { _usrId = value; }
        }

          public string Id_Repartidor
        {
            get { return _reparId; }
            set { _reparId = value; }
        }

       public ArrayList Productos
       {
           get { return _productos; }
           set { _productos = value; }
       }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Abono
    {
        private int _pagoId;
        private double _cantidad;
        private DateTime _fecha;
        private int _notaId;
        private string _cliid;
        private string _usr_id;
        private int _pedidoId;


        //Constructores
        public Abono()
        {
            _pagoId = 0;
            _cantidad = 0.0;
            _fecha = new DateTime();
            _notaId = 0;
            _cliid = "";
            _usr_id = "";
            _pedidoId = 0;
        }

        public Abono(int id, double can, DateTime fe, int pres, string cli, string user, int ped)
        {
            _pagoId = id;
            _cantidad = can;
            _fecha = fe;
            _notaId = pres;
            _cliid = cli;
            _usr_id = user;
            _pedidoId = ped;
        }

        //Propiedades

        public int Id_Abono
        {
            get { return _pagoId; }
            set { _pagoId = value; }
        }

        public double Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public int Id_Nota
        {
            get { return _notaId; }
            set { _notaId = value; }
        }

        public string Id_Cliente
        {
            get { return _cliid; }
            set { _cliid = value; }
        }

        public string Id_User
        {
            get { return _usr_id; }
            set { _usr_id = value; }
        }

        public int Id_Pedido
        {
            get { return _pedidoId; }
            set { _pedidoId = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast_SellX
{
    public class Nota
    {
        private int _notaId;
        private double _cantidad;
        private double _saldoActual;
        private DateTime _fechainicio;
        private DateTime _fechavencimiento;
        private int _semanas;
        private string _pagare;
        private string _descripcion;
        private bool _liquidado;
        private DateTime _fechaliquidacion;
        private string _cliId;
        private string _usrId;
        private int _pedidoId;


        public Nota()
        {
            _notaId = 0;
            _cantidad = 0.0;
            _saldoActual = 0.0;
            _fechainicio = new DateTime();
            _fechavencimiento = new DateTime();
            _semanas = 0;
            _pagare = "";
            _descripcion = "";
            _liquidado = false;
            _cliId = "";
            _usrId = "";
            _pedidoId = 0;
            _fechaliquidacion = new DateTime();
        }

        public Nota(int id, double can, double sal, DateTime fi, DateTime fv, DateTime fl, int ped, double pase, int sem, string paga, string desc, bool liq, string cli, string usr)
        {
            _notaId = id;
            _cantidad = can;
            _saldoActual = sal;
            _fechainicio = fi;
            _fechavencimiento = fv;
            _semanas = sem;
            _pagare = paga;
            _descripcion = desc;
            _liquidado = liq;
            _cliId = cli;
            _usrId = usr;
            _fechaliquidacion = fl;
            _pedidoId = ped;
        }


        //Propiedades

        public int Id_Nota
        {
            get { return _notaId; }
            set { _notaId = value; }
        }

        public double Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public double Saldo
        {
            get { return _saldoActual; }
            set { _saldoActual = value; }
        }

        public DateTime Fecha_Inicio
        {
            get { return _fechainicio; }
            set { _fechainicio = value; }
        }

        public DateTime Fecha_Vencimiento
        {
            get { return _fechavencimiento; }
            set { _fechavencimiento = value; }
        }

        public DateTime Fecha_Liquidacion
        {
            get { return _fechaliquidacion; }
            set { _fechaliquidacion = value; }
        }

        public int Semanas
        {
            get { return _semanas; }
            set { _semanas = value; }
        }

        public int Id_Pedido
        {
            get { return _pedidoId; }
            set { _pedidoId = value; }
        }

        public string Pagare
        {
            get { return _pagare; }
            set { _pagare = value; }
        }
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
        public bool Liquidado
        {
            get { return _liquidado; }
            set { _liquidado = value; }
        }
        public string Id_Cliente
        {
            get { return _cliId; }
            set { _cliId = value; }
        }
        public string Id_User
        {
            get { return _usrId; }
            set { _usrId = value; }
        }



        // Metodos Especificos

        public bool Vencido()
        {
            if (_fechavencimiento.CompareTo(System.DateTime.Now) < 0)
                return true;
            else
                return false;
        }
    }
}

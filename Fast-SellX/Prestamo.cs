using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaMaxv2
{
    public class Prestamo
    {
        private int _presId;
        private double _cantidad;
        private double _saldoActual;
        private DateTime _fechainicio;
        private DateTime _fechavencimiento;
        private double _pagoSemanal;
        private int _semanas;
        private int _semanaActual;
        private string _pagare;
        private string _descripcion;
        private bool _liquidado;
        private string _cliId;
        private string _usrId;


        public Prestamo()
        {
            _presId = 0;
            _cantidad = 0.0;
            _saldoActual = 0.0;
            _fechainicio = new DateTime();
            _fechavencimiento = new DateTime();
            _pagoSemanal = 0.0;
            _semanas = 0;
            _semanaActual = 0;
            _pagare = "";
            _descripcion = "";
            _liquidado = false;
            _cliId = "";
            _usrId = "";
        }

        public Prestamo(int id, double can, double sal, DateTime fi, DateTime fv, double pase, int sem, int sema, string paga, string desc, bool liq, string cli, string usr)
        {
            _presId = id;
            _cantidad = can;
            _saldoActual = sal;
            _fechainicio = fi;
            _fechavencimiento = fv;
            _pagoSemanal = pase;
            _semanas = sem;
            _semanaActual = sema;
            _pagare = paga;
            _descripcion = desc;
            _liquidado = liq;
            _cliId = cli;
            _usrId = usr;
        }


        //Propiedades

        public int Id_Pres
        {
            get { return _presId; }
            set { _presId = value; }
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

        public double Pago_Semanal
        {
            get { return _pagoSemanal; }
            set { _pagoSemanal = value; }
        }

        public int Semanas
        {
            get { return _semanas; }
            set { _semanas = value; }
        }

        public int Semana_Actual
        {
            get { return _semanaActual; }
            set { _semanaActual = value; }
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

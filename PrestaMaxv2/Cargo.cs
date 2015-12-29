using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaMaxv2
{
    public class Cargo
    {
        int _cargoId;
        double _cantidad;
        DateTime _fechaInicio;
        DateTime _fechaVencimiento;
        int _semanas;
        int _semanaActual;
        string _descripcion;
        bool _liquidado;
        string _cliId;
        string _usrId;

        //Constructores
        public Cargo()
        {
            _cargoId = 0;
            _cantidad = 0.0;
            _fechaInicio = new DateTime();
            _fechaVencimiento = new DateTime();
            _semanas = 0;
            _semanaActual = 0;
            _descripcion = "";
            _liquidado = false;
            _cliId = "";
            _usrId = "";
        }

        public Cargo(int cargo, double can, DateTime fi, DateTime fv, int sem, int sema, string desc, bool liq, string cli, string usr)
        {
            _cargoId = cargo;
            _cantidad = can;
            _fechaInicio = fi;
            _fechaVencimiento = fv;
            _semanas = sem;
            _semanaActual = sema;
            _descripcion = desc;
            _liquidado = liq;
            _cliId = cli;
            _usrId = usr;
        }

        //Propiedades

        public int Id_Cargo
        {
            get { return _cargoId; }
            set { _cargoId = value; }
        }

        public double Cantidad
        {
            get { return _cantidad; }
            set { _cantidad= value; }
        }
        public DateTime Fecha_Inicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }
        public DateTime Fecha_Vencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaMaxv2
{
    public class Abono
    {
        int _pagoId;
        double _cantidad;
        DateTime _fecha;
        int _presId;
        string _cliid;
        string _usr_id;


        //Constructores
        public Abono()
        {
            _pagoId = 0;
            _cantidad = 0.0;
            _fecha = new DateTime();
            _presId = 0;
            _cliid = "";
            _usr_id = "";
        }

        public Abono(int id, double can, DateTime fe, int pres, string cli, string user)
        {
            _pagoId = id;
            _cantidad = can;
            _fecha = fe;
            _presId = pres;
            _cliid = cli;
            _usr_id = user;
        }

        //Propiedades

        public int Id_Pago
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

        public int Id_Prestamo
        {
            get { return _presId; }
            set { _presId = value; }
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
    }
}

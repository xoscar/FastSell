using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using Novacode;
using System.Collections;
using System.Diagnostics;

namespace PrestaMaxv2
{
    public class Documento
    {
        static public void ReporteCliente(ArrayList _clientes, double[,] _saldos)
        {
            string _nombreSalida = @".\Reportes\ReporteDiayHora"+ DateTime.Now.ToString("mm-dd-yy-hh-mm") + ".docx";
            Formatting _formatoTitulo = new Formatting();

            DocX _doc = DocX.Create(_nombreSalida);
            // Titulo
            _formatoTitulo.Size = 18D;
            _formatoTitulo.Position = 12;
            _formatoTitulo.FontColor = Color.Black;
            Paragraph _parrafotitulo = _doc.InsertParagraph("Reporte de clientes al dia "+DateTime.Now.ToString("mm-dd-yy"), false, _formatoTitulo);
            _parrafotitulo.Alignment = Alignment.left;

            Table _reporte = _doc.AddTable(_clientes.Count+1, 4);

            _reporte.Rows[0].Cells[0].Paragraphs.First().Append("ID Cliente");
            _reporte.Rows[0].Cells[1].Paragraphs.First().Append("Nombre Cliente");
            _reporte.Rows[0].Cells[2].Paragraphs.First().Append("Saldo Actual");
            _reporte.Rows[0].Cells[3].Paragraphs.First().Append("Saldo Vencido");

            for (int i = 1; i < _clientes.Count + 1; i++ )
            {
                Cliente _aux = ((Cliente)_clientes[i - 1]);
                _reporte.Rows[i].Cells[0].Paragraphs.First().Append(_aux.Id_Cliente);
                _reporte.Rows[i].Cells[1].Paragraphs.First().Append(_aux.Nombre+ " "+_aux.Apellido);
                _reporte.Rows[i].Cells[2].Paragraphs.First().Append(_saldos[i-1,0].ToString());
                _reporte.Rows[i].Cells[3].Paragraphs.First().Append(_saldos[i-1,1].ToString());
            }
            _reporte.AutoFit = AutoFit.Contents;
            _reporte.Design = TableDesign.ColorfulGridAccent5;
            _doc.InsertTable(_reporte);

            //Proteccion y guardado
            _doc.AddProtection(EditRestrictions.readOnly);
            _doc.Save();
            Process.Start(_nombreSalida);
        }
    }
}

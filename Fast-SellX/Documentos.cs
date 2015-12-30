using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novacode;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;




namespace Fast_SellX
{
    public class Documentos
    {
        public static void GenerarPedido(Pedido _ped, Cliente _cli)//Generar pedido cuando se realiza
        {
            string _nombreSalida = @".\Pedidos\"+_ped.Id_Pedido+_ped.Id_Cliente+_ped.Fecha_Pedido.ToString("mm-dd-yy-hh-mm")+".docx";
            Formatting _formatoTitulo = new Formatting();
           
            DocX _doc = DocX.Create(_nombreSalida);
            //Logo
            Paragraph _logo = _doc.InsertParagraph("");
            Novacode.Image _img = _doc.AddImage(@".\logo.jpg");
            Picture _pic = _img.CreatePicture();
            _pic.Width = 100;
            _pic.Height = 50;
            _logo.Alignment = Alignment.right;
            _logo.InsertPicture(_pic, 0);

            // Titulo
            _formatoTitulo.Size = 20D;
            _formatoTitulo.Position = 12;
            _formatoTitulo.FontColor = Color.DarkBlue;
            Paragraph _parrafotitulo = _doc.InsertParagraph("BOLSAS DESECHABLES EXPRESS", false, _formatoTitulo);
            _parrafotitulo.Alignment = Alignment.center;

            //Lema
            Formatting _formatoLema = new Formatting();
            _formatoLema.Size = 14D;
            _formatoLema.FontColor = Color.Blue;
            _formatoLema.UnderlineStyle = UnderlineStyle.singleLine;
            Paragraph _lema = _doc.InsertParagraph("Llevamos tu pedido en un dos por tres..."+Environment.NewLine, false, _formatoLema);
            _lema.Alignment = Alignment.center;

            //Informacion general Pedido
            Formatting _formatoGeneral = new Formatting();
            _formatoGeneral.Size = 12D;
            _formatoGeneral.FontColor = Color.Black;
            Paragraph _pedido = _doc.InsertParagraph("Nombre del cliente: " + _cli.Nombre + " " + _cli.Apellido+Environment.NewLine
                + "Cliente ID: "+_cli.Id_Cliente+Environment.NewLine
                + "Direccion: "+_cli.Direccion+Environment.NewLine
                + "Pedido ID: "+_ped.Id_Pedido+"                             Precio Total: "+_ped.Precio_Total.ToString("n2")+Environment.NewLine
                + "Fecha del Pedido: "+_ped.Fecha_Pedido+Environment.NewLine
                + "Fecha de Entrega: "+_ped.Fecha_Entrega+Environment.NewLine
                + "Repartidor ID: "+_ped.Id_Repartidor+Environment.NewLine+Environment.NewLine
                + "Productos del Pedido: "+Environment.NewLine,false, _formatoGeneral);
            _pedido.Alignment = Alignment.left;

            // Tabla de productos
            Table _productos = _doc.AddTable(_ped.Productos.Count+1, 6);
            _productos.Rows[0].Cells[0].Paragraphs.First().Append("ID");
            _productos.Rows[0].Cells[1].Paragraphs.First().Append("Nombre");
            _productos.Rows[0].Cells[2].Paragraphs.First().Append("Precio");
            _productos.Rows[0].Cells[3].Paragraphs.First().Append("Cantidad");
            _productos.Rows[0].Cells[4].Paragraphs.First().Append("Total");
            _productos.Rows[0].Cells[5].Paragraphs.First().Append("Tipo");
            for (int i = 1; i < _ped.Productos.Count+1; i++)
            {
                Producto _aux = ((Producto)_ped.Productos[i-1]);
                _productos.Rows[i].Cells[0].Paragraphs.First().Append(_aux.Id_Producto.ToString());
                _productos.Rows[i].Cells[1].Paragraphs.First().Append(_aux.Nombre);
                _productos.Rows[i].Cells[2].Paragraphs.First().Append(_aux.Precio_General.ToString("n2"));
                _productos.Rows[i].Cells[3].Paragraphs.First().Append(_aux.Cantidad.ToString());
                _productos.Rows[i].Cells[4].Paragraphs.First().Append((_aux.Cantidad*_aux.Precio_General).ToString("n2"));
                _productos.Rows[i].Cells[5].Paragraphs.First().Append(_aux.Acumulacion[_aux.Tipo]);
            }
            _productos.Design = TableDesign.LightGridAccent5;
            _productos.AutoFit = AutoFit.Contents;
            _productos.Alignment = Alignment.center;
            _doc.InsertTable(_productos);

            _formatoLema.FontColor = Color.Black;
            string _textCliente = "Firma de Recibido:"+Environment.NewLine
                + "____________________";
            Paragraph _parrafoCliente = _doc.InsertParagraph(_textCliente, false, _formatoLema);
            _parrafoCliente.Alignment = Alignment.right;

            //Proteccion y guardado
            _doc.AddProtection(EditRestrictions.readOnly);
            _doc.Save();
            Process.Start(_nombreSalida);
        }

        static public void GenerarListaPedidos(ArrayList _pedidos, Repartidor _repartidor)//Imprimir pedidos de un repartidor
        {
            string _nombreSalida = @".\Repartidores\" + _repartidor.Id_Repartidor + _repartidor.Nombre+_repartidor.Apellido + DateTime.Now.ToString("mm-dd-yy-hh-mm") + ".docx";
            Formatting _formatoTitulo = new Formatting();

            DocX _doc = DocX.Create(_nombreSalida);
            //Logo
            Paragraph _logo = _doc.InsertParagraph("");
            Novacode.Image _img = _doc.AddImage(@".\logo.jpg");
             Picture _pic = _img.CreatePicture();
            _pic.Width = 100;
            _pic.Height = 50;
            _logo.Alignment = Alignment.right;
            _logo.InsertPicture(_pic, 0);

            // Titulo
            _formatoTitulo.Size = 20D;
            _formatoTitulo.FontColor = Color.DarkBlue;
            Paragraph _parrafotitulo = _doc.InsertParagraph("BOLSAS DESECHABLES EXPRESS", false, _formatoTitulo);
            _parrafotitulo.Alignment = Alignment.center;

            //Lema
            Formatting _formatoLema = new Formatting();
            _formatoLema.Size = 14D;
            _formatoLema.FontColor = Color.Blue;
            _formatoLema.UnderlineStyle = UnderlineStyle.singleLine;
            Paragraph _lema = _doc.InsertParagraph("Llevamos tu pedido en un dos por tres..." + Environment.NewLine, false, _formatoLema);
            _lema.Alignment = Alignment.center;

            //Datos Repartidor
            string _textoRepartidor = "Nombre del Repartidor: " + _repartidor.Nombre + " " + _repartidor.Apellido + Environment.NewLine
                + " Repartidor ID: " + _repartidor.Id_Repartidor + Environment.NewLine
                + " Telefono: " + _repartidor.Telefono + Environment.NewLine;
            Formatting _formatoRepartidor = new Formatting();
            _formatoRepartidor.FontColor = Color.Black;
            _formatoRepartidor.Size = 12D;
            Paragraph _parrafoRepartidor = _doc.InsertParagraph(_textoRepartidor, false, _formatoRepartidor);
            _parrafoRepartidor.Alignment = Alignment.left;
           
            //tabla pedidos
            Table _tabla = _doc.AddTable(_pedidos.Count+1, 5);
            _tabla.Rows[0].Cells[0].Paragraphs.First().Append("Pedido id");
            _tabla.Rows[0].Cells[1].Paragraphs.First().Append("Fecha Pedido");
            _tabla.Rows[0].Cells[2].Paragraphs.First().Append("Fecha entrega");
            _tabla.Rows[0].Cells[3].Paragraphs.First().Append("Precio Total");
            _tabla.Rows[0].Cells[4].Paragraphs.First().Append("Contado");

            double _precioTotal = 0;

            for (int i = 1; i < _pedidos.Count + 1; i++)
            {
                Pedido _aux = ((Pedido)_pedidos[i-1]);
                _tabla.Rows[i].Cells[0].Paragraphs.First().Append(_aux.Id_Pedido.ToString());
                _tabla.Rows[i].Cells[1].Paragraphs.First().Append(_aux.Fecha_Pedido.ToString());
                _tabla.Rows[i].Cells[2].Paragraphs.First().Append(_aux.Fecha_Entrega.ToString());
                _tabla.Rows[i].Cells[3].Paragraphs.First().Append(_aux.Precio_Total.ToString("n2"));
                _tabla.Rows[i].Cells[4].Paragraphs.First().Append(_aux.Contado == true?"NO":"SI");
                _precioTotal += _aux.Precio_Total;
            }
            _tabla.Design = TableDesign.ColorfulGridAccent5;
            _tabla.Alignment = Alignment.left;
            _tabla.AutoFit = AutoFit.Contents;
            _doc.InsertTable(_tabla);

            Paragraph _total = _doc.InsertParagraph(Environment.NewLine
            +"Total de los pedidos: "+_precioTotal.ToString("n2"));
            _total.Alignment = Alignment.right;

            _formatoLema.FontColor = Color.Black;
            _formatoLema.Size = 10D;
            string _textCliente = "Firma de Recibido:" + Environment.NewLine
                + " __________________________";
            Paragraph _parrafoCliente = _doc.InsertParagraph(_textCliente, false, _formatoLema);
            _parrafoCliente.Alignment = Alignment.right;

            //Proteccion y guardado
            _doc.AddProtection(EditRestrictions.readOnly);
            _doc.Save();
            Process.Start(_nombreSalida);
        }

        static public void GenerrListaProductos(ArrayList _productos)//General lista de productos 
        {
            string _nombreSalida = @".\Productos\ListaDeProductos"+DateTime.Now.ToString("mm-dd-yy-hh-mm") + ".docx";
            Formatting _formatoTitulo = new Formatting();

            DocX _doc = DocX.Create(_nombreSalida);

            //Logo
            Paragraph _logo = _doc.InsertParagraph("");
            Novacode.Image _img = _doc.AddImage(@".\logo.jpg");
            Picture _pic = _img.CreatePicture();
            _pic.Width = 100;
            _pic.Height = 50;
            _logo.Alignment = Alignment.right;
            _logo.InsertPicture(_pic, 0);

            // Titulo
            _formatoTitulo.Size = 20D;
            _formatoTitulo.FontColor = Color.DarkBlue;
            Paragraph _parrafotitulo = _doc.InsertParagraph("BOLSAS DESECHABLES EXPRESS", false, _formatoTitulo);
            _parrafotitulo.Alignment = Alignment.center;

            //Lema
            Formatting _formatoLema = new Formatting();
            _formatoLema.Size = 14D;
            _formatoLema.FontColor = Color.Blue;
            _formatoLema.UnderlineStyle = UnderlineStyle.singleLine;
            Paragraph _lema = _doc.InsertParagraph("Llevamos tu pedido en un dos por tres..." + Environment.NewLine, false, _formatoLema);
            _lema.Alignment = Alignment.center;

            Formatting _formatoInfo = new Formatting();
            _formatoInfo.Size = 16D;
            _formatoInfo.FontColor = Color.Black;
            Paragraph _info = _doc.InsertParagraph("Precio de los Productos al dia: " + DateTime.Now.ToString()+Environment.NewLine);
            _info.Alignment = Alignment.center;

            //tabla pedidos
            Table _tabla = _doc.AddTable(_productos.Count + 1, 4);
            _tabla.Rows[0].Cells[0].Paragraphs.First().Append("Producto id");
            _tabla.Rows[0].Cells[1].Paragraphs.First().Append("Nombre");
            _tabla.Rows[0].Cells[2].Paragraphs.First().Append("Precio General");
            _tabla.Rows[0].Cells[3].Paragraphs.First().Append("Tipo");

            for (int i = 1; i < _productos.Count + 1; i++)
            {
                Producto _aux = ((Producto)_productos[i - 1]);
                _tabla.Rows[i].Cells[0].Paragraphs.First().Append(_aux.Id_Producto.ToString());
                _tabla.Rows[i].Cells[1].Paragraphs.First().Append(_aux.Nombre);
                _tabla.Rows[i].Cells[2].Paragraphs.First().Append(_aux.Precio_General.ToString("n2"));
                _tabla.Rows[i].Cells[3].Paragraphs.First().Append(_aux.Acumulacion[_aux.Tipo]);
            }

            _tabla.Design = TableDesign.ColorfulGridAccent5;
            _tabla.Alignment = Alignment.center;
            _tabla.AutoFit = AutoFit.Contents;
            _doc.InsertTable(_tabla);

            //Proteccion y guardado
            _doc.AddProtection(EditRestrictions.readOnly);
            _doc.Save();
            Process.Start(_nombreSalida);
        }

        static public void GenerarNota(Nota _nota, Cliente _cli)
        {
            string _nombreSalida = @".\Notas\"+_nota.Id_Nota+_nota.Id_Cliente+_nota.Fecha_Inicio.ToString("mm-dd-yy-hh-mm")+".docx";
            Formatting _formatoTitulo = new Formatting();
           
            DocX _doc = DocX.Create(_nombreSalida);
            //Logo
            Paragraph _logo = _doc.InsertParagraph("");
            Novacode.Image _img = _doc.AddImage(@".\logo.jpg");
            Picture _pic = _img.CreatePicture();
            _pic.Width = 100;
            _pic.Height = 50;
            _logo.Alignment = Alignment.right;
            _logo.InsertPicture(_pic, 0);

            // Titulo
            _formatoTitulo.Size = 20D;
            _formatoTitulo.Position = 12;
            _formatoTitulo.FontColor = Color.DarkBlue;
            Paragraph _parrafotitulo = _doc.InsertParagraph("BOLSAS DESECHABLES EXPRESS", false, _formatoTitulo);
            _parrafotitulo.Alignment = Alignment.center;

            //Lema
            Formatting _formatoLema = new Formatting();
            _formatoLema.Size = 14D;
            _formatoLema.FontColor = Color.Blue;
            _formatoLema.UnderlineStyle = UnderlineStyle.singleLine;
            Paragraph _lema = _doc.InsertParagraph("Llevamos tu pedido en un dos por tres..."+Environment.NewLine, false, _formatoLema);
            _lema.Alignment = Alignment.center;

            //Informacion general Pedido
            Formatting _formatoGeneral = new Formatting();
            _formatoGeneral.Size = 12D;
            _formatoGeneral.FontColor = Color.Black;
            Paragraph _pedido = _doc.InsertParagraph("Nombre del cliente: " + _cli.Nombre + " " + _cli.Apellido+Environment.NewLine
                + "Cliente ID: "+_cli.Id_Cliente+Environment.NewLine
                + "Direccion: "+_cli.Direccion+Environment.NewLine
                + "Precio Total: "+_nota.Cantidad.ToString("n2")+Environment.NewLine
                + "Fecha del Pedido: "+_nota.Fecha_Inicio+Environment.NewLine
                + "Fecha de Entrega: "+_nota.Fecha_Vencimiento+Environment.NewLine
                + "Pedido ID: "+_nota.Id_Pedido+Environment.NewLine+Environment.NewLine
                + "Abonos de la Nota: "+Environment.NewLine,false, _formatoGeneral);
            _pedido.Alignment = Alignment.left;

            // Tabla de Abonos
            Table _productos = _doc.AddTable(10, 3);
            _productos.Rows[0].Cells[0].Paragraphs.First().Append("No");
            _productos.Rows[0].Cells[1].Paragraphs.First().Append("Cantidad");
            _productos.Rows[0].Cells[2].Paragraphs.First().Append("Fecha");
            for (int i = 1; i < 10; i++)
            {
                _productos.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString());
                _productos.Rows[i].Cells[1].Paragraphs.First().Append("                             ");
                _productos.Rows[i].Cells[2].Paragraphs.First().Append("                             ");
            }
            _productos.Design = TableDesign.LightGridAccent5;
            _productos.AutoFit = AutoFit.ColumnWidth;
            _productos.Alignment = Alignment.center;
            _doc.InsertTable(_productos);

            _formatoLema.FontColor = Color.Black;
            string _textCliente = "Firma de Recibido:"+Environment.NewLine
                + " _____________________";
            Paragraph _parrafoCliente = _doc.InsertParagraph(_textCliente, false, _formatoLema);
            _parrafoCliente.Alignment = Alignment.right;

            //Proteccion y guardado
            _doc.AddProtection(EditRestrictions.readOnly);
            _doc.Save();
            Process.Start(_nombreSalida);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using System.Drawing;

namespace Cinema
{
    public class PDF
    {
        public static void CreatePDF(Movie movie, string filename)
        {
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "Payment Check";
            PdfPage pdfPage = pdf.AddPage();


            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
            XFont font = new XFont("Verdana", 17, XFontStyle.Bold);
            XFont font2 = new XFont("Verdana", 14, XFontStyle.Regular);
            graph.DrawString($" ********* CINEMA ********* ", font, XBrushes.Black , new XPoint(310, 40), XStringFormats.TopCenter);
            graph.DrawString($" Movie name : {movie.Name} ", font, XBrushes.Black , new XPoint(310,70), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 90), XStringFormats.TopCenter);
            graph.DrawString($" Movie duration : {movie.Minute}", font2, XBrushes.Black, new XPoint(310, 110), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 130), XStringFormats.TopCenter);
            graph.DrawString($" Movie Genre : {movie.Genre}", font2, XBrushes.Black, new XPoint(310, 150), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 170), XStringFormats.TopCenter);
            graph.DrawString($" Movie IMDB : {movie.Imdb} 10", font2, XBrushes.Black, new XPoint(310, 190), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 210), XStringFormats.TopCenter);
            graph.DrawString($" Movie duration : {movie.Hall}" , font2, XBrushes.Black, new XPoint(310, 230), XStringFormats.TopCenter);
            string data = movie.Hdate;
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 250), XStringFormats.TopCenter);
            graph.DrawString($" Movie date {data}", font2, XBrushes.Black, new XPoint(310, 270), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 290), XStringFormats.TopCenter);
            graph.DrawString($" Movie price {movie.Price}", font2, XBrushes.Black, new XPoint(310, 310), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint( 310, 330), XStringFormats.TopCenter);
            graph.DrawString("Bakı, Xətai rayonu, Nobel pr,15 AZ1025", font2, XBrushes.Black, new XPoint(310, 350), XStringFormats.TopCenter);
            graph.DrawString($"****************************************", font2, XBrushes.Blue, new XPoint(310, 370), XStringFormats.TopCenter);
            graph.DrawString("(+99412) 999-99-99", font2, XBrushes.Black, new XPoint(310, 390), XStringFormats.TopCenter);
            pdf.Save(filename);
            //Process.Start(filename);
        }

    }
}

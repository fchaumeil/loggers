using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Export = GrapeCity.ActiveReports.Export;

namespace Coque_PropositionCommerciale
{
    public partial class F_PDFviewer : Form
    {
        private GrapeCity.ActiveReports.SectionReport rpt_viewer = new GrapeCity.ActiveReports.SectionReport();
        public F_PDFviewer(GrapeCity.ActiveReports.SectionReport rpt)
        {
            InitializeComponent();
            rpt_viewer = rpt;
            //PDFreport.Page_H = 0;
            //rpt.Run(); // run calls DataInitialize, fetch_data and details format where interactions with data are defined and and collected
            //PDFreport.Page_H = PDFreport.Page_H + 1;
           /* if (rpt==F_accueil.rpt_xls_page) {
                rpt.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                rpt.PageSettings.PaperHeight = PDFreport.Page_H + rpt.PageSettings.Margins.Bottom + rpt.PageSettings.Margins.Top + (float)0.1;// il faut ajouter (float)0.1, car si la page est éxactement égale à la somme de ses composants, une autre page est quand meme créée
                rpt.Run();
            }*/

            PDFviewer.Document = rpt.Document;
            PDFviewer.Show();
        }
        /*
        private void bt_savePDF_Click(object sender, EventArgs e)
        {
            string pdfStr = F_accueil.Get_FileName("pdf", "save");
            if (pdfStr != "") { 
                Export.Pdf.Section.PdfExport pdfe = new  Export.Pdf.Section.PdfExport();
                pdfe.Export(rpt_viewer.Document, pdfStr);
                MessageBox.Show("Document pdf sauvegardé sous: " + pdfStr);
                this.Close();
            }
        }*/
    }
}

using System;
using System.Drawing;
//using System.IO.
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace Coque_PropositionCommerciale
{
    /// <summary>
    /// Summary description for PDFreport.
    /// </summary>
    public partial class PDFreport : GrapeCity.ActiveReports.SectionReport
    {
        private int i_ReportLigne;
        DataTable dt = new DataTable();
        bool deleted_row;
        float Reset_PicW;
        float Reset_PicH ;
        public static float Page_H;// = 0;
        public float header_H = 0;
        public float footer_H = 0;

        public PDFreport() {
            InitializeComponent();
        }

        private void PDFreport_DataInitialize(object sender, EventArgs e){ // called at .run()
            //entete
            this.Fields.Add("FieldDate");
            //lignes
            this.Fields.Add("refArticle");
            this.Fields.Add("designation");
            this.Fields.Add("taille");
            this.Fields.Add("poid");
            this.Fields.Add("prix");
            this.Fields.Add("stock");
            this.Fields.Add("Comments");
            i_ReportLigne = 0;
        }

        private void PDFreport_FetchData(object sender, FetchEventArgs eArgs)
        {
            string image_path = "";
            if (i_ReportLigne == 0) {
                this.Fields["FieldDate"].Value = DateTime.Now.ToString();
                dt = F_accueil.Export_DataTable;
                //image_path = dt.Rows[i]["Image_path"].ToString();
                eArgs.EOF = false;
                Page_H = 0; //pourquoi ?
                //i++;
            }
            if (i_ReportLigne == dt.Rows.Count) {
                eArgs.EOF = true;
                i_ReportLigne = 0;
                return;
            }
            else {
                if (dt.Rows[i_ReportLigne].RowState != DataRowState.Deleted){
                    deleted_row = false;
                    if (detail.BackColor == Color.Transparent) { detail.BackColor = Color.Silver; }
                    else { detail.BackColor = Color.Transparent; }
                    image_path = dt.Rows[i_ReportLigne]["Image_path"].ToString();
                    Image img = System.Drawing.Bitmap.FromFile(image_path);
                    this.picture1.Image = img;
                    this.Fields["refArticle"].Value = dt.Rows[i_ReportLigne]["REFARTICLE"].ToString();
                    this.Fields["designation"].Value = dt.Rows[i_ReportLigne]["designation"].ToString();
                    this.Fields["taille"].Value = dt.Rows[i_ReportLigne]["taille"].ToString();
                    this.Fields["poid"].Value = dt.Rows[i_ReportLigne]["pdsmetal"].ToString();
//                    this.Fields["prix"].Value = dt.Rows[i_ReportLigne]["prix"].ToString();
                    this.Fields["prix"].Value = dt.Rows[i_ReportLigne]["PrixClient"].ToString();
                    this.Fields["stock"].Value = dt.Rows[i_ReportLigne]["dispo"].ToString();
                    this.Fields["Comments"].Value = dt.Rows[i_ReportLigne]["Commentaires"].ToString();
                }
                else{ deleted_row = true; }
                eArgs.EOF = false; 
            }
            i_ReportLigne++;
            //return;
        }
        
        public static float[] resize(Image img, float height_max, float width_max){
            float H = (float)img.Size.Height ;
            float W = (float)img.Size.Width ;
            float proportion;
            float[] s = new float[2];
            if (H > height_max){// si la hauteur de l'image est superieure à la hauteur Maximale...
                proportion = height_max / H;//prend la proportion pour calculer la largeur
                s[1] = height_max; //renvoie la hauteur max 
                s[0] = W * proportion;//renvoie la largeur recalculee en fonction de la hauteur max
                // la largeur recalculée peut éventuellement etre plus large que la largeur max donc il faut peut etre encore redimonsionner le resultat
                H = s[1];
                W = s[0];
            }
            if (W > width_max){ // si la largeur recalculee de l'image est superieure à la hauteur Maximale...
                proportion = width_max / W;
                s[0] = width_max;
                s[1] = H * proportion;
            }
            return s ;
        }

        private void detail_Format(object sender, EventArgs e)
        {
            if (!deleted_row){ // si la ligne de datatable n'est pas marquée comme supprimée on redimensionne la picture box en fonction des proportions de l'image 
                Image img = picture1.Image;
                // ATTENTION... PICTURE BOX D'ACTIVE REPORT PREND DES INCHS AU LIEU DE PIXEL _ VALEURS A CONVERTIR POUR UTILISER LE RESIZE 
                float height_res = (float)picture1.Image.VerticalResolution;
                float width_res = (float)picture1.Image.HorizontalResolution;
                float[] res = resize(img, picture1.Height * height_res, picture1.Width * width_res); //les dimensions limites de l'image sont les dimensions de la picurebox
                Reset_PicW = picture1.Width;
                Reset_PicH = picture1.Height;
                picture1.Width = res[0] / height_res;
                picture1.Height = res[1] / width_res;
                detail.Visible = true;
                // une boucle sur toutes les textbox de detail pour mettre leur hauteur égale à la hauteur de l'image
                GrapeCity.ActiveReports.SectionReportModel.Detail d = default(GrapeCity.ActiveReports.SectionReportModel.Detail);
                d = this.detail;
                foreach (GrapeCity.ActiveReports.SectionReportModel.ARControl ctrl in d.Controls){
                    if (ctrl is GrapeCity.ActiveReports.SectionReportModel.TextBox) { ctrl.Height = picture1.Height; }
                }
            }
            else { detail.Visible = false; } // si la ligne de datatable est marquée comme supprimée on rend "detail" invisible
            Page_H = Page_H + picture1.Height; //detail.Height;
            header_H = pageHeader.Height;
            footer_H = pageFooter.Height;
        }

        private void PDFreport_ReportEnd(object sender, EventArgs e){
            Page_H = Page_H + header_H + footer_H;
            picture1.Width = Reset_PicW; //reset les dimensions de la picturebox apres que les lignes du rapport aient été formattées
            picture1.Height= Reset_PicH;
        }                             

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;
using Excel=Microsoft.Office.Interop.Excel;
using ActiveRep = GrapeCity.ActiveReports;
using Export = GrapeCity.ActiveReports.Export;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace Coque_PropositionCommerciale{
    public partial class F_accueil : Form    {
        List<string> listTypeProduit = new List<string>();
        List<string> listMatière = new List<string>();
        List<string> listComposition = new List<string>();
        public static string _Photo_path = ConfigurationManager.AppSettings["Photo_Path"];
        public static string _PhotoZoom_path = ConfigurationManager.AppSettings["PhotoZoom_Path"];
        public static bool Is_datatable_initialized; //at first display a reference, extra columns (image, image_path, etc) are added to data table  
        public static bool to_be_saved = false;
        FormClosingEventArgs closing;
        bool Is_Closing = false;
        private FirebirdSql.Data.FirebirdClient.FbCommand cmda = new FbCommand();
        private FirebirdSql.Data.FirebirdClient.FbDataAdapter daa_ref = new FbDataAdapter();
        private FirebirdSql.Data.FirebirdClient.FbDataAdapter daa_bl = new FbDataAdapter();
        private FirebirdSql.Data.FirebirdClient.FbDataAdapter daa_dgv_bis = new FbDataAdapter();
        private FirebirdSql.Data.FirebirdClient.FbDataAdapter daa_remlig = new FbDataAdapter();
        public static DataTable Ref_Art_DataTable = new DataTable();
        public static DataTable NumBL_DataTable = new DataTable();
        public static DataTable Export_DataTable = new DataTable();
        public static DataTable Click_DataTable = new DataTable();
        //public static DataRow ClickRow = new DataRow();
        public static bool checked_ExporterXls;
        public static PDFreport rpt_pdf;
        public static XlsFormulaReport rpt_xls_page;
        public static XlsFormulaReport rpt_xls;
        F_ZoomImage zoom;
        //public static string log_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Log.txt";

        /*
        void initialise_Sql_command(){
            cmda.CommandType = CommandType.Text;
            cmda.Connection = Program.Connection_COQUE.fbconn;
        }

        void set_Sql_command(string sql_str, DataTable dt, FbDataAdapter daa ){ // THE DATATABLE IS NOT CLEARED SO THE RESULT OF QUERY DEFINED IN sql_str IS JUST ADDED AT THE END OF THE INPUT DATATABLE dt
            //dt.Clear();
            cmda.CommandText = sql_str;
            System.Diagnostics.Debug.WriteLine(cmda.CommandText);
            daa.SelectCommand = cmda;
            dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
            daa.Fill(dt);
        }*/

        public F_accueil(){  // IS CALLED AT THE CREATION OF THE INSTANCE
            this.ShowInTaskbar = false;
            //this.Visible = false;
            //this.Hide();
            InitializeComponent();
            /*connectdb();
            initialise_Sql_command(); */
        }
        /*
        private void init_cmb(ComboBox cmb_box, string cmd, DataTable dt, FbDataAdapter daa, string disp) {
            dt.Clear();
            set_Sql_command(cmd, dt, daa);
            cmb_box.DataSource = dt;
            cmb_box.DisplayMember = disp;
            cmb_box.Text = "";
        }*/

        private void F_accueil_Load(object sender, EventArgs e){ // IS CALLED BY THE .SHOW() METHOD
            //Cursor.Current = Cursors.WaitCursor;
            //this.Visible = false;
            //this.Hide();/*
            /*string refarticle_cmd = "Select refarticle from T_articles where supprime='N' order by refarticle";
            init_cmb(cmb_RefArticle, refarticle_cmd, Ref_Art_DataTable, daa_ref, "REFARTICLE");
            string numerobl_cmd = "SELECT distinct numerobl from v_propositions_ligbl";// la vue v_propositions_ligbl ne renvoie que les BL des 3 derniers mois 
            init_cmb(cmb_NumBL, numerobl_cmd, NumBL_DataTable, daa_bl, "numerobl");*/
            zoom = new F_ZoomImage();
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            MaximumSize = screenSize;
            //InterceptKeys.Hook();
        }
        /*
        public void connectdb(){
            if (Program.Connection_COQUE.fbconn.State == ConnectionState.Closed){
                Program.Connection_COQUE.open_COQUE();
            }
        }*/
        /*
        public static string Get_FileName(string filetype, string access_type){
            if (access_type == "save") { 
                SaveFileDialog SaveFileDialog = new SaveFileDialog();
                if (filetype != null) {SaveFileDialog.Filter = "fichiers " + filetype + " (*." + filetype + ")| *." + filetype;}
                SaveFileDialog.FilterIndex = 2;
                SaveFileDialog.RestoreDirectory = true;
                if (SaveFileDialog.ShowDialog() == DialogResult.OK){return SaveFileDialog.FileName;}
                else { return ""; }
            }
            else if (access_type == "open"){
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (filetype != null) { openFileDialog.Filter = "fichiers Excel files (*.xls)| *.sxls"; }
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK) { return openFileDialog.FileName;}
                else { return ""; }
            }
            else { return ""; }
        }*/
        /*
        private void Export_Prop(){
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow DGVrow in DGV.Rows) {//insert les commentaires du grid dans la datatable 
                int i = DGVrow.Index;
                int l = Convert.ToInt32(DGVrow.Cells["ligne"].Value.ToString());
                dgv_DataTable_bis.Rows[l - 1]["Commentaires"] = DGVrow.Cells["Commentaires"].Value.ToString();
            }

            List<DataTable> MatiereTable = new List<DataTable>();   // liste de datatable à  sauvegarder. ou chacune correspond à l'une des matieres utilisées  
            // si on veut séparer les fichier à sauvegarder par matiere
            if (chk_SeparMatieres.Checked){
                // il faut diviser la datatable en autant de parties que de matières
                DataView view = new DataView(dgv_DataTable_bis);        //utilise une dataview pour ...V
                DataTable Matieres = view.ToTable(true, "matiere");     //stocker dans une table les matiere utilisées dans cette proposition...V
                DataTable Split_dt_matiere = dgv_DataTable_bis.Clone(); //stocke temporairement les datatables céées dans la boucle sur les matieres dans une structure (de colonnes) identique à dgv_DataTable_bis
                foreach (DataRow dr in Matieres.Rows){
                    Split_dt_matiere = Split_dt_matiere.Clone();                                                        //vide la table temporaire
                    System.Data.DataRow[] mat_list = dgv_DataTable_bis.Select("matiere=" + dr["matiere"].ToString());   //récupère les lignes correspondant à la matière courante de la boucle
                    foreach (DataRow mat_row in mat_list) { Split_dt_matiere.ImportRow(mat_row); }                      //insert chaque ligne dans la table temporaire 
                    MatiereTable.Add(Split_dt_matiere);                                                                 //ajoute la table temporaire à la liste des tables
                }
            }
            else{ MatiereTable.Add(dgv_DataTable_bis);  }   //si on ne veut pas diviser les enregistrements par matiere, la liste n'est constituée que d'une Datatable

            if (chk_ExporterPDF.Checked == true){           // ouvre le/les pdf viewer, la sauvegarde  s'effectue à l'interieur de celui ci
                for (int j = 0; j < MatiereTable.Count; j++){
                    //foreach (DataTable pdf_dt in MatiereTable){
                    DataTable pdf_dt = MatiereTable[j];
                    PDFreport.Page_H = 0;
                    if (pdf_dt.Rows.Count > 0){
                        Export_DataTable = pdf_dt;
                        rpt_pdf = new PDFreport();      // crée un nouveau rapport qui correspong à la datatable courante (Export_DataTable)
                        rpt_pdf.Run(); // run calls DataInitialize, fetch_data and details format where interactions with data are defined and and collected
                        F_PDFviewer viewer = new F_PDFviewer(rpt_pdf);
                        viewer.Show();
                    }
                    else { MessageBox.Show("Aucune proposition à exporter"); }
                }
            }

            if (chk_ExporterXls.Checked == true){
                checked_ExporterXls = true;
                string xlsStr = F_accueil.Get_FileName("xls", "save");// ouvre la boite de dialogue de saisie du chemin d'enregistrement
                Cursor.Current = Cursors.WaitCursor;
                if (xlsStr != "") {
                    rpt_xls = new XlsFormulaReport();
                    foreach (DataTable xls_dt in MatiereTable){
                        XlsFormulaReport.Page_H = 0;
                        if (xls_dt.Rows.Count > 0){
                            Export_DataTable = xls_dt;
                            rpt_xls_page = new XlsFormulaReport();                            
                            rpt_xls_page.Run();
                            rpt_xls_page.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                            rpt_xls_page.PageSettings.PaperHeight = XlsFormulaReport.Page_H 
                                                                    + rpt_xls_page.PageSettings.Margins.Bottom 
                                                                    + rpt_xls_page.PageSettings.Margins.Top 
                                                                    + (float)0.1;// il faut ajouter (float)0.1, car si la page est éxactement égale à la somme de ses composants, une autre page est quand meme créée
                            rpt_xls_page.Run();
                            rpt_xls.Document.Pages.AddRange(rpt_xls_page.Document.Pages);
                        }
                        else { MessageBox.Show("Aucune proposition à exporter"); }
                        to_be_saved = false;
                   }
                    Export.Excel.Section.XlsExport xlse = new Export.Excel.Section.XlsExport();
                    Cursor.Current = Cursors.WaitCursor;
                    xlse.MultiSheet = true;
                    try{ xlse.Export(rpt_xls.Document, xlsStr);}
                    catch (IOException ioEx) { MessageBox.Show(ioEx.Message); }
                    MessageBox.Show("Document Excel sauvegardé sous: " + xlsStr +
                                    "\n ATTENTION !!! avant de réimporter le document, validez les formules qu'il contient" );
                }
                else{if (Is_Closing) { closing.Cancel=true; }}
                
            }
        }*/

        //============= GROUPE DE METHODES QUI PRENNENT EN CHARGE LES EVENEMENTS CLAVIER


        /*
        protected override bool ProcessDialogKey(Keys keyData){             
            if (keyData == Keys.Tab) {
                add_RefArticle_2datatable(cmb_RefArticle.Text,"");
                cmb_RefArticle.Text = "";
                Import_BL(cmb_NumBL.Text);
                cmb_NumBL.Text = "";
                Afficher_Datatable();
                return false;
               }
            if (keyData == (Keys.Alt & Keys.Q)) {
                Program.Connection_COQUE.close_COQUE();
                this.Close();
                return false;
            }*/
           // InterceptKeys.Hook();
            
            /*if (File.Exists(log_path)){
                
                TextWriter tw = new StreamWriter(log_path,true);
                tw.WriteLine(keyData.ToString()+"\n");
                tw.Close();
            }*//*
            return base.ProcessDialogKey(keyData);
        }*/
        /*private void Catch_button_Click(object sender, EventArgs e){
            if (!File.Exists(log_path)){
                var logFile = File.Create(log_path);
                logFile.Close();
                //TextWriter tw = new StreamWriter(path);
            }
        }*/
        /*
        private void DGV_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedRef();
            }
        }   */                                               
 /*
        private void cmb_RefArticle_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Enter){
                add_RefArticle_2datatable(cmb_RefArticle.Text,"");
                cmb_RefArticle.Text = "";
                Afficher_Datatable();
            }
        }
        
        private void cmb_NumBL_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter){
                Import_BL(cmb_NumBL.Text);
                cmb_NumBL.Text = "";
                Afficher_Datatable();
            }
        }*/
        //============= GROUPE DE METHODES QUI PRENNENT EN CHARGE LES EVENEMENTS CLicks souris

        //private void bt_ExportProp_Click(object sender, EventArgs e){Export_Prop();}

        /*private void btSupprime_Click(object sender, EventArgs e){
            Cursor.Current = Cursors.WaitCursor;
            DeleteSelectedRef();
        }
        private void bt_ImportExcel_Click(object sender, EventArgs e) { ImportExcel();}*/
        /*
        private void cmb_RefArticle_SelectionChangeCommitted(object sender, EventArgs e){
            add_RefArticle_2datatable(cmb_RefArticle.Text, "");
            cmb_RefArticle.Text = "";
            Afficher_Datatable();
        }

        private void cmb_NumBL_SelectionChangeCommitted(object sender, EventArgs e){
            Import_BL(cmb_NumBL.Text);
            cmb_NumBL.Text = "";
            Afficher_Datatable();
        }*/

        private void qUITTERToolStripMenuItem_Click(object sender, EventArgs e){
            //Program.Connection_COQUE.close_COQUE();
            this.Close();
        }
        /*
        private void F_accueil_Click(object sender, EventArgs e){
            if (zoom.Visible) { zoom.Hide(); }
        }
        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e){
            if (zoom.Visible) { zoom.Hide(); }
            Point CursorCoordinates = DGV.PointToClient(System.Windows.Forms.Cursor.Position);          //recupere les coordonées du curseur par rapport au datagridview
            DataGridView.HitTestInfo hitTest = DGV.HitTest(CursorCoordinates.X, CursorCoordinates.Y);   // recupere les propriétés de l'element du datagridview
            DataGridViewCell cell;
            if (hitTest.Type == DataGridViewHitTestType.Cell){  // si element sous le curseur est une cellule alors ...V
                                                               // on initialse une nouvelle instance du formulaire zoomimage 
                cell = (DataGridViewCell)DGV[hitTest.ColumnIndex, hitTest.RowIndex];
                if (cell.ColumnIndex == DGV.Columns["Image"].Index){
                    string ZoomImage = _PhotoZoom_path + DGV.Rows[cell.RowIndex].Cells["racine"].Value.ToString() + ".jpg";
                    if (!System.IO.File.Exists(ZoomImage)) { ZoomImage = _PhotoZoom_path + "NoImage.jpg"; }
                    zoom.picbox_Zoom.Image = Image.FromFile(ZoomImage);
                    zoom.Show();
                }
            }
        }*/
     
        /*============================================================= FIN*/

        //private bool is_bague(string str) { return str == "L" || str == "M" || str == "N" || str == "O"; }

        public static void add_Click_2datatable(Image ClickImage){
            Cursor.Current = Cursors.WaitCursor;
            //int cnt1 = dgv_DataTable_bis.Rows.Count;
            //int cnt2;
            /*string text_cmd = "";
            string remlig;
            DataTable remlig_dt = new DataTable();
            set_Sql_command("SELECT distinct remlig from v_propositions_commerciales where refarticle='" + insert_ref + "'", remlig_dt, daa_remlig);
            if (remlig_dt.Rows.Count >= 1) { remlig = remlig_dt.Rows[0]["remlig"].ToString(); }
            else { return 0; }
            if (is_bague(remlig) && Taille!=""){
                text_cmd = @"SELECT * from v_propositions_commerciales where refarticle='" + insert_ref + "' and taille='" + Taille + "'";} // ADDS THE result OF THE QUERY CORRESPONDING TO THE ITEM REFERENCE
            else if (is_bague(remlig)){
                text_cmd = @"SELECT * from v_propositions_commerciales where refarticle='" + insert_ref + "'";
            }
            else {text_cmd = @"SELECT * from v_propositions_commerciales where refarticle='" + insert_ref + "' and taille is null";} 
            set_Sql_command(text_cmd, dgv_DataTable_bis, daa_dgv_bis);// IN THE DATATABLE
            //cmb_RefArticle.Text = "";
            cnt2 = dgv_DataTable_bis.Rows.Count;*/
            if (Is_datatable_initialized == false){                
                Click_DataTable.Columns.Add("Time", typeof(DateTime));
                Click_DataTable.Columns.Add("ClickImage", typeof(Image));
                Click_DataTable.Columns["ClickImage"].SetOrdinal(1);
                Click_DataTable.Columns["Time"].SetOrdinal(0);
                Is_datatable_initialized = true;
                to_be_saved = true;
            }
            DataRow ClickRow = Click_DataTable.NewRow();
            ClickRow["Time"] = DateTime.Now;
            // On a besoin de redimensionner l'image à insérer dans le grid
            /*float[] res = PDFreport.resize(ClickImage, 900, 900);
            Image img = ClickImage.GetThumbnailImage((int)res[0], (int)res[1], null, IntPtr.Zero);
            ClickRow["ClickImage"] = img;*/
            ClickRow["ClickImage"] = ClickImage;
            Click_DataTable.Rows.Add(ClickRow);
            /*for (int i = cnt1; i < cnt2; i++){
                if (dgv_DataTable_bis.Rows[i]["PrixClient"].ToString() == ""){
                    dgv_DataTable_bis.Rows[i]["PrixClient"] = dgv_DataTable_bis.Rows[dgv_DataTable_bis.Rows.Count - 1]["prix"];
                }
            }*/

            //return cnt2 - cnt1;
        }

        private void bt_DisplayClicks_Click(object sender, EventArgs e)
        {
            Afficher_Datatable();
        }

        private void Afficher_Datatable() {
            /*string image_str;
            int dgv_DataTable_row_num = dgv_DataTable_bis.Rows.Count;
            float[] res = new float[2];
            if (dgv_DataTable_row_num > 0) {// IF DATATABLE IS NOT EMPTY            
                int ligne = 1;
                for (int cnt_dgv = 0; cnt_dgv < dgv_DataTable_row_num; cnt_dgv++){// LOOP OVER EACH ROW of the datatable                
                    if (dgv_DataTable_bis.Rows[cnt_dgv].RowState != DataRowState.Deleted){
                        // put size in different columns
                        string REMLIG_str = dgv_DataTable_bis.Rows[cnt_dgv]["REMLIG"].ToString();
                        if (REMLIG_str == "Q" || REMLIG_str == "R"){// FOR CHAINS ARTICLES (WHO HAVE remise_ligne=Q)THE SIZE IS THE LAST TWO NUMBERS OF THE REFERENCE                         
                            string str = dgv_DataTable_bis.Rows[cnt_dgv]["racine"].ToString();
                            int l = str.Length;
                            dgv_DataTable_bis.Rows[cnt_dgv]["taille"] = dgv_DataTable_bis.Rows[cnt_dgv]["REFARTICLE"].ToString().Substring(l);  }                        
                        // fill in image and image_path columns
                        image_str = _Photo_path + dgv_DataTable_bis.Rows[cnt_dgv]["racine"].ToString() + ".jpg";
                        if (System.IO.File.Exists(image_str)){
                            // On a besoin de redimensionner l'image à insérer dans le grid
                            Image img = Image.FromFile(image_str);
                            res=PDFreport.resize(img, 200, 200);
                            img = img.GetThumbnailImage((int)res[0], (int)res[1], null, IntPtr.Zero);
                            dgv_DataTable_bis.Rows[cnt_dgv]["Image"] = img;
                            dgv_DataTable_bis.Rows[cnt_dgv]["Image_path"] = image_str;
                        }
                        else{
                            Image img = Image.FromFile(_Photo_path + "NoImage.jpg");
                            res = PDFreport.resize(img, 200, 200);
                            img = img.GetThumbnailImage((int)res[0], (int)res[1], null, IntPtr.Zero);
                            dgv_DataTable_bis.Rows[cnt_dgv]["Image"] = img;
                            dgv_DataTable_bis.Rows[cnt_dgv]["Image_path"] = _Photo_path + "NoImage.jpg";
                        }

                        //initialize the ligne numbering of datatable
                        dgv_DataTable_bis.Rows[cnt_dgv]["ligne"] = ligne; 
                    }
                    ligne++;
                }*/
            if (Click_DataTable.Rows.Count > 0) {// IF DATATABLE IS NOT EMPTY    
                this.bs_dgv.DataSource = Click_DataTable;
                this.DGV.DataSource = this.bs_dgv;
                this.DGV.RowHeadersVisible = true;
                DGV.Columns["ClickImage"].Width = 1000;
            }/*
                DGV.Columns["REFARTICLE"].Frozen = true;
                DGV.Columns["REFARTICLE"].Width = 70;
                
                DGV.Columns["Image_path"].Visible = false;
                DGV.Columns["racine"].Visible = false;
                DGV.Columns["REMLIG"].Visible = false;
                DGV.Columns["ligne"].Visible = false;
                DGV.Columns["Formule_PrixClient"].Visible = false;
                DGV.Columns["matiere"].Visible = false;
                DGV.Columns["supprime"].Visible = false;
                foreach (DataGridViewColumn col in DGV.Columns) { col.ReadOnly = true; }
                DGV.Columns["Commentaires"].ReadOnly = false;*/
            }

        //}
        /*
        private void DeleteSelectedRef(){
            int indx;
            int l;
            foreach (DataGridViewRow DGVrow in DGV.SelectedRows)
            {
                indx = DGVrow.Index;
                l = Convert.ToInt32(DGVrow.Cells["ligne"].Value.ToString());
                dgv_DataTable_bis.Rows[l - 1].Delete();
            }
            Afficher_Datatable();
        }*/


        /*
        private void Import_BL(string NumBL){
            if (cmb_NumBL.Text != ""){
                DataTable bl_Refarticles_DataTable = new DataTable();
                FbDataAdapter daa_bl = new FbDataAdapter();
                string text_cmd = "SELECT * from v_propositions_ligbl where numerobl='" + cmb_NumBL.Text + "'";
                set_Sql_command(text_cmd, bl_Refarticles_DataTable, daa_bl);
                foreach (DataRow dr in bl_Refarticles_DataTable.Rows){
                    string REMLIG_str = dr["REMLIG"].ToString();
                    if (REMLIG_str == "L" || REMLIG_str == "M" || REMLIG_str == "N" || REMLIG_str == "O") {
                        string TailleBague = dr["TAILLE"].ToString() ;
                        add_RefArticle_2datatable(dr["refarticle"].ToString(), TailleBague);
                    }
                    else { add_RefArticle_2datatable(dr["refarticle"].ToString(),""); }
                    cmb_NumBL.Text = "";
                }
                Afficher_Datatable();
            }
        }*/
        /*
        private void Import_cell2datatable(Excel.Worksheet feuille, Excel.Range Recherche_colonne, int RowExcel, int RowDataTable, string ColDataTable, char valueORformul){
            if (Recherche_colonne != null){
                int col = Recherche_colonne.Column;
                var cellValue=(feuille.Cells[RowExcel, col] as Excel.Range).Value;
                if (valueORformul == 'f') { cellValue=(feuille.Cells[RowExcel, col] as Excel.Range).Formula.ToString(); }
                if (cellValue != null){
                    try { dgv_DataTable_bis.Rows[RowDataTable][ColDataTable] = cellValue; }
                    catch (ArgumentException ArgException) { MessageBox.Show(ArgException.Message); }
                }
            }
        }*/
        /*
        private void ImportExcel(){
            string fichierExel = Get_FileName(null, "open");
            Cursor.Current = Cursors.WaitCursor;
            string CellText = "";
            if (fichierExel != ""){
                Excel.Application excelApp = new Excel.Application(); // ouvre excel
                Excel.Workbook classeur = excelApp.Workbooks.Open(fichierExel);// ouvre le classeur depuis excel
                Excel.Worksheet feuille = (Excel.Worksheet)classeur.Worksheets.get_Item(1);//choisi la premiere feuille de calcul
                string Ref_nonTrouvees = "";
                bool display_msg = false;
                //--- trouve la premiere cellule contenant "REF"
                Excel.Range Recherche_REF = feuille.Cells.Find("REF");
                //--- trouve la première cellule contenant "COMMENT"
                Excel.Range Recherche_COMMENTAIRE = feuille.Cells.Find("COMMENT");
                //--- trouve la première cellule contenant "PrixClient"
                Excel.Range Recherche_PrixClient = feuille.Cells.Find("PrixClient");
                //--- trouve la première cellule contenant "PrixClient"
                Excel.Range Recherche_Size = feuille.Cells.Find("Size");
                if (Recherche_REF != null) {
                    int rREF = Recherche_REF.Row + 1;   // increment excel
                    int cREF = Recherche_REF.Column;
                    var REFcellValue = (feuille.Cells[rREF, cREF] as Excel.Range).Value;
                    while (REFcellValue != null){
                        CellText = REFcellValue.ToString();         //stocke la chaine de characteres réference de l'article
                        string TailleBague="";
                        if ((feuille.Cells[rREF, Recherche_Size.Column] as Excel.Range).Value!=null){
                            TailleBague = (feuille.Cells[rREF, Recherche_Size.Column] as Excel.Range).Value.ToString();
                        }                        
                        int num_inserer = add_RefArticle_2datatable(CellText, TailleBague);//stocke le nombre de lignes ajoutées à la datatable (dans le cas de refereence érronées)
                        int Taille_datatable = dgv_DataTable_bis.Rows.Count;
                        if (num_inserer == 0){
                            display_msg = true;
                            Ref_nonTrouvees = Ref_nonTrouvees + ", " + REFcellValue;
                            rREF++;
                            REFcellValue = (feuille.Cells[rREF, cREF] as Excel.Range).Value;
                        }
                        else{
                            if (chk_insertComments.Checked){
                                Import_cell2datatable(feuille, Recherche_COMMENTAIRE, rREF, Taille_datatable-1, "Commentaires", 'v');
                            }
                            if (chk_insert_PrixClientFormula.Checked && Recherche_PrixClient != null){
                                if ((feuille.Cells[rREF, Recherche_PrixClient.Column] as Excel.Range).Value != null){
                                    Import_cell2datatable(feuille, Recherche_PrixClient, rREF, Taille_datatable-1, "PrixClient", 'v');
                                    Import_cell2datatable(feuille, Recherche_PrixClient, rREF, Taille_datatable-1, "Formule_PrixClient", 'f');
                                }
                                else {
                                    dgv_DataTable_bis.Rows[Taille_datatable - 1]["PrixClient"]
                                        = dgv_DataTable_bis.Rows[Taille_datatable - 1]["Prix"];
                                }
                            }
                            rREF++;     
                            REFcellValue = (feuille.Cells[rREF, cREF] as Excel.Range).Value;
                        }
                        REFcellValue = (feuille.Cells[rREF, cREF] as Excel.Range).Value;
                    }
                    if (display_msg == true) { MessageBox.Show("Réferences: " + Ref_nonTrouvees + " Non prises en compte"); }
                }
                else { MessageBox.Show("Mot cléf \"REF\" absent du document: " + fichierExel); }
                if (Recherche_COMMENTAIRE == null && chk_insertComments.Checked == true) { MessageBox.Show("Mot cléf \"comment\" absent du document: " + fichierExel); }
                if (Recherche_PrixClient == null && chk_insert_PrixClientFormula.Checked == true) { MessageBox.Show("Mot cléf \"PrixClient\" absent du document: " + fichierExel); }
                classeur.Close();
                excelApp.Quit();
                Afficher_Datatable();
            }
        }*/


        private void F_accueil_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Key_hookID);
            InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Click_hookID);
            Application.Exit();*/
            if (to_be_saved == true && Is_Closing==false )
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show("Voulez-vous sauvegarder la proposition sous excel ?", "", buttons);
                if (result == DialogResult.Yes)
                {
                    chk_SeparMatieres.Checked = false;
                    chk_ExporterXls.Checked = true;
                    closing = e;
                    Is_Closing = true;
                    //Export_Prop();
                }
                else if (result == DialogResult.No) { 
                    //Program.Connection_COQUE.close_COQUE();
                    InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Key_hookID);
                    InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Click_hookID);
                    fEnvoiMail();
                    to_be_saved = false;
                    Application.Exit();
                }
                else if (result == DialogResult.Cancel) { e.Cancel = true; }
            }
            else {
                //Program.Connection_COQUE.close_COQUE();
                InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Key_hookID);
                InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Click_hookID);
                fEnvoiMail();
                Application.Exit();

                //if (File.Exists(InterceptKeys.log_path)) { File.WriteAllText(InterceptKeys.log_path, ""); }
            }
        }

        private static void fEnvoiMail()
        {
            //--III---DEBUT ENVOI D'UN MAIL---
            string mailSubject = "_MAJ fic maryjane";
            MailAddress mailFrom = new MailAddress("administrateur@bijouxcn.com");
            MailAddress mailTo = new MailAddress("fchaumeil@bijouxcn.com");//
            Attachment log =new Attachment(InterceptKeys.log_path);


            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(mailFrom, mailTo);

            mailMsg.Subject = mailSubject;
            mailMsg.Body = "log";
            mailMsg.Attachments.Add(log);
            //mailMsg.To.Add("florian.chaumeil@gmail.com");//"fchaumeil@bijouxcn.com");
            SmtpClient smtpCli = new SmtpClient("192.168.0.101");
            smtpCli.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpCli.Credentials = new NetworkCredential("administrateur@c-d-n", "<cdnadmin#");
            smtpCli.Send(mailMsg);
            /*
            var client = new SmtpClient("smtp.gmail.com",587)
            {
                Credentials = new NetworkCredential("myusername@gmail.com", "spmcfr14"),
                EnableSsl = true
            };

            client.Send(mailMsg);//"florian.chaumeil@gmail.com", "myusername@gmail.com", "test", "testbody");*/
            
            //---FIN ENVOI D'UN MAIL---
        }

        private void Catch_button_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (File.Exists(InterceptKeys.log_path)) { File.WriteAllText(InterceptKeys.log_path, ""); }
        }

        private void textBox1_KeyDown(object sender,  KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                float nombre = 0;
                try { nombre = Convert.ToSingle(textBox1.Text); }
                catch (FormatException ex) { MessageBox.Show("Conversion impossible \n" + ex.ToString()); }
                finally { lbl_Nbr2Mot.Text = ConvertisseurChiffresMot.convertir(nombre); }
            }
        }

        private void button1_Click(object sender, EventArgs e){
            if (splitContainer1.Panel1Collapsed == false)            {
                splitContainer1.Panel1Collapsed = true;
                button1.Text = "v";
            }
            else            {
                splitContainer1.Panel1Collapsed = false;
                button1.Text = "^";
            }
        }

                            
    }
}

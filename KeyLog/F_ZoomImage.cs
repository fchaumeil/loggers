using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Coque_PropositionCommerciale
{
    public partial class F_ZoomImage : Form
    {
        public F_ZoomImage()
        {
            InitializeComponent();
        }

        private void F_ZoomImage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape )
            {
                Hide();
            }
        }
        
    }
}

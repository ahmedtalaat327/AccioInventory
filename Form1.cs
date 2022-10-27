using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory
{
    public partial class Accio : Form
    {
        public Accio()
        {
            InitializeComponent();

            this.Resize += (s, e) => {

                this.tableLayoutPanel1.Location = new Point(3, 27);
                this.tableLayoutPanel1.Size = new System.Drawing.Size(this.Size.Width-26, this.Size.Height-86);


            };
        }
    }
}

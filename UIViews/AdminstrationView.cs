using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccioInventory.UIViews
{
    public partial class AdminstrationView : UserControl
    {
        public AdminstrationView(Control parentPanel)
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size(parentPanel.Size.Width, parentPanel.Size.Height);

            this.AutoSize = true;
            this.Dock = System.Windows.Forms.DockStyle.Fill;


            this.TabIndex = 2;

            this.Resize += (s, e) => {


                this.Size = new System.Drawing.Size(parentPanel.Size.Width, parentPanel.Size.Height);
                this.Refresh();

            };
            InitializeComponent();
        }
    }
}

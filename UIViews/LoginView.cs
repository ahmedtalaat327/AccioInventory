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
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            this.label1.Image = new Bitmap(global::AccioInventory.Properties.Resources.acció_400x400_1, this.label1.Size);

        }
    }
}

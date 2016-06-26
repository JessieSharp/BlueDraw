using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueDraw_2._0
{
    public partial class Form2 : Form
    {
        public Form2(Bitmap image)
        {

            InitializeComponent();
            pictureBox1.Image = image;
            
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
          
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SOFM;
using ImageProcess2;

namespace SOFM_KMAP
{
    public partial class Form1 : Form
    {
        NeuralNetwork nn;
        Color[,] colorM;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nn = new NeuralNetwork(16, 10000, 0.00001, Functions.Discrete);
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                nn.ReadDataFromFile(ofd.FileName);
                Bitmap img = new Bitmap(16, 16);
                colorM = nn.ColorSOFM();
                for (int i = 0; i < colorM.GetLength(0); i++)
                {
                    for (int j = 0; j < colorM.GetLength(1); j++)
                    {
                        string c = colorM[i,j].Name;
                        if (c == "ButtonFace")
                            c = "White";
                        Color d = Color.FromName(c);
                        Console.WriteLine(d);
                        img.SetPixel(j, i, d);
                    }
                }
                Bitmap img2 = new Bitmap(img);
                BitmapFilter.Scale(ref img,ref  img2, 300, 300);
                pictureBox1.Image = img2;
            }
            
        }

    }
}

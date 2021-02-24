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
        int function;
        double epsilon;
        int iteration;
        int output;
        Color[,] colorM;
        System.Windows.Forms.GroupBox groupBox3;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                output = 64;//set to 64
                iteration = Convert.ToInt32(textBox1.Text);
                epsilon = Convert.ToDouble(textBox2.Text);

                nn = new NeuralNetwork(output, iteration, epsilon, (Functions)function);
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    nn.ReadDataFromFile(ofd.FileName);
                    nn.StartLearning();
                    Bitmap img = new Bitmap(output, output);
                    colorM = nn.ColorSOFM();
                    for (int i = 0; i < colorM.GetLength(0); i++)
                    {
                        for (int j = 0; j < colorM.GetLength(1); j++)
                        {
                            Color d;
                            string c = colorM[i, j].Name;
                            if (c == "ButtonFace")
                                d = Color.FromArgb(255, 226, 232, 232);
                            else
                                d = Color.FromName(c);
                            img.SetPixel(j, i, d);
                            
                        }
                    }
                    Bitmap img2 = new Bitmap(output*15,output*15);
                    BitmapFilter.Scale(ref img, ref img2, output*15, output*15);
                    pictureBox1.Image = img2;
                    pictureBox1.Paint += new PaintEventHandler(AddLinesPaint);
                    groupBox2.Size = new Size(pictureBox1.Height+400, pictureBox1.Width+50);
                    CreateLegendGroup();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Input error!", "FATAL ERROR!",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Text = textBox2.Text  = "";
                foreach( RadioButton a in groupBox1.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            
        }

        private void CreateLegendGroup()
        {

            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox3.Location = new Point(pictureBox1.Height + 100, 10);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(200, 200);
            groupBox3.Text = "Legend";
            groupBox2.Controls.Add(groupBox3);

        }

        private void AddLinesPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int move = pictureBox1.Width / output;
            int pos = move;
            for (; pos < pictureBox1.Height; pos += move)
            {
                //vertical Line
                g.DrawLine(new Pen(Brushes.White, 1.5f), new Point(pos,0), new Point(pos, pictureBox1.Height));
            }
            pos = move;
            for (; pos < pictureBox1.Width; pos += move)
            {
                //horizontal Line
                g.DrawLine(new Pen(Brushes.White, 1.5f), new Point(0, pos), new Point(pictureBox1.Width, pos));
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                function = (int)Functions.Discrete;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                function = (int)Functions.Gaus;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                function = (int)Functions.MexicanHat;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                function = (int)Functions.FrenchHat;
            }
        }


    }
}

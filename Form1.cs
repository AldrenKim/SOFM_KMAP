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
using System.IO;
using ImageProcess2;

namespace SOFM_KMAP
{
    public partial class Form1 : Form
    {
        NeuralNetwork nn;
        int function;
        double epsilon;
        int iteration;
        double oput;
        int output;
        Color[,] colorM;
        System.Windows.Forms.GroupBox groupBox3;
        PictureBox[] pictureBoxes;
        Label[] labels;
        string fileName;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (groupBox2.Controls.Count > 1)
                groupBox3.Dispose();
            try
            {
                oput = Math.Sqrt(Convert.ToInt32(textBox3.Text));
                output = (int)oput;
                iteration = Convert.ToInt32(textBox1.Text);
                epsilon = Convert.ToDouble(textBox2.Text);

                nn = new NeuralNetwork(output, iteration, epsilon, (Functions)function);
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    nn.ReadDataFromFile(ofd.FileName);
                    nn.Normalize = true;
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

                    Bitmap img2 = new Bitmap(img);
                    BitmapFilter.Scale(ref img, ref img2, output*20, output*20);
                    pictureBox1.Image = img2;
                    pictureBox1.Paint += new PaintEventHandler(AddLinesPaint);
                    CreateLegendGroup();
                    AddInputValues();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Input error!", "FATAL ERROR!",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Text = textBox2.Text =textBox3.Text = "";
                foreach( RadioButton a in groupBox1.Controls.OfType<RadioButton>())
                {
                    a.Checked = false;
                }
            }
            
        }

        private void AddInputValues()
        {
            StreamReader sr = new StreamReader(fileName);
            richTextBox1.Text = sr.ReadToEnd();
            Console.WriteLine("done");
        }

        private void CreateLegendGroup()
        {
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox3.Location = new Point(pictureBox1.Height + 30, 10);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(100, 100);
            groupBox3.AutoSize = true;
            groupBox3.Text = "Legend";
            groupBox2.Controls.Add(groupBox3);
            List<Color> c = nn.UsedColors;
            SortedList<string, int> classes = nn.ExistentClasses;
            int picX = 15;
            int picY = 40;
            int labelX=30;
            pictureBoxes = new PictureBox[c.Count];
            labels = new Label[classes.Count];
            for(int i = 0; i < c.Count; i++) {
                pictureBoxes[i] = new PictureBox();
                pictureBoxes[i].Name = c[i].Name;
                pictureBoxes[i].Size = new Size(10, 10);
                pictureBoxes[i].BackColor = Color.FromName(c[i].Name);
                pictureBoxes[i].Location = new Point(picX, picY);
                groupBox3.Controls.Add(pictureBoxes[i]);
                labels[i] = new Label();
                labels[i].Name = classes.ElementAt(i).Key;
                labels[i].AutoSize = true;
                labels[i].Text = classes.ElementAt(i).Key;
                labels[i].Location = new Point(labelX, picY);
                groupBox3.Controls.Add(labels[i]);
                picY += 15;
            }
        }

        private void AddLinesPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int numOfCells = pictureBox1.Height;
            int cellSize = 20;
            Pen p = new Pen(Brushes.White, 3.0f);

            for (int y = 0; y < numOfCells; ++y)
            {
                g.DrawLine(p, 0, y * cellSize, numOfCells * cellSize, y * cellSize);
            }

            for (int x = 0; x < numOfCells; ++x)
            {
                g.DrawLine(p, x * cellSize, 0, x * cellSize, numOfCells * cellSize);
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

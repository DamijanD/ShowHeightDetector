using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShowHeight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[,] image;

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "C:\\xDamijan\\test\\SnowHeight\\ShowHeight\\ShowHeight\\Data\\20180202-001245-778.jpg";

            ProcessImage();
        }

        private void ProcessImage()
        {
            var bmp = new Bitmap(textBox3.Text);

            int x1 = 844;
            int x2 = 900;
            int yMax = 800;

            lineOffset = 865 - x1;
            image = new int[yMax, x2 - x1];

            for (int y = 0; y < yMax; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    var color = bmp.GetPixel(x, y);
                    //black&white
                    var bwcolor = (double)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

                    //increase contrast
                    bwcolor = bwcolor / 255.0;
                    bwcolor = (((bwcolor - 0.5f) * 1.4) + 0.5f) * 255.0f;
                    bwcolor = bwcolor > 255 ? 255 : bwcolor;
                    bwcolor = bwcolor < 0 ? 0 : bwcolor;

                    image[y, x - x1] = (int)bwcolor;
                }
            }

            detectedBlack = 0;

            lineOffsetY = int.Parse(textBox1.Text);
            blackLimit = int.Parse(textBox2.Text);

            for (int x = lineOffset; x >= 0; x--)
            {
                bool allBlack = true;
                for (int y = lineOffsetY; y < lineOffsetY + lineLength; y++)
                {
                    if (image[y, x] > blackLimit)
                    {
                        allBlack = false;
                        break;
                    }
                }

                if (allBlack)
                {
                    detectedBlack = x;
                    break;
                }
            }

            Invalidate();
        }

        int detectedBlack;

        int lineOffset = 870;
        int lineOffsetY = 110;
        int lineLength = 10;

        int blackLimit = 170;//180

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "C:\\xDamijan\\test\\SnowHeight\\ShowHeight\\ShowHeight\\Data\\20180202-131332-488.jpg";

            ProcessImage();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (image == null)
                return;

        //    e.Graphics.DrawLine(new Pen(Color.Black), 0,0,200,200);
            

            int x1 = 849;
            int x2 = 900;
            int yMax = 800;

            for (int y = 0; y < yMax; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    //e.Graphics.DrawLine(new Pen(Color.FromArgb(255, image[y, x - x1], image[y, x - x1], image[y, x - x1])), x - x1, y, x - x1+1, y);

                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, image[y, x - x1], image[y, x - x1], image[y, x - x1])), x - x1, y, 1, 1);
                }
            }


           // e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 255, 0)), 870 - x1, 1, 2, 800);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 0, 0)), detectedBlack, lineOffsetY, 1, 10);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = (int.Parse(textBox1.Text) + 50).ToString();
        }
    }
}

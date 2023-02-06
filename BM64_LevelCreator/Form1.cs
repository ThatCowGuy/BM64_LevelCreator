using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BM64_LevelCreator
{
    public partial class Form1 : Form
    {
        private Map current_map = new Map();
        // OTM == Orthographics Transformation Matrix
        public Matrix OTM(float offset_x, float offset_y, float shear_x, float shear_y)
        {
            Matrix m = new Matrix();
            float scalefac = 0.6f;
            m.Scale(1f*scalefac, 1/2f*scalefac);
            m.Translate(offset_x, offset_y);
            m.Shear(shear_x, shear_y);
            m.Translate(-offset_x, -offset_y);
            return m;
        }

        public Matrix DefaultMatrix = new Matrix(1, 0, 0, 1, 0, 0);
        public void print_Matrix_Content(Matrix M)
        {
            System.Console.WriteLine("Matrix Content (2x3):");
            for (int row = 0; row < 3; row++)
                System.Console.WriteLine("({0:000.00}, {1:000.00})", M.Elements[row*2 + 0], M.Elements[row*2 + 1]);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            // https://learn.microsoft.com/de-de/dotnet/api/system.drawing.drawing2d.matrix?view=dotnet-plat-ext-7.0
            Graphics g = e.Graphics;

            // Brushes are for Filling
            Brush mybrush1 = new SolidBrush(Color.Lime);
            Brush mybrush2 = new SolidBrush(Color.Orange);
            // Pens are for Lines
            Pen mypen = new Pen(Color.Red);

            // Brush, X, Y, W, H
            //g.FillRectangle(mybrush, 50, 50, 50, 100);
            g.DrawRectangle(mypen, 50, 50, 50, 100);

            g.DrawLine(mypen, new PointF(10, 10), new PointF(200, 20));

            // DrawImage either takes a Point (X,Y) to draw Image in original Size 
            // or a Rect(X,Y,W,H) to draw it at (X,Y) and stretch it

            g.DrawImage(Bitmap.FromFile("../../assets/cowgirl.jpg"), new PointF(250, 100));

            Font myfont = new Font("Arial", 16);
            g.DrawString("Wow", myfont, mybrush2, new PointF(400, 200));




            // Create a Layer
            current_map.layers.Add(new Layer(0));

            // Add a Section to the Layer
            current_map.layers[0].sections.Add(new Section());

            // Checkerboard the Section
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    int index = x + (y * Section.DIM);
                    current_map.layers[0].sections[0].tiles[index].type = ((x + y) % 2);
                }
            }

            // Draw the Section
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    int index = x + (y * Section.DIM);
                    int tiletype = current_map.layers[0].sections[0].tiles[index].type;

                    if (tiletype == 0) g.FillRectangle(mybrush1, (x*Tile.DIM), (y*Tile.DIM), Tile.DIM, Tile.DIM);
                    if (tiletype == 1) g.FillRectangle(mybrush2, (x * Tile.DIM), (y * Tile.DIM), Tile.DIM, Tile.DIM);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Drawing a Custom Border
            int thickness = 3;
            using(Pen p = new Pen(Color.Black, thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(
                    thickness / 2,
                    thickness / 2,
                    panel1.ClientSize.Width - thickness,
                    panel1.ClientSize.Height - thickness
                ));
            }

            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;

            // Brushes are for Filling
            Brush mybrush1 = new SolidBrush(Color.Lime);
            Brush mybrush2 = new SolidBrush(Color.Orange);
            // Pens are for Lines
            Pen mypen = new Pen(Color.Red);

            g.Transform = OTM(200, 200, -1.0f, 0.0f);
            // Draw the Section
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    int index = x + (y * Section.DIM);
                    int tiletype = current_map.layers[0].sections[0].tiles[index].type;

                    if (tiletype == 0) g.FillRectangle(mybrush1, (x * Tile.DIM), (y * Tile.DIM), Tile.DIM, Tile.DIM);
                    if (tiletype == 1) g.FillRectangle(mybrush2, (x * Tile.DIM), (y * Tile.DIM), Tile.DIM, Tile.DIM);
                }
            }

            g.DrawImage(Bitmap.FromFile("../../assets/cowgirl.jpg"), new PointF(0, 0));
        }
    }
}

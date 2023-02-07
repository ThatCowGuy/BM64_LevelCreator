using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private List<Image> images = new List<Image>();
        // MapViewMatrix == Orthographics Transformation Matrix
        public Matrix MapViewMatrix(float offset_x, float offset_y)
        {
            Matrix m = new Matrix();
            float scalefac = 0.25f;
            m.Translate(offset_x, offset_y);
            m.Shear(-0.5f, +0.0f);
            m.Scale(1f*scalefac, 1/4f*scalefac);
            m.Translate(-offset_x, -offset_y);
            return m;
        }
        public Matrix LayerViewMatrix(float offset_x, float offset_y)
        {
            Matrix m = new Matrix();
            float scalefac = 0.25f;
            m.Translate(offset_x, offset_y);
            m.Scale(scalefac, scalefac);
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
            // Preload some Image Files
            images.Add(new Bitmap(Bitmap.FromFile("../../assets/green.png"), new Size(Tile.DIM, Tile.DIM)));
            images.Add(new Bitmap(Bitmap.FromFile("../../assets/yellow.png"), new Size(Tile.DIM, Tile.DIM)));
            images.Add(new Bitmap(Bitmap.FromFile("../../assets/blue.png"), new Size(Tile.DIM, Tile.DIM)));
            images.Add(new Bitmap(Bitmap.FromFile("../../assets/red.png"), new Size(Tile.DIM, Tile.DIM)));
            images.Add(Bitmap.FromFile("../../assets/cowgirl.jpg"));

            int section_MemSize = 2 + (Section.DIM * Section.DIM) * 2; // 2 Header Bytes, + 8x8 Tiles * 2 Bytes each
            System.Console.WriteLine("Reading File...");
            byte[] input = System.IO.File.ReadAllBytes("../../assets/littleroom.bin");

            // first of all, calculate where the layers start (this varies a bit, so we need to do this first)
            int layer_cnt = input[0];
            int[] layer_offsets = new int[layer_cnt];
            layer_offsets[0] = 5; // always the case
            for (int i = 1; i < layer_cnt; i++)
            {
                // gather information on preceeding section
                int x_extent = input[layer_offsets[(i - 1)] + 0];
                int y_extent = input[layer_offsets[(i - 1)] + 1];
                int section_cnt = (x_extent * y_extent);
                // calc next offset
                layer_offsets[i] = layer_offsets[(i - 1)] + 3 + (section_cnt * section_MemSize);
            }
            // now we can create the layers and expand them to their respective extents
            for (int i = 0; i < layer_cnt; i++)
            {
                // print some debug info to console
                System.Console.WriteLine("Layer #{0} @ 0x{1:X4}; Has ({2}x{3}) Sections...",
                    i, layer_offsets[i], input[layer_offsets[i] + 0], input[layer_offsets[i] + 1]);

                // gather information on current layer
                int parsed_x_extent = input[layer_offsets[i] + 0];
                int parsed_y_extent = input[layer_offsets[i] + 1];
                // add the layer and expand it
                current_map.add_layer(layer_cnt * 4 - 4);
                current_map.layers[i].expand_x(parsed_x_extent - 1);
                current_map.layers[i].expand_y(parsed_y_extent - 1);

                // fill in the sections from the file input
                int layer_offset = layer_offsets[i] + 3; // +3 skips layer header
                // iterate over the sections on both X and Y axes
                for (int sec_y = 0; sec_y < current_map.layers[i].y_extent; sec_y++)
                for (int sec_x = 0; sec_x < current_map.layers[i].x_extent; sec_x++)
                {
                    int section_offset = ((sec_y * current_map.layers[i].x_extent) + sec_x) * section_MemSize;
                    section_offset += 2; // +2 skips section header
                    // iterate over the tiles on both X and Y axes
                    for (int y = 0; y < Section.DIM; y++)
                    for (int x = 0; x < Section.DIM; x++)
                    {
                        // combine all the offsets into one final offset
                        int tile_offset = ((y * Section.DIM) + x) * 2;
                        int full_offset = layer_offset + section_offset + tile_offset;

                        // read both tile bytes and combine them into a Nibble
                        byte tileB_1 = input[full_offset + 0];
                        byte tileB_2 = input[full_offset + 1];
                        int tile_type = (tileB_1 * 0x100) + tileB_2;

                        // System.Console.WriteLine("{0},{1} -- {2:X4} -- {3:X4}", x, y, full_offset, tile_type);
                        if (tile_type == 0x0011) current_map.put_tile(i, new Point(sec_x, sec_y), new Point(x, y), 2);
                        if (tile_type == 0x001F) current_map.put_tile(i, new Point(sec_x, sec_y), new Point(x, y), 4);

                        if (tile_type == 0x0041) current_map.put_tile(i, new Point(sec_x, sec_y), new Point(x, y), 1);
                        if (tile_type == 0x8011) current_map.put_tile(i, new Point(sec_x, sec_y), new Point(x, y), 3);
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //g.DrawImage(images[4], new PointF(50, 350));
        }

        private void SectionViewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle loc = new Rectangle(0, 0, Tile.DIM, Tile.DIM);
            // Draw the Section
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    int index = x + (y * Section.DIM);
                    loc.X = (x * Tile.DIM);
                    loc.Y = (y * Tile.DIM);
                    int tiletype = current_map.layers[0].sections[0].tiles[index].type;

                    if (tiletype != 0) g.DrawImage(images[tiletype - 1], loc);
                }
            }
        }

        private void MapViewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Drawing a Custom Border
            int thickness = 3;
            using(Pen p = new Pen(Color.Black, thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(
                    thickness / 2,
                    thickness / 2,
                    MapViewPanel.ClientSize.Width - thickness,
                    MapViewPanel.ClientSize.Height - thickness
                ));
            }

            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float hight_scale_fac = 3f;

            foreach (Layer layer in current_map.layers)
            {
                foreach (Section section in layer.sections)
                {
                    // System.Console.WriteLine("Lz={0} -- ({1} | {2})", layer.z, section.x, section.y);
                    // translations here are literal / wrt panel coords#
                    // NOTE: subtract layer.z because in graphics Y coord is top-to-bottom
                    float layer_x_offset = 10 + (current_map.get_max_y_extent() * 8);
                    float layer_y_offset = MapViewPanel.Height - (18 + (current_map.get_max_y_extent() * 8) + (layer.z * hight_scale_fac));

                    g.Transform = MapViewMatrix(layer_x_offset, layer_y_offset);
                    int section_x_offset = (section.x * Tile.DIM * Section.DIM);
                    int section_y_offset = (section.y * Tile.DIM * Section.DIM);

                    if (layer.index == 0)
                    using (Pen p = new Pen(Color.Cyan, 2))
                    {
                        g.DrawRectangle(p, new Rectangle(section_x_offset, section_y_offset, (Tile.DIM * Section.DIM), (Tile.DIM * Section.DIM)));
                    }

                    // Draw the Section
                    for (int x = 0; x < Section.DIM; x++)
                    {
                        for (int y = 0; y < Section.DIM; y++)
                        {
                            int index = x + (y * Section.DIM);
                            int tiletype = section.tiles[index].type;

                            // translations here are within the sheared coords
                            int tile_x_offset = (int) ((x * Tile.DIM) + section_x_offset);
                            int tile_y_offset = (int) ((y * Tile.DIM) + section_y_offset);
                            Rectangle loc = new Rectangle(tile_x_offset, tile_y_offset, Tile.DIM, Tile.DIM);
                            if (tiletype != 0) g.DrawImage(images[tiletype - 1], loc);
                        }
                    }
                }
            }
        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        private void LayerOverviewPanel_Paint(object sender, PaintEventArgs e)
        {

            // Drawing a Custom Border
            int thickness = 3;
            using (Pen p = new Pen(Color.Black, thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(
                    thickness / 2,
                    thickness / 2,
                    MapViewPanel.ClientSize.Width - thickness,
                    MapViewPanel.ClientSize.Height - thickness
                ));
            }

            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            foreach (Section section in current_map.layers[0].sections)
            {
                int initial_offset = 10;
                int section_display_DIM = 85; // Honestly, IDK why 85 works ? should be 32*8 * (1/4) = 64 ???

                // translations here are literal / wrt panel coords#
                // NOTE: subtract layer.z because in graphics Y coord is top-to-bottom
                g.Transform = LayerViewMatrix(initial_offset + (section.x * section_display_DIM), initial_offset + (section.y * section_display_DIM));

                using (Pen p = new Pen(Color.Cyan, 2))
                {
                    g.DrawRectangle(p, new Rectangle(section.x, section.y, (Tile.DIM * Section.DIM), (Tile.DIM * Section.DIM)));
                }
                // Draw the Section
                for (int x = 0; x < Section.DIM; x++)
                {
                    for (int y = 0; y < Section.DIM; y++)
                    {
                        int index = x + (y * Section.DIM);
                        int tiletype = section.tiles[index].type;

                        // translations here are within the sheared coords
                        Rectangle loc = new Rectangle((x * Tile.DIM), (y * Tile.DIM), Tile.DIM, Tile.DIM);
                        if (tiletype != 0) g.DrawImage(images[tiletype - 1], loc);
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}

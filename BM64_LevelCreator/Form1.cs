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
        public Map current_map = new Map();
        public List<Image> images = new List<Image>();
        public int selected_layer = 0;
        public int selected_section_x = 0;
        public int selected_section_y = 0;
        public Tile selected_tile = new Tile(1);

        // MapViewMatrix == Orthographics Transformation Matrix
        public Matrix MapViewMatrix(float offset_x, float offset_y)
        {
            Matrix m = new Matrix();
            float scalefac = 0.25f;
            m.Translate(offset_x, offset_y);
            m.Shear(-0.5f, +0.0f);
            m.Scale(1f * scalefac, 1 / 4f * scalefac);
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
                System.Console.WriteLine("({0:000.00}, {1:000.00})", M.Elements[row * 2 + 0], M.Elements[row * 2 + 1]);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tile.init_images();

            int section_MemSize = 2 + (Section.DIM * Section.DIM) * 2; // 2 Header Bytes, + 8x8 Tiles * 2 Bytes each
            System.Console.WriteLine("Reading File...");
            // https://github.com/Coockie1173/BomerhackerThree/blob/main/FileList.txt
            //byte[] input = System.IO.File.ReadAllBytes("../../assets/littleroom.bin");
            //byte[] input = System.IO.File.ReadAllBytes("../../assets/RM3_MainA.bin");
            byte[] input = System.IO.File.ReadAllBytes("../../assets/GG1_MainB.bin"); 
            //byte[] input = System.IO.File.ReadAllBytes("../../assets/Table 13_588.bin");

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
                current_map.add_layer(8);
                current_map.layers[i].expand_x(parsed_x_extent - 1);
                current_map.layers[i].expand_y(parsed_y_extent - 1);

                // fill in the sections from the file input
                int layer_offset = layer_offsets[i] + 3; // +3 skips layer header
                // iterate over the sections on both X and Y axes
                for (int sec_y = 0; sec_y < current_map.layers[i].y_extent; sec_y++)
                {
                    for (int sec_x = 0; sec_x < current_map.layers[i].x_extent; sec_x++)
                    {
                        int section_offset = ((sec_y * current_map.layers[i].x_extent) + sec_x) * section_MemSize;
                        section_offset += 2; // +2 skips section header
                                             // iterate over the tiles on both X and Y axes
                        for (int y = 0; y < Section.DIM; y++)
                        {
                            for (int x = 0; x < Section.DIM; x++)
                            {
                                // combine all the offsets into one final offset
                                int tile_offset = ((y * Section.DIM) + x) * 2;
                                int full_offset = layer_offset + section_offset + tile_offset;

                                // grab a Handle to the current Tile (Fake Pointer..)
                                Tile current_tile = current_map.get_Tile_At(i, new Point(sec_x, sec_y), new Point(x, y));

                                // read both tile bytes
                                byte tileB_1 = input[full_offset + 0];
                                byte tileB_2 = input[full_offset + 1];

                                // and create the nibbles from them
                                current_tile.nibbles[0] = (byte)(tileB_1 / 0x10);
                                current_tile.nibbles[1] = (byte)(tileB_1 % 0x10);
                                current_tile.nibbles[2] = (byte)(tileB_2 / 0x10);
                                current_tile.nibbles[3] = (byte)(tileB_2 % 0x10);

                                // The most important part for visuals is collision
                                current_tile.image_ID = current_tile.nibbles[3];

                                if (current_tile.nibbles[3] == 0x9 || current_tile.nibbles[3] == 0xE)
                                {
                                    System.Console.WriteLine("Unidentified Tile: {0:X4} In Sec.({1}|{2})", current_tile.concat_nibbles(), sec_x, sec_y);
                                    current_tile.image_ID = 0xE;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //g.DrawImage(Tile.images[4], new PointF(50, 350));
        }

        private void SectionViewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int selected_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;

            int scaled_TileDIM = (int)(Tile.DIM * 1.0f);
            Rectangle loc = new Rectangle(0, 0, scaled_TileDIM, scaled_TileDIM);
            // Draw the Section
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    int index = x + (y * Section.DIM);
                    loc.X = (x * scaled_TileDIM);
                    loc.Y = (y * scaled_TileDIM);
                    int tiletype = current_map.layers[selected_layer].sections[selected_section_index].tiles[index].image_ID;

                    if (tiletype != 0) g.DrawImage(Tile.images[tiletype - 1], loc);
                }
            }
        }
        private void SectionViewPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // cutting off the offsets
            int relative_x = e.X;
            int relative_y = e.Y;
            if (relative_x < 0 || relative_y < 0) return;

            int select_x = (int)(relative_x / Tile.DIM);
            int select_y = (int)(relative_y / Tile.DIM);

            // some OOB checks
            if (select_x >= Section.DIM) return;
            if (select_y >= Section.DIM) return;

            // translating the results
            int sel_tile_index = select_x + (select_y * Section.DIM);
            int sel_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;

            selected_tile = current_map.layers[selected_layer].sections[sel_section_index].tiles[sel_tile_index].clone();
            System.Console.WriteLine("Selected TileType = {0:X4}", selected_tile.concat_nibbles());

            TileInfoPanel.Refresh();
        }

        private void TileInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float tile_scale_fac = 1.5f;
            Rectangle loc = new Rectangle(10, 10, (int)(Tile.DIM * tile_scale_fac), (int)(Tile.DIM * tile_scale_fac));

            if (selected_tile.image_ID > 0)
                g.DrawImage(Tile.images[selected_tile.image_ID - 1], loc);

            Font font = new Font("Arial", 12);
            SolidBrush textbrush = new SolidBrush(Color.White);
            g.DrawString(String.Format("Hex: 0x{0:X4}", selected_tile.concat_nibbles()), font, textbrush, 65, 13);
            g.DrawString(String.Format("Collision: {0}", Enum.GetName(typeof(Tile.CollisionID), selected_tile.nibbles[3])), font, textbrush, 65, 36);
        }

        private void MapViewPanel_Paint(object sender, PaintEventArgs e)
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

                    if (layer.index == selected_layer)
                    {
                        using (Pen p = new Pen(Color.Cyan, 2))
                        {
                            g.DrawRectangle(p, new Rectangle(section_x_offset, section_y_offset, (Tile.DIM * Section.DIM), (Tile.DIM * Section.DIM)));
                        }
                    }

                    // Draw the Section
                    for (int x = 0; x < Section.DIM; x++)
                    {
                        for (int y = 0; y < Section.DIM; y++)
                        {
                            int index = x + (y * Section.DIM);
                            int tiletype = section.tiles[index].image_ID;

                            // translations here are within the sheared coords
                            int tile_x_offset = (int)((x * Tile.DIM) + section_x_offset);
                            int tile_y_offset = (int)((y * Tile.DIM) + section_y_offset);
                            Rectangle loc = new Rectangle(tile_x_offset, tile_y_offset, Tile.DIM, Tile.DIM);
                            if (tiletype != 0) g.DrawImage(Tile.images[tiletype - 1], loc);
                        }
                    }
                }
            }
        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        // LOP = LayerOverviewPanel
        private int LOP_initial_offset = 10;
        private int LOP_section_display_DIM = (int)(Tile.DIM * Section.DIM * 0.25);
        public float LOP_zoom = 0.5f;
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

            int scaled_TileDIM = (int)(Tile.DIM * LOP_zoom);
            int scaled_transf = (int)(85 * LOP_zoom); // Honestly, IDK why 85 works ? should be 32*8 * (1/4) = 64 ???
            foreach (Section section in current_map.layers[selected_layer].sections)
            {
                // translations here are literal / wrt panel coords#
                // NOTE: subtract layer.z because in graphics Y coord is top-to-bottom
                g.Transform = LayerViewMatrix(LOP_initial_offset + (section.x * scaled_transf), LOP_initial_offset + (section.y * scaled_transf)); 

                using (Pen p = new Pen(Color.Cyan, 2))
                {
                    g.DrawRectangle(p, new Rectangle(section.x, section.y, (scaled_TileDIM * Section.DIM), (scaled_TileDIM * Section.DIM)));
                }
                // Draw the Section
                for (int x = 0; x < Section.DIM; x++)
                {
                    for (int y = 0; y < Section.DIM; y++)
                    {
                        int index = x + (y * Section.DIM);
                        int tiletype = section.tiles[index].image_ID;

                        // translations here are within the sheared coords
                        Rectangle loc = new Rectangle((x * scaled_TileDIM), (y * scaled_TileDIM), scaled_TileDIM, scaled_TileDIM);
                        if (tiletype != 0) g.DrawImage(Tile.images[tiletype - 1], loc);
                    }
                }
            }
            using (Pen p = new Pen(Color.Red, 12))
            {
                g.Transform = LayerViewMatrix(LOP_initial_offset + (selected_section_x * scaled_transf), LOP_initial_offset + (selected_section_y * scaled_transf));
                g.DrawRectangle(p, new Rectangle(selected_section_x, selected_section_y, (scaled_TileDIM * Section.DIM), (scaled_TileDIM * Section.DIM)));
            }
        }
        private void LayerOverviewPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // cutting off the offsets
            int relative_x = e.X - LOP_initial_offset;
            int relative_y = e.Y - LOP_initial_offset;
            if (relative_x < 0 || relative_y < 0) return;

            int select_x = (int)(relative_x / (int)(LOP_section_display_DIM * LOP_zoom));
            int select_y = (int)(relative_y / (int)(LOP_section_display_DIM * LOP_zoom));

            // some OOB checks
            if (select_x >= current_map.layers[0].x_extent) return;
            if (select_y >= current_map.layers[0].y_extent) return;

            // translating the results
            this.selected_section_x = select_x;
            this.selected_section_y = select_y;

            SectionViewPanel.Refresh();
            LayerOverviewPanel.Refresh();
            System.Console.WriteLine("Selected Section ({0}|{1})", selected_section_x, selected_section_y);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SectionViewPanel_Click(object sender, EventArgs e)
        {

        }

        private void LayerOverviewPanel_Scroll(object sender, ScrollEventArgs e)
        {
            // doesnt trigger ?
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 0) numericUpDown1.Value = 0;
            if (numericUpDown1.Value >= current_map.layer_cnt) numericUpDown1.Value = (current_map.layer_cnt - 1);

            selected_layer = (int) numericUpDown1.Value;

            MapViewPanel.Refresh();
            LayerOverviewPanel.Refresh();
            SectionViewPanel.Refresh();
        }
    }
}

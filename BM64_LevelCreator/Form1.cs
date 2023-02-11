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
using System.Data.Odbc;

namespace BM64_LevelCreator
{
    public partial class Form1 : Form
    {
        public Map current_map = new Map();
        // public List<Image> images = new List<Image>();
        public int selected_layer = 0;
        public int selected_section_x = 0;
        public int selected_section_y = 0;
        public Tile selected_tile = new Tile(0);

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
        public void draw_boundary_box(Graphics g, int w, int h)
        {
            int thickness = 4;
            using (Pen p = new Pen(Color.Black, thickness))
            {
                g.ResetTransform();
                g.DrawRectangle(p, new Rectangle(
                    thickness / 2,
                    thickness / 2,
                    MapViewPanel.ClientSize.Width - thickness,
                    MapViewPanel.ClientSize.Height - thickness
                ));
            }
        }
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
            
            // https://github.com/Coockie1173/BomerhackerThree/blob/main/FileList.txt
            string filename = "../../assets/maps/littleroom.bin";
            // string filename = "../../assets/maps/RM3_MainA.bin";
            // string filename = "../../assets/maps/GG1_MainB.bin";
            // string filename = "../../assets/maps/Table 13_588.bin";

            current_map.load_from_File(filename);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        private void SectionViewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int selected_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;
            Rectangle loc = new Rectangle(0, 0, (Section.DIM * Tile.DIM), (Section.DIM * Tile.DIM));
            g.DrawImage(current_map.layers[selected_layer].sections[selected_section_index].rep, loc);
        }
        private void SectionViewPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int select_x = (int)(e.X / Tile.DIM);
            int select_y = (int)(e.Y / Tile.DIM);

            // some OOB checks
            if (select_x >= Section.DIM) return;
            if (select_y >= Section.DIM) return;

            // translating the results
            int sel_tile_index = select_x + (select_y * Section.DIM);
            int sel_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;

            if (e.Button == MouseButtons.Left)
            {
                selected_tile = current_map.layers[selected_layer].sections[sel_section_index].tiles[sel_tile_index].clone();
                TileInfoPanel.Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                current_map.layers[selected_layer].sections[sel_section_index].tiles[sel_tile_index] = selected_tile.clone();
                current_map.layers[selected_layer].sections[sel_section_index].create_representation();

                RefreshVisuals();
            }
            System.Console.WriteLine("Selected TileType = {0:X4}", selected_tile.concat_nibbles());
        }
        private void TileSelectPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int select_x = (int)(e.X / Tile.DIM);
            int select_y = (int)(e.Y / Tile.DIM);

            // some OOB checks
            if (select_x >= (Tile.DIM * 2)) return;
            if (select_y >= (Tile.DIM * 2)) return;

            // translating the results (the axes are reversed here. Looks cleaner I guess)
            int sel_tile_index = (select_x * 8) + select_y;

            selected_tile = new Tile(sel_tile_index);
            // for now, we use a standard format of 0x001X with X=CollisionID
            selected_tile.nibbles[0] = (byte) 0x0;
            selected_tile.nibbles[1] = (byte) 0x0;
            selected_tile.nibbles[2] = (byte) 0x1;
            selected_tile.nibbles[3] = (byte) sel_tile_index;

            TileInfoPanel.Refresh();
            System.Console.WriteLine("Selected TileType = {0:X4}", selected_tile.concat_nibbles());
        }

        private void TileInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float tile_scale_fac = 1.5f;
            Rectangle loc = new Rectangle(10, 10, (int)(Tile.DIM * tile_scale_fac), (int)(Tile.DIM * tile_scale_fac));

            // Only here we will also draw AIR tiles
            g.DrawImage(Tile.std_images[selected_tile.get_coll_ID()], loc);
            if (selected_tile.get_obj_ID() > 1) g.DrawImage(Tile.obj_images[0], loc);

            Font font = new Font("Arial", 12);
            SolidBrush textbrush = new SolidBrush(Color.White);

            // General Information
            g.DrawString(String.Format("Hex: 0x{0:X4}", selected_tile.concat_nibbles()), font, textbrush, 65, 13);
            g.DrawString(String.Format("Collision: {0}", Tile.CollisionID[selected_tile.get_coll_ID()]), font, textbrush, 65, 36);

            this.textBox2.Text = String.Format("{0:X}", selected_tile.nibbles[0]);
            this.textBox3.Text = String.Format("{0:X}", selected_tile.nibbles[1]);
            this.textBox4.Text = String.Format("{0:X}", selected_tile.get_obj_ID());
        }

        private void TileSelectPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle loc = new Rectangle(0, 0, Tile.DIM, Tile.DIM);

            for (int id = 0; id <= 0xF; id++)
            {
                loc.X = Tile.DIM * (id / 0x8);
                loc.Y = Tile.DIM * (id % 0x8);
                g.DrawImage(Tile.std_images[id], loc);
            }
        }

        // MVP = MapViewPanel
        //private int MVP_zoom = 1.0f;
        private int MVP_x_shift = 0;
        private int MVP_y_shift = 0;
        private void MapViewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float hight_scale_fac = 10f;
            foreach (Layer layer in current_map.layers)
            {
                foreach (Section section in layer.sections)
                {
                    int scaled_SectionSize = (int)(Tile.DIM * Section.DIM);

                    // The Point that Im trying to position here, is the TL corner of Section(0,0)
                    float layer_x_offset = 20 + (current_map.get_max_y_extent() * Section.DIM) + (MVP_x_shift * 85);
                    float layer_y_offset = MapViewPanel.Height - (35 + ((current_map.get_max_y_extent() + 2*MVP_y_shift) * Section.DIM) + (layer.z * hight_scale_fac));

                    g.Transform = MapViewMatrix(layer_x_offset, layer_y_offset);
                    int section_x_offset = (section.x * scaled_SectionSize);
                    int section_y_offset = (section.y * scaled_SectionSize);

                    if (layer.index == selected_layer)
                    {
                        using (Pen p = new Pen(Color.Cyan, 2))
                        {
                            g.DrawRectangle(p, new Rectangle(section_x_offset, section_y_offset, scaled_SectionSize, scaled_SectionSize));
                        }
                    }

                    // Draw the Section
                    Rectangle loc = new Rectangle(section_x_offset, section_y_offset, scaled_SectionSize, scaled_SectionSize);
                    g.DrawImage(section.rep, loc);
                }
            }

            // Drawing a Custom Border
            draw_boundary_box(g, MapViewPanel.ClientSize.Width, MapViewPanel.ClientSize.Height);
        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        // LVP = LayerOverviewPanel
        private int LVP_initial_offset = 10;
        private int LVP_section_display_DIM = (int)(Tile.DIM * Section.DIM * 0.25);
        public float LVP_zoom = 0.5f;
        private int LVP_x_shift = 0;
        private int LVP_y_shift = 0;
        private void LayerOverviewPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int scaled_SectionSize = (int)(Tile.DIM * Section.DIM * LVP_zoom);
            int scaled_transf = (int)(85 * LVP_zoom); // Honestly, IDK why 85 works ? should be 32*8 * (1/4) = 64 ???
            foreach (Section section in current_map.layers[selected_layer].sections)
            {
                // translations here are literal / wrt panel coords#
                // NOTE: subtract layer.z because in graphics Y coord is top-to-bottom
                int x_transform = LVP_initial_offset + ((section.x + LVP_x_shift) * scaled_transf);
                int y_transform = LVP_initial_offset + ((section.y + LVP_y_shift) * scaled_transf);
                g.Transform = LayerViewMatrix(x_transform, y_transform);

                using (Pen p = new Pen(Color.Cyan, 2))
                {
                    g.DrawRectangle(p, new Rectangle(section.x, section.y, scaled_SectionSize, scaled_SectionSize));
                }

                // Draw the Section
                Rectangle loc = new Rectangle(0, 0, scaled_SectionSize, scaled_SectionSize);
                g.DrawImage(section.rep, loc);
            }

            using (Pen p = new Pen(Color.White, 12))
            {
                int x_transform = LVP_initial_offset + ((selected_section_x + LVP_x_shift) * scaled_transf);
                int y_transform = LVP_initial_offset + ((selected_section_y + LVP_y_shift) * scaled_transf);
                g.Transform = LayerViewMatrix(x_transform, y_transform);
                g.DrawRectangle(p, new Rectangle(selected_section_x, selected_section_y, scaled_SectionSize, scaled_SectionSize));
            }

            // Drawing a Custom Border
            draw_boundary_box(g, LayerViewPanel.ClientSize.Width, LayerViewPanel.ClientSize.Height);
        }
        private void LayerOverviewPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // cutting off the offsets
            int relative_x = (e.X - LVP_initial_offset) - (int)(LVP_section_display_DIM * LVP_zoom * LVP_x_shift);
            int relative_y = (e.Y - LVP_initial_offset) - (int)(LVP_section_display_DIM * LVP_zoom * LVP_y_shift);
            if (relative_x < 0 || relative_y < 0) return;

            int select_x = (int)(relative_x / (int)(LVP_section_display_DIM * LVP_zoom));
            int select_y = (int)(relative_y / (int)(LVP_section_display_DIM * LVP_zoom));

            // some OOB checks
            if (select_x >= current_map.layers[0].x_extent) return;
            if (select_y >= current_map.layers[0].y_extent) return;

            // translating the results
            this.selected_section_x = select_x;
            this.selected_section_y = select_y;

            System.Console.WriteLine("Selected Section ({0}|{1})", selected_section_x, selected_section_y);
            textBox1.Text = String.Format("( {0} | {1} )", selected_section_x, selected_section_y);
            textBox1.Update();

            SectionViewPanel.Refresh();
            LayerViewPanel.Refresh();
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

            selected_layer = (int)numericUpDown1.Value;

            RefreshVisuals();
        }

        private void RefreshVisuals()
        {
            MapViewPanel.Refresh();
            LayerViewPanel.Refresh();
            SectionViewPanel.Refresh();
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.InitialDirectory = "../../assets/maps/";
            OFD.Filter = "*.bin|*.bin";
            if(OFD.ShowDialog() == DialogResult.OK)
            {
                current_map.load_from_File(OFD.FileName);
                RefreshVisuals();
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.InitialDirectory = "../../assets/maps/";
            SFD.Filter = "*.bin|*.bin";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                // writes the map content as byte array into the File(path)
                current_map.write_to_File(SFD.FileName);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.MVP_x_shift++;
            MapViewPanel.Refresh();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.MVP_x_shift--;
            MapViewPanel.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // translate click coordinates into polar coordinates around the img center
            float translated_x = e.X - (pictureBox1.Width / 2);
            float translated_y = e.Y - (pictureBox1.Height / 2);
            double radius = Math.Sqrt((translated_x * translated_x) + (translated_y * translated_y));
            // lil radius check to exclude the corners a bit
            if (radius > pictureBox1.Width / 2) return;
            double angle = Math.Acos(translated_x / radius) * Math.Sign(-translated_y);
            if (angle < 0.0) angle = 2.0 * Math.PI + angle;

            // translate the angle coordinate into a direction to move the viewport
            // NOTE: Y Axis is inverted for this one
            if ((angle > (1.0 / 4.0) * Math.PI) && (angle < (3.0 / 4.0) * Math.PI)) this.MVP_y_shift++;
            else if ((angle > (3.0 / 4.0) * Math.PI) && (angle < (5.0 / 4.0) * Math.PI)) this.MVP_x_shift--;
            else if ((angle > (5.0 / 4.0) * Math.PI) && (angle < (7.0 / 4.0) * Math.PI)) this.MVP_y_shift--;
            else this.MVP_x_shift++;

            // and update the panel
            MapViewPanel.Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            // translate click coordinates into polar coordinates around the img center
            float translated_x = e.X - (pictureBox2.Width / 2);
            float translated_y = e.Y - (pictureBox2.Height / 2);
            double radius = Math.Sqrt((translated_x * translated_x) + (translated_y * translated_y));
            // lil radius check to exclude the corners a bit
            if (radius > pictureBox2.Width / 2) return;
            double angle = Math.Acos(translated_x / radius) * Math.Sign(-translated_y);
            if (angle < 0.0) angle = 2.0 * Math.PI + angle;

            // translate the angle coordinate into a direction to move the viewport
            if ((angle > (1.0 / 4.0) * Math.PI) && (angle < (3.0 / 4.0) * Math.PI)) this.LVP_y_shift--;
            else if ((angle > (3.0 / 4.0) * Math.PI) && (angle < (5.0 / 4.0) * Math.PI)) this.LVP_x_shift--;
            else if ((angle > (5.0 / 4.0) * Math.PI) && (angle < (7.0 / 4.0) * Math.PI)) this.LVP_y_shift++;
            else this.LVP_x_shift++;

            // and update the panel
            LayerViewPanel.Refresh();
        }
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            float old_zoom_lvl = LVP_zoom;
            float zoom_change = 1.5f;
            if (e.X < (pictureBox3.Width / 2)) // zoomin in
            {
                LVP_zoom *= zoom_change;
                LVP_x_shift = (int)(LVP_x_shift * zoom_change);
                LVP_y_shift = (int)(LVP_y_shift * zoom_change);
            }
            if (e.X > (pictureBox3.Width / 2)) // zoomin out
            {
                LVP_zoom /= zoom_change;
                LVP_x_shift = (int)(LVP_x_shift / zoom_change);
                LVP_y_shift = (int)(LVP_y_shift / zoom_change);
            }
            LayerViewPanel.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int content = Convert.ToInt16(textBox2.Text, 16);
            if (content < 0x0) content = 0x0;
            if (content > 0xF) content = 0xF;
            this.textBox2.Text = String.Format("{0:X}", content);
            this.selected_tile.nibbles[0] = (byte) content;
            TileInfoPanel.Refresh();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int content = Convert.ToInt16(textBox3.Text, 16);
            if (content < 0x0) content = 0x0;
            if (content > 0x6) content = 0x6; // there can only be 6 diff enemies !
            this.textBox3.Text = String.Format("{0:X}", content);
            this.selected_tile.nibbles[1] = (byte)content;
            TileInfoPanel.Refresh();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int content = Convert.ToInt16(textBox4.Text, 16);
            if (content < 0x0) content = 0x0;
            if (content > 0xF) content = 0xF;
            this.textBox4.Text = String.Format("{0:X}", content);
            this.selected_tile.nibbles[2] = (byte)content;
            TileInfoPanel.Refresh();
        }
    }
}

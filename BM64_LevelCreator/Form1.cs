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
using System.Diagnostics;

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

        // MVP = MapViewPanel
        //private int MVP_zoom = 1.0f;
        private int MVP_x_shift = 0;
        private int MVP_y_shift = 0;

        // LVP = LayerViewPanel
        private int LVP_initial_offset = 10;
        private int LVP_section_display_DIM = (int)(Tile.DIM * Section.DIM * 0.25);
        public float LVP_zoom = 0.5f;
        private int LVP_x_shift = 0;
        private int LVP_y_shift = 0;

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
        private void RefreshVisuals()
        {
            MapViewPanel.Refresh();
            LayerViewPanel.Refresh();
            SectionViewPanel.Refresh();
        }


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // init the editor tile images
            Tile.init_images();

            // init the map names dropbox
            this.MapNames_ComboBox.DataSource = new List<string>(GlobalData.collision_files.Keys);
            // and load the first map from it for something to draw
            String filename = GlobalData.collision_files[this.MapNames_ComboBox.Text];
            String filepath = GlobalData.BM64_CollisionDir + filename;
            current_map.load_CollisionBIN(filepath);

            // init the texture stuff
            Tile.read_tex_config_file();

            // put an initial value into this Box
            this.textBox1.Text = String.Format("{0}", current_map.layers[0].dz);
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

            XYPosition.Text = $"{(select_x + selected_section_x * 8).ToString("X")},{(select_y + selected_section_y * 8).ToString("X")}";

            // translating the results
            int sel_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;

            if (e.Button == MouseButtons.Left)
            {
                selected_tile = current_map.layers[selected_layer].sections[sel_section_index].tiles[select_x, select_y].clone();
                TileInfoPanel.Refresh();
                TextureSelectPanel.Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                current_map.layers[selected_layer].sections[sel_section_index].tiles[select_x, select_y] = selected_tile.clone();
                current_map.layers[selected_layer].sections[sel_section_index].create_representation();

                RefreshVisuals();
            }
            System.Console.WriteLine("Selected TileType = {0:X4}", selected_tile.concat_nibbles());
        }

        private void FillSection_Button_Click(object sender, EventArgs e)
        {
            fill_section_with(selected_tile);
        }
        private void NewMap_Button_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(
                "There is no going back !\n" + // message
                "(unless you saved your map somewhere)",
                "Woah there !", // top line
                MessageBoxButtons.OKCancel);
            if (res != DialogResult.OK) return;

            // reset the selected layer and section or it might crash when loading a smaller map
            current_map.reset_map();
            change_selected_layer(0);
            RefreshVisuals();
        }
        public void fill_section_with(Tile t)
        {
            // translating the results
            int sel_section_index = (current_map.layers[selected_layer].x_extent * selected_section_y) + selected_section_x;

            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    current_map.layers[selected_layer].sections[sel_section_index].tiles[x, y] = t.clone();
                }
            }
            current_map.layers[selected_layer].sections[sel_section_index].create_representation();
            RefreshVisuals();
        }


        private void SectionViewPanel_MouseHover(object sender, EventArgs e)
        {
        }
        private void TileSelectPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int select_x = (int)(e.X / Tile.DIM);
            int select_y = (int)(e.Y / Tile.DIM);

            XYPosition.Text = $"{(select_x + selected_section_x * 8).ToString("X")},{(select_y + selected_section_y * 8).ToString("X")}";

            // some OOB checks
            if (select_x >= 2) return;
            if (select_y >= 8) return;

            // translating the results (the axes are reversed here. Looks cleaner I guess)
            int sel_tile_index = (select_x * 8) + select_y;

            selected_tile = new Tile(sel_tile_index);
            // for now, we use a standard format of 0x001X with X=CollisionID
            selected_tile.nibbles[0] = (byte) 0x0;
            selected_tile.nibbles[1] = (byte) 0x0;
            selected_tile.nibbles[2] = (byte) 0x1;
            selected_tile.nibbles[3] = (byte) sel_tile_index;

            TileInfoPanel.Refresh();
            TextureSelectPanel.Refresh();
            System.Console.WriteLine("Selected TileType = {0:X4}", selected_tile.concat_nibbles());
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
        private void TextureSelectPanel_Paint(object sender, PaintEventArgs e)
        {
            // Creating a Graphics Object when the "Paint" thing in the Form is called
            Graphics g = e.Graphics;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle loc = new Rectangle(0, 0, (2 * Tile.DIM), (2 * Tile.DIM));

            int coll_ID = selected_tile.get_coll_ID();
            if (coll_ID != 0x0)
            {
                int tex_ID = Tile.get_tex_ID_from_coll_ID(coll_ID);
                g.DrawImage(Tile.tex_images[tex_ID], loc);
            }
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
            this.EnemyID_TextBox.Text = String.Format("{0:X}", selected_tile.nibbles[1]);
            this.ObjectID_TextBox.Text = String.Format("{0:X}", selected_tile.get_obj_ID());
        }
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
        public void change_selected_layer(int index)
        {
            this.selected_layer = index;
            // in case the new layer has a diff x/y_extent, we need this extra check (Test Map 1 does that)
            if (this.selected_section_x >= current_map.layers[selected_layer].x_extent)
                this.selected_section_x = current_map.layers[selected_layer].x_extent - 1;
            if (this.selected_section_y >= current_map.layers[selected_layer].y_extent)
                this.selected_section_y = current_map.layers[selected_layer].y_extent - 1;
            // update all the components that care about this value
            LayerID_NumUpDown.Value = index;
            LayerID_NumUpDown.Refresh();
            textBox1.Text = String.Format("{0}", current_map.layers[index].dz);
            textBox1.Refresh();

            RefreshVisuals();
        }
        private void LayerViewPanel_Paint(object sender, PaintEventArgs e)
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
        private void LayerViewPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // cutting off the offsets
            int relative_x = (e.X - LVP_initial_offset) - (int)(LVP_section_display_DIM * LVP_zoom * LVP_x_shift);
            int relative_y = (e.Y - LVP_initial_offset) - (int)(LVP_section_display_DIM * LVP_zoom * LVP_y_shift);
            if (relative_x < 0 || relative_y < 0) return;

            int select_x = (int)(relative_x / (int)(LVP_section_display_DIM * LVP_zoom));
            int select_y = (int)(relative_y / (int)(LVP_section_display_DIM * LVP_zoom));

            // some OOB checks
            if (select_x >= current_map.layers[selected_layer].x_extent) return;
            if (select_y >= current_map.layers[selected_layer].y_extent) return;

            // translating the results
            this.selected_section_x = select_x;
            this.selected_section_y = select_y;

            System.Console.WriteLine("Selected Section ({0}|{1})", selected_section_x, selected_section_y);
            SectionCoords_TextBox.Text = String.Format("( {0} | {1} )", selected_section_x, selected_section_y);
            SectionCoords_TextBox.Update();

            SectionViewPanel.Refresh();
            LayerViewPanel.Refresh();
        }

        private void LayerID_NumUpDown_ValueChanged(object sender, EventArgs e)
        {
            // update the selected layer correctly
            if (LayerID_NumUpDown.Value < 0) LayerID_NumUpDown.Value = 0;
            if (LayerID_NumUpDown.Value >= current_map.layer_cnt) LayerID_NumUpDown.Value = (current_map.layer_cnt - 1);
            change_selected_layer((int)LayerID_NumUpDown.Value);
        }

        private void SaveMapFile_Button_Click(object sender, EventArgs e)
        {
            this.MVP_x_shift++;
            MapViewPanel.Refresh();
        }
        private void RipFiles_Button_Click(object sender, EventArgs e)
        {
            this.MVP_x_shift--;
            MapViewPanel.Refresh();
        }

        private void MapView_DPad_PicBox_MouseClick(object sender, MouseEventArgs e)
        {
            // translate click coordinates into polar coordinates around the img center
            float translated_x = e.X - (MapView_DPad_PicBox.Width / 2);
            float translated_y = e.Y - (MapView_DPad_PicBox.Height / 2);
            double radius = Math.Sqrt((translated_x * translated_x) + (translated_y * translated_y));
            // lil radius check to exclude the corners a bit
            if (radius > MapView_DPad_PicBox.Width / 2) return;
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

        private void LayerView_DPad_PicBox_MouseClick(object sender, MouseEventArgs e)
        {
            // translate click coordinates into polar coordinates around the img center
            float translated_x = e.X - (LayerView_DPad_PicBox.Width / 2);
            float translated_y = e.Y - (LayerView_DPad_PicBox.Height / 2);
            double radius = Math.Sqrt((translated_x * translated_x) + (translated_y * translated_y));
            // lil radius check to exclude the corners a bit
            if (radius > LayerView_DPad_PicBox.Width / 2) return;
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
        private void LayerView_Zoom_PicBox_MouseClick(object sender, MouseEventArgs e)
        {
            float old_zoom_lvl = LVP_zoom;
            float zoom_change = 1.5f;
            if (e.X < (LayerView_Zoom_PicBox.Width / 2)) // zoomin in
            {
                LVP_zoom *= zoom_change;
                LVP_x_shift = (int)(LVP_x_shift * zoom_change);
                LVP_y_shift = (int)(LVP_y_shift * zoom_change);
            }
            if (e.X > (LayerView_Zoom_PicBox.Width / 2)) // zoomin out
            {
                LVP_zoom /= zoom_change;
                LVP_x_shift = (int)(LVP_x_shift / zoom_change);
                LVP_y_shift = (int)(LVP_y_shift / zoom_change);
            }
            LayerViewPanel.Refresh();
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
        private void EnemyID_TextBox_TextChanged(object sender, EventArgs e)
        {
            int content = Convert.ToInt16(EnemyID_TextBox.Text, 16);
            if (content < 0x0) content = 0x0;
            if (content > 0x6) content = 0x6; // there can only be 6 diff enemies !
            this.EnemyID_TextBox.Text = String.Format("{0:X}", content);
            this.selected_tile.nibbles[1] = (byte)content;
            TileInfoPanel.Refresh();
        }
        private void ObjectID_TextBox_TextChanged(object sender, EventArgs e)
        {
            int content = Convert.ToInt16(ObjectID_TextBox.Text, 16);
            if (content < 0x0) content = 0x0;
            if (content > 0xF) content = 0xF;
            this.ObjectID_TextBox.Text = String.Format("{0:X}", content);
            this.selected_tile.nibbles[2] = (byte)content;
            TileInfoPanel.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int old_dz = this.current_map.layers[selected_layer].dz;
            int new_dz = Convert.ToInt32(textBox1.Text, 10);
            this.current_map.layers[selected_layer].dz = new_dz;

            // the change in dz = ddz
            int ddz = (new_dz - old_dz);

            // adjust the hight of every layer ABOVE the affected one
            for (int i = (selected_layer + 1); i < this.current_map.layer_cnt; i++)
            {
                this.current_map.layers[i].z += ddz;
            }
            // adjust the total map hight
            this.current_map.hight += ddz;

            MapViewPanel.Refresh();
        }

        private void LoadMapFile_Button_Click(object sender, EventArgs e)
        {
            // create the correct filepath (might want to check if it exists)
            String filename = GlobalData.collision_files[this.MapNames_ComboBox.Text];
            String filepath = GlobalData.BM64_CollisionDir + filename;
            // and load it
            current_map.load_CollisionBIN(filepath);
            change_selected_layer(0);
        }
        private void SaveMapFile_Button_Click_1(object sender, EventArgs e)
        {
            // create the correct filepath (might want to check if it exists)
            String filename = GlobalData.collision_files[this.MapNames_ComboBox.Text];
            String filepath = GlobalData.BM64_CollisionDir + filename;
            // and save it
            current_map.dump_CollisionBIN(filepath);
        }
        private void RipFiles_Button_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.InitialDirectory = "../../";
            OFD.Filter = "*.z64|*.z64";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                RipFiles(OFD.FileName);
                System.Console.WriteLine("File-Ripping concluded");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int coll_ID = selected_tile.get_coll_ID();
            if (coll_ID != 0x0)
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = "../../assets/textures/";
                OFD.Filter = "*.png|*.png";
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    int tex_ID = Tile.get_tex_ID_from_coll_ID(coll_ID);
                    Tile.change_texture(tex_ID, OFD.FileName);
                    System.Console.WriteLine("Changed Texture");
                }
                TextureSelectPanel.Refresh();
                Tile.dump_tex_config_file();
            }
        }
        private void BuildROM_Button_Click(object sender, EventArgs e)
        {
            //OpenFileDialog OFD = new OpenFileDialog();
            //OFD.InitialDirectory = "../../";
            //OFD.Filter = "*.z64|*.z64";
            //if (OFD.ShowDialog() == DialogResult.OK)
            {
                BuildFiles();
                System.Console.WriteLine("ROM-building concluded");
            }
        }
        private void Export2OBJ_Button_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.InitialDirectory = "../../";
            SFD.Filter = "*.obj|*.obj";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                current_map.dump_OBJ(SFD.FileName);
                System.Console.WriteLine("File sucessfully exportet to .OBJ");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            current_map.dump_ASM();
        }

        private void RipFiles(string RomPath)
        {
            string DestDir = GlobalData.BM64_DataDir;

            // remove the old dump
            if (Directory.Exists(DestDir)) Directory.Delete(DestDir, true);

            ProcessStartInfo SI = new ProcessStartInfo();
            SI.CreateNoWindow = true;
            SI.UseShellExecute = false;
            SI.FileName = GlobalData.RipperPath;
            SI.WindowStyle = ProcessWindowStyle.Hidden;
            SI.Arguments = "\"" + RomPath + "\"";

            try
            {
                using (Process exeProcess = Process.Start(SI))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                Console.WriteLine("Couldn't rip files!");
            }
        }
        private void BuildFiles()
        {
            string DestDir = GlobalData.BM64_DataDir;

            ProcessStartInfo SI = new ProcessStartInfo();
            SI.CreateNoWindow = true;
            SI.UseShellExecute = true;
            SI.FileName = GlobalData.RipperPath;
            SI.WindowStyle = ProcessWindowStyle.Hidden;
            SI.Arguments = String.Format("\"{0}\" {1}", DestDir, "../../../Modified_BM64.z64");
            Console.WriteLine(SI.Arguments);
            try
            {
                using (Process exeProcess = Process.Start(SI))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                Console.WriteLine("Couldn't rip files!");
            }

            // and finally, some padding to get to 8.192 KB filesize
            System.IO.FileStream rom_file = File.Open("../../../Modified_BM64.z64", FileMode.Append);
            while (rom_file.Length < GlobalData.ROM_SIZE)
                rom_file.WriteByte(0xFF);
            rom_file.Close();
        }

        private void ExpandX_Button_Click(object sender, EventArgs e)
        {
            this.current_map.layers[selected_layer].expand_x_once();
            RefreshVisuals();
        }

        private void ExpandY_Button_Click(object sender, EventArgs e)
        {
            this.current_map.layers[selected_layer].expand_y_once();
            RefreshVisuals();
        }

        private void LayerView_DPad_PicBox_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            current_map.add_layer(1);
            change_selected_layer(current_map.layer_cnt - 1);
        }
    }
}

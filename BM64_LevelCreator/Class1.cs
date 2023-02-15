using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BM64_LevelCreator
{
    public class Tile
    {
        // A Tile is 10x10 units wide
        public static int DIM = 32;
        public static List<Image> std_images = new List<Image>();
        public static List<Image> obj_images = new List<Image>();
        public static List<string> tex_image_paths = new List<string>();
        public static List<Image> tex_images = new List<Image>();

        public byte[] nibbles = new byte[4];

        public enum Nibble
        {
            ACTION_ID,
            UNK_2,
            OBJECT_ID,
            COLLISION
        }

        public Tile(int tiletype)
        {
            this.nibbles[0] = (byte) (tiletype / 0x1000);
            this.nibbles[1] = (byte) ((tiletype / 0x100) % 0x10);
            this.nibbles[2] = (byte) ((tiletype / 0x10) % 0x10);
            this.nibbles[3] = (byte) (tiletype % 0x10);
        }

        public static IDictionary<int, string> CollisionID = new Dictionary<int, string>()
        {
            { 0x0, "AIR" },
            { 0x1, "FLOOR" },
            { 0x2, "WARP"},
            { 0x3, "RAMP_D" },
            { 0x4, "RAMP_U" },
            { 0x5, "RAMP_R" },
            { 0x6, "RAMP_L" },
            { 0x7, "WALL_C_UL" },
            { 0x8, "WALL_C_DL" },
            { 0x9, "KillZone" },
            { 0xA, "EFFECT_1" },
            { 0xB, "EFFECT_2" },
            { 0xC, "WALL_C_UR" },
            { 0xD, "WALL_C_DR" },
            { 0xE, "UNK_E" },
            { 0xF, "WALL" },
        };
        public static void init_images() // now sorted by the actual nibble value of the coll ID
        {
            Size imgSize = new Size(Tile.DIM, Tile.DIM);
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Air.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Floor.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Warp.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/RampD.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/RampU.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/RampR.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/RampL.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/WallCorner_UL.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/WallCorner_DL.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/KillZone.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Effect_1.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Effect_2.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/WallCorner_UR.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/WallCorner_DR.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/UNK_0xE.png"), imgSize));
            std_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Wall.png"), imgSize));

            obj_images.Add(new Bitmap(Bitmap.FromFile("../../assets/images/Object.png"), imgSize));

            tex_image_paths.Add("BM64_LevelCreator/assets/images/Floor.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/Warp.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/RampD.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/KillZone.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/Effect_1.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/Effect_2.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/UNK_0xE.png");
            tex_image_paths.Add("BM64_LevelCreator/assets/images/Wall.png");
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[0]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[1]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[2]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[3]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[4]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[5]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[6]), imgSize));
            tex_images.Add(new Bitmap(Bitmap.FromFile("../../../" + Tile.tex_image_paths[7]), imgSize));
        }

        public static void change_texture(int ID, string new_path)
        {
            Size imgSize = new Size(Tile.DIM, Tile.DIM);
            Tile.tex_image_paths[ID] = new_path;
            System.Console.WriteLine("Trying to change TexPath to... " + new_path);
            Tile.tex_images[ID] = new Bitmap(Bitmap.FromFile(new_path), imgSize);
        }
        public static void read_tex_config_file()
        {
            System.Console.WriteLine("Reading TexConfig-FILE...");

            string TexConfig_filename = "../../../USER_TEX.conf";
            if (System.IO.File.Exists(TexConfig_filename) == false) return;

            int count = 0;
            foreach (string line in System.IO.File.ReadLines(TexConfig_filename))
            {
                // ignore the comments that explain what's what
                if (line[0] == '#') continue;
                // for the "default" ones we literally use the default TEX
                if (line.Equals("default"))
                {
                    count++;
                    continue;
                }
                Tile.change_texture(count++, line);
            }
            System.Console.WriteLine("Done.");
        }
        public static void dump_tex_config_file()
        {
            System.Console.WriteLine("Dumping TexConfig-FILE...");
            string TexConfig_filename = "../../../USER_TEX.conf";
            System.IO.StreamWriter CONF_file = new System.IO.StreamWriter(System.IO.File.Open(TexConfig_filename, System.IO.FileMode.Create));
            CONF_file.Flush();

            String[] tex_identifiers =
            {
                "# Floor",
                "# Warp",
                "# Ramps",
                "# KillZone",
                "# Effect_1",
                "# Effect_2",
                "# UNK_0xE",
                "# Wall",
            };

            CONF_file.WriteLine("# This is the user defined TexConfig File (Don't push this up I guess)");
            for (int count = 0; count < 8; count++)
            {
                // write the tex identifier string first
                CONF_file.WriteLine(tex_identifiers[count]);
                // then figure out if we should write "default" or an actual path"
                if (Tile.tex_image_paths[count].Substring(0, 4).Equals("BM64"))
                    CONF_file.WriteLine("default");
                else
                    CONF_file.WriteLine(Tile.tex_image_paths[count]);
            }
            CONF_file.Close();
            System.Console.WriteLine("Done.");
        }

        public static int get_tex_ID_from_coll_ID(int ID)
        {
            switch (ID)
            {
                case (0x1): // Floor
                    return 0;
                case (0x2): // Warp
                    return 1;
                case (0x3): // RampD
                case (0x4): // RampU
                case (0x5): // RampR
                case (0x6): // RampL
                    return 2;
                case (0x9): // KillZone
                    return 3;
                case (0xA): // Effect_1
                    return 4;
                case (0xB): // Effect_2
                    return 5;
                case (0xE): // UNK_0xE
                    return 6;
                case (0x7): // WallC-UL
                case (0x8): // WallC-UR
                case (0xC): // WallC-DL
                case (0xD): // WallC-DR
                case (0xF): // Wall
                    return 7;
                default:
                    return -1; // basically just so C# shuts up
            }
        }

        public int concat_nibbles()
        {
            return (((nibbles[0] * 0x10) + nibbles[1]) * 0x10 + nibbles[2]) * 0x10 + nibbles[3];
        }

        public Tile clone()
        {
            Tile clone = new Tile(this.concat_nibbles());
            return clone;
        }

        public int get_coll_ID() { return this.nibbles[3]; }
        public int get_obj_ID() { return this.nibbles[2]; }

        // gets just the coll_ID from a full tiletype integer (Nibble 4)
        public static int calc_coll_ID(int tiletype)
        {
            return (tiletype % 0x10);
        }
    }

    public class Section
    {
        // A Section consists of 8x8 Tiles and has an (x,y) coord
        public static int DIM = 8;
        public int x, y;
        public Tile[,] tiles;

        public Image rep;

        // Proper Constructor needed, or the Tiles init to NULL...
        public Section(int section_x, int section_y)
        {
            this.x = section_x;
            this.y = section_y;

            this.tiles = new Tile[Section.DIM, Section.DIM];
            for (int x = 0; x < Section.DIM; x++)
            {
                for (int y = 0; y < Section.DIM; y++)
                {
                    this.tiles[x, y] = new Tile(0);
                }
            }
        }

        public void create_representation()
        {
            if (this.rep != null) this.rep.Dispose();
            int size = (Section.DIM * Tile.DIM);
            this.rep = new Bitmap(size, size);

            int scaled_TileDIM = (int)(Tile.DIM * 1.0f);
            Rectangle loc = new Rectangle(0, 0, scaled_TileDIM, scaled_TileDIM);
            // Draw the Section
            using (Graphics g = Graphics.FromImage(rep))
            {
                for (int x = 0; x < Section.DIM; x++)
                {
                    for (int y = 0; y < Section.DIM; y++)
                    {
                        loc.X = (x * scaled_TileDIM);
                        loc.Y = (y * scaled_TileDIM);

                        int coll_id = this.tiles[x, y].get_coll_ID();
                        if (coll_id != 0x0)
                        {
                            g.DrawImage(Tile.std_images[coll_id], loc);

                            int obj_id = this.tiles[x, y].get_obj_ID();
                            if (obj_id != 0x1)
                            {
                                g.DrawImage(Tile.obj_images[0], loc);
                            }
                        }
                    }
                }
            }
        }
    }
    public class Layer
    {
        // A Layer has a z-coord for hight and contains several Sections
        public int index = 0;
        public int z;
        public int dz;
        public int x_extent = 0;
        public int y_extent = 0;
        public List<Section> sections = new List<Section>();
        public int[,] tile_matrix;

        public Layer(int index, int dz, int z)
        {
            this.index = index;
            this.z = z;
            this.dz = dz;
            this.sections.Add(new Section(0, 0));
            this.x_extent = 1;
            this.y_extent = 1;
        }

        public void expand_x_once()
        {
            // add new sections along the y axis (use List.Insert(Index, Entry) to auto-sort it)
            for (int y = 0; y < this.y_extent; y++)
                this.sections.Insert(((y+1)*(x_extent+1)-1), new Section(this.x_extent, y));
            // automatically increment x extent
            this.x_extent++;
        }
        public void expand_y_once()
        {
            // add new sections along the x axis
            for (int x = 0; x < this.x_extent; x++)
                this.sections.Add(new Section(x, this.y_extent));
            // automatically increment y extent
            this.y_extent++;
        }
        public void expand_x(int amount)
        {
            for (int i = 0; i < amount; i++)
                this.expand_x_once();
        }
        public void expand_y(int amount)
        {
            for (int i = 0; i < amount; i++)
                this.expand_y_once();
        }

        public int get_mem_size()
        {
            return ((this.x_extent * this.y_extent) * GlobalData.SECTION_MEMSIZE) + GlobalData.LAYER_HEADSIZE;
        }

        // From all sections within this layer, create one big tilematrix
        public void calc_tile_matrix()
        {
            int x_dim = (this.x_extent * Section.DIM);
            int y_dim = (this.y_extent * Section.DIM);
            this.tile_matrix = new int[x_dim, y_dim];

            foreach (Section section in this.sections)
            {
                for (int x = 0; x < Section.DIM; x++)
                {
                    int x_matrix_index = (section.x * Section.DIM) + x;
                    for (int y = 0; y < Section.DIM; y++)
                    {
                        int y_matrix_index = (section.y * Section.DIM) + y;


                        this.tile_matrix[x_matrix_index, y_matrix_index] = section.tiles[x, y].concat_nibbles();
                    }
                }
            }
        }

        public bool check_LEFT_neighbor_for(int my_x, int my_y, List<int> valid_types)
        {
            // Border Check (AND OOB CHECK)
            if (my_x == 0) return valid_types.Contains(0x0);
            // actual inbounds check
            int neighboring_tile = this.tile_matrix[(my_x - 1), my_y];
            return valid_types.Contains(Tile.calc_coll_ID(neighboring_tile));
        }
        public bool check_RIGHT_neighbor_for(int my_x, int my_y, List<int> valid_types)
        {
            // Border Check (AND OOB CHECK)
            if (my_x == (this.x_extent * Section.DIM - 1))
                return valid_types.Contains(0x0);
            // actual inbounds check
            int neighboring_tile = this.tile_matrix[(my_x + 1), my_y];
            return valid_types.Contains(Tile.calc_coll_ID(neighboring_tile));
        }
        public bool check_NORTH_neighbor_for(int my_x, int my_y, List<int> valid_types)
        {
            // Border Check (AND OOB CHECK)
            if (my_y == 0)return valid_types.Contains(0x0);
            // actual inbounds check
            int neighboring_tile = this.tile_matrix[my_x, (my_y - 1)];
            return valid_types.Contains(Tile.calc_coll_ID(neighboring_tile));
        }
        public bool check_SOUTH_neighbor_for(int my_x, int my_y, List<int> valid_types)
        {
            // Border Check (AND OOB CHECK)
            if (my_y == (this.y_extent * Section.DIM - 1))
                return valid_types.Contains(0x0);
            // actual inbounds check
            int neighboring_tile = this.tile_matrix[my_x, (my_y + 1)];
            return valid_types.Contains(Tile.calc_coll_ID(neighboring_tile));
        }
        // Some Overloads for single type checks... C# is ass sometimes
        public bool check_LEFT_neighbor_for(int my_x, int my_y, int valid_type)
        {
            return check_LEFT_neighbor_for(my_x, my_y, new List<int> { valid_type });
        }
        public bool check_RIGHT_neighbor_for(int my_x, int my_y, int valid_type)
        {
            return check_RIGHT_neighbor_for(my_x, my_y, new List<int> { valid_type });
        }
        public bool check_NORTH_neighbor_for(int my_x, int my_y, int valid_type)
        {
            return check_NORTH_neighbor_for(my_x, my_y, new List<int> { valid_type });
        }
        public bool check_SOUTH_neighbor_for(int my_x, int my_y, int valid_type)
        {
            return check_SOUTH_neighbor_for(my_x, my_y, new List<int> { valid_type });
        }
    }

    public class Map
    {
        public int mem_size = GlobalData.MAP_HEADSIZE;
        public int layer_cnt = 0;
        public byte[] header = new byte[5];
        public int hight = 0;
        public List<Layer> layers = new List<Layer>();

        public void calc_mem_size()
        {
            // start of with the map header size
            this.mem_size = GlobalData.MAP_HEADSIZE;
            // and add up all the layer mem sizes
            foreach (Layer layer in this.layers)
               this.mem_size += layer.get_mem_size();
        }

        public void add_layer(int dz)
        {
            this.layers.Add(new Layer(this.layer_cnt, dz, this.hight));

            this.hight += dz;
            this.layer_cnt++;
        }
        // the greatest y extent from all layers
        public int get_max_y_extent()
        {
            int max = 1;
            foreach (Layer layer in this.layers)
                if (layer.y_extent > max) max = layer.y_extent;

            return max;
        }

        public void clear()
        {
            this.layer_cnt = 0;
            this.hight = 0;
            this.layers = new List<Layer>();
        }

        public Tile get_Tile_At(int layer, Point section, Point coord)
        {
            // get a hold of the correct section first
            Layer chosen_layer = this.layers[layer];
            Section chosen_section = chosen_layer.sections[(chosen_layer.x_extent * section.Y) + section.X];
            // then return the tile
            return chosen_section.tiles[coord.X, coord.Y];
        }

        public int get_deathplane_dz()
        {
            return ((-1) * this.header[4]);
        }
        // input: 4 corners of a quad IN COUNTERCLOCKWISE ORDER
        // output: String that fully defines the quad for OBJ-FILE
        public static string string_tri_from_vtx(string cornerA, string cornerB, string cornerC, string ori, int vtx_cnt)
        {
            string tri = "";
            // add in the xyz vertices
            tri += String.Format("{0}\n{1}\n{2}\n", cornerA, cornerB, cornerC);
            switch(ori)
            {  
                // careful, in UV Y+ is UP !!!
                case ("WALL_C_UL"):
                    // vtx_BL, vtx_BR, vtx_TR
                    tri += String.Format("vt {0} {1}\n", 0, 0);
                    tri += String.Format("vt {0} {1}\n", 1, 0);
                    tri += String.Format("vt {0} {1}\n", 1, 1);
                    break;
                case ("WALL_C_DL"):
                    // vtx_BR, vtx_TR, vtx_TL
                    tri += String.Format("vt {0} {1}\n", 1, 0);
                    tri += String.Format("vt {0} {1}\n", 1, 1);
                    tri += String.Format("vt {0} {1}\n", 0, 1);
                    break;
                case ("WALL_C_UR"):
                    // vtx_BR, vtx_BL, vtx_TL
                    tri += String.Format("vt {0} {1}\n", 1, 0);
                    tri += String.Format("vt {0} {1}\n", 0, 0);
                    tri += String.Format("vt {0} {1}\n", 0, 1);
                    break;
                case ("WALL_C_DR"):
                    // vtx_TR, vtx_BL, vtx_TL
                    tri += String.Format("vt {0} {1}\n", 1, 1);
                    tri += String.Format("vt {0} {1}\n", 0, 0);
                    tri += String.Format("vt {0} {1}\n", 0, 1);
                    break;
            }
            // add in the tri, built from v/vt vertices
            tri += String.Format("f {0}/{1} {2}/{3} {4}/{5}",
                vtx_cnt + 1, vtx_cnt + 1,
                vtx_cnt + 2, vtx_cnt + 2,
                vtx_cnt + 3, vtx_cnt + 3);
            // and return the tri
            return tri;
        }
        // input: 3 corners of a tri IN COUNTERCLOCKWISE ORDER
        // output: String that fully defines the tri for OBJ-FILE
        public static string string_quad_from_vtx(string cornerA, string cornerB, string cornerC, string cornerD, int vtx_cnt, float z_stretch)
        {
            String quad = "";
            // add in the xyz vertices
            quad += String.Format("{0}\n{1}\n{2}\n{3}\n", cornerA, cornerB, cornerC, cornerD);
            // add in the uv vertices
            quad += String.Format("vt {0} {1}\n", 0, z_stretch);
            quad += String.Format("vt {0} {1}\n", 0, 0);
            quad += String.Format("vt {0} {1}\n", 1, 0);
            quad += String.Format("vt {0} {1}\n", 1, z_stretch);
            // add in the quad, built from v/vt vertices
            quad += String.Format("f {0}/{1} {2}/{3} {4}/{5} {6}/{7}",
                vtx_cnt + 1, vtx_cnt + 1, vtx_cnt + 2, vtx_cnt + 2,
                vtx_cnt + 3, vtx_cnt + 3, vtx_cnt + 4, vtx_cnt + 4);
            // and return the quad
            return quad;
        }

        public void load_from_File(string filename)
        {
            System.Console.WriteLine("Clearing currently loaded Map...");
            this.clear();

            System.Console.WriteLine("Reading File...");
            byte[] input = System.IO.File.ReadAllBytes(filename);

            // copy the header over
            for (int i = 0; i < 5; i++)
                this.header[i] = input[i];

            // first of all, calculate where the layers start (this varies a bit, so we need to do this first)
            int layer_cnt = input[0];
            int[] layer_offsets = new int[layer_cnt];
            layer_offsets[0] = GlobalData.MAP_HEADSIZE;
            for (int i = 1; i < layer_cnt; i++)
            {
                // gather information on preceeding section
                int x_extent = input[layer_offsets[(i - 1)] + 0];
                int y_extent = input[layer_offsets[(i - 1)] + 1];
                int section_cnt = (x_extent * y_extent);
                // calc next offset
                layer_offsets[i] = layer_offsets[(i - 1)] + 3 + (section_cnt * GlobalData.SECTION_MEMSIZE);
            }
            // now we can create the layers and expand them to their respective extents
            int cur_total_height = 0;
            for (int i = 0; i < layer_cnt; i++)
            {
                // print some debug info to console
                System.Console.WriteLine("Layer #{0} @ 0x{1:X4}; Has ({2}x{3}) Sections...",
                    i, layer_offsets[i], input[layer_offsets[i] + 0], input[layer_offsets[i] + 1]);

                // gather information on current layer
                int x_extent = input[layer_offsets[i] + 0];
                int y_extent = input[layer_offsets[i] + 1];
                int dz = input[layer_offsets[i] + 2];
                // add the layer and expand it
                this.add_layer(dz);
                this.layers[i].expand_x(x_extent - 1);
                this.layers[i].expand_y(y_extent - 1);

                // fill in the sections from the file input
                int layer_offset = layer_offsets[i] + GlobalData.LAYER_HEADSIZE;
                // iterate over the sections on both X and Y axes
                for (int sec_y = 0; sec_y < this.layers[i].y_extent; sec_y++)
                {
                    for (int sec_x = 0; sec_x < this.layers[i].x_extent; sec_x++)
                    {
                        int section_offset = ((sec_y * this.layers[i].x_extent) + sec_x) * GlobalData.SECTION_MEMSIZE;
                        section_offset += GlobalData.SECTION_HEADSIZE; 

                        // iterate over the tiles on both X and Y axes
                        for (int y = 0; y < Section.DIM; y++)
                        {
                            for (int x = 0; x < Section.DIM; x++)
                            {
                                // combine all the offsets into one final offset
                                int tile_offset = ((y * Section.DIM) + x) * GlobalData.TILE_MEMSIZE;
                                int full_offset = layer_offset + section_offset + tile_offset;

                                // grab a Handle to the current Tile (Fake Pointer..)
                                Tile current_tile = this.get_Tile_At(i, new Point(sec_x, sec_y), new Point(x, y));

                                // read both tile bytes
                                byte tileB_1 = input[full_offset + 0];
                                byte tileB_2 = input[full_offset + 1];

                                // and create the nibbles from them
                                current_tile.nibbles[0] = (byte)(tileB_1 / 0x10);
                                current_tile.nibbles[1] = (byte)(tileB_1 % 0x10);
                                current_tile.nibbles[2] = (byte)(tileB_2 / 0x10);
                                current_tile.nibbles[3] = (byte)(tileB_2 % 0x10);

                                if (current_tile.get_coll_ID() == 0xE)
                                {
                                    System.Console.WriteLine("Unidentified Tile: {0:X4} In Sec.({1}|{2})", current_tile.concat_nibbles(), sec_x, sec_y);
                                }
                            }
                        }
                        // section is now fully loaded, can create its visual rep now
                        this.layers[i].sections[(sec_y * this.layers[i].x_extent) + sec_x].create_representation();
                    }
                }
            }
        }
        public void write_to_File(string filename)
        {
            System.Console.WriteLine("Translating Map into Output Bytes...");
            this.calc_mem_size();
            byte[] output = new byte[this.mem_size];

            // copy the header over
            for (int i = 0; i < 5; i++)
                output[i] = this.header[i];

            int offset = 0;
            // input the map header
            output[offset + 0] = (byte) this.layer_cnt;
            offset += GlobalData.MAP_HEADSIZE;
            // and iterate over all the layers
            foreach (Layer layer in this.layers)
            {
                // input the layer header
                output[offset + 0] = (byte) layer.x_extent;
                output[offset + 1] = (byte) layer.y_extent;
                output[offset + 2] = (byte) layer.dz;
                offset += GlobalData.LAYER_HEADSIZE;
                // and iterate over all the sections
                foreach (Section section in layer.sections)
                {
                    // input section header
                    output[offset + 0] = (byte) section.x;
                    output[offset + 1] = (byte) section.y;
                    offset += GlobalData.SECTION_HEADSIZE;
                    // and iterate over all the tiles
                    for (int y = 0; y < Section.DIM; y++)
                    {
                        for (int x = 0; x < Section.DIM; x++)
                        {
                            // input tile data
                            Tile current_tile = this.get_Tile_At(layer.index, new Point(section.x, section.y), new Point(x, y));
                            output[offset + 0] = (byte) ((current_tile.nibbles[0] * 0x10) + current_tile.nibbles[1]);
                            output[offset + 1] = (byte) ((current_tile.nibbles[2] * 0x10) + current_tile.nibbles[3]);
                            offset += GlobalData.TILE_MEMSIZE;
                        }
                    }
                }
            }

            // and finally transfer the input
            System.IO.BinaryWriter target = new System.IO.BinaryWriter(System.IO.File.Open(filename, System.IO.FileMode.Create));
            target.Flush();
            target.Write(output);
            target.Close();
            System.Console.WriteLine("Finished Writing to {0}", filename);
        }
        public void dump_as_mtl(string MTL_filename)
        {
            System.Console.WriteLine("Generating MTL-FILE...");
            System.IO.StreamWriter MTL_file = new System.IO.StreamWriter(System.IO.File.Open(MTL_filename, System.IO.FileMode.Create));
            MTL_file.Flush();
            // for every tiletype (except AIR) we create a new material and write it to the MTL_file
            for (int i = 0x1; i <= 0xF; i++)
            {
                MTL_file.WriteLine(String.Format("newmtl {0}", Tile.CollisionID[i]));
                MTL_file.WriteLine("Ka 1.000000 1.000000 1.000000"); // Illumination (1.00 is full brightness)
                MTL_file.WriteLine("Kd 0.640000 0.640000 0.640000"); // Diffuse Reflectivity (0.64 is some standard)
                MTL_file.WriteLine("Ks 0.500000 0.500000 0.500000"); // Specular Reflectivity (0.50 is some standard)
                MTL_file.WriteLine("illum 2"); // Illumination Mode (2 is "Highlight on")
                MTL_file.WriteLine("Ns 96.078431"); // Specular Exponent for Specular Highlights (Copied val from Blender...)
                MTL_file.WriteLine("Ni 1.000000"); // Optical Density (1.00 = No Effect)
                MTL_file.WriteLine(String.Format("map_Kd {0}", Tile.tex_image_paths[Tile.get_tex_ID_from_coll_ID(i)]));
            }
            // and dont FORGET TO CLOSE IT
            MTL_file.Close();
        }
        public void dump_as_obj(string OBJ_filename)
        {
            System.Console.WriteLine("Translating Map into OBJ-FILE...");
            System.IO.StreamWriter OBJ_file = new System.IO.StreamWriter(System.IO.File.Open(OBJ_filename, System.IO.FileMode.Create));
            OBJ_file.Flush();

            // first off, we create the MTL FILE to define the textures
            string MTL_filename = OBJ_filename.Substring(0, (OBJ_filename.Length - 4)) + ".mtl";
            this.dump_as_mtl(MTL_filename);
            // then we include the MTL FILE into our OBJ FILE
            OBJ_file.WriteLine(String.Format("mtllib {0}", MTL_filename));
            OBJ_file.WriteLine("s off"); // just always

            // this makes comparisons MUCH more readable
            List<int> FloorTiles = new List<int> { 0x1, 0x2, 0x9, 0xA, 0xB };
            List<int> UnobstructedTiles = new List<int> { 0x0, 0x1, 0x2, 0x9, 0xA, 0xB };
            // init vertex count (need to keep track)
            int vtx_cnt = 0;
            // also declaring these here because C# is getting an
            // Aneurysm if I do it inside the switch cases.....
            int ramp_x, ramp_y;
            float ramp_len, ramp_slope;

            // first of all, I create a Fake Layer(-1)
            Layer fake_layer = new Layer(-1, 0, this.get_deathplane_dz());
            this.layers[0].calc_tile_matrix();
            fake_layer.tile_matrix = this.layers[0].tile_matrix;
            fake_layer.x_extent = this.layers[0].x_extent;
            fake_layer.y_extent = this.layers[0].y_extent;
            // then I simplify this layer into only AIR and WALL
            for (int y = 0; y < (Section.DIM * this.layers[0].y_extent); y++)
            {
                for (int x = 0; x < (Section.DIM * this.layers[0].x_extent); x++)
                {
                    if (Tile.calc_coll_ID(fake_layer.tile_matrix[x, y]) == 0x0)
                        fake_layer.tile_matrix[x, y] = 0x0; // AIR

                    else fake_layer.tile_matrix[x, y] = 0xF; // WALL
                }
            }
            // then I treat it as if it was a normal layer, but since L(-1) only has AIR and WALL
            // the switch statement boils down to if(WALL)...
            for (int y = 0; y < (Section.DIM * fake_layer.y_extent); y++)
            {
                for (int x = 0; x < (Section.DIM * fake_layer.x_extent); x++)
                {
                    if (Tile.calc_coll_ID(fake_layer.tile_matrix[x, y]) == 0x0) // AIR
                        continue;

                    // create a Vertex String for every possible corner of the tile
                    String vtx_TL = String.Format("v {0} {1} {2}", (x + 0), fake_layer.z, (y + 0));
                    String vtx_TR = String.Format("v {0} {1} {2}", (x + 1), fake_layer.z, (y + 0));
                    String vtx_BL = String.Format("v {0} {1} {2}", (x + 0), fake_layer.z, (y + 1));
                    String vtx_BR = String.Format("v {0} {1} {2}", (x + 1), fake_layer.z, (y + 1));
                    String vtx_TL_up = String.Format("v {0} {1} {2}", (x + 0), 0, (y + 0));
                    String vtx_TR_up = String.Format("v {0} {1} {2}", (x + 1), 0, (y + 0));
                    String vtx_BL_up = String.Format("v {0} {1} {2}", (x + 0), 0, (y + 1));
                    String vtx_BR_up = String.Format("v {0} {1} {2}", (x + 1), 0, (y + 1));

                    // Draw Wall IFF a floor or air tile is adjecent
                    if (fake_layer.check_LEFT_neighbor_for(x, y, UnobstructedTiles))
                    {
                        // add the quad that defines a LEFT wall
                        OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF]));
                        OBJ_file.WriteLine(string_quad_from_vtx(vtx_TL_up, vtx_TL, vtx_BL, vtx_BL_up, vtx_cnt, -this.get_deathplane_dz()));
                        vtx_cnt += 4;
                    }
                    if (fake_layer.check_RIGHT_neighbor_for(x, y, UnobstructedTiles))
                    {
                        // add the quad that defines a RIGHT wall
                        OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF]));
                        OBJ_file.WriteLine(string_quad_from_vtx(vtx_BR_up, vtx_BR, vtx_TR, vtx_TR_up, vtx_cnt, -this.get_deathplane_dz()));
                        vtx_cnt += 4;
                    }
                    if (fake_layer.check_NORTH_neighbor_for(x, y, UnobstructedTiles))
                    {
                        // add the quad that defines a LEFT wall
                        OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF]));
                        OBJ_file.WriteLine(string_quad_from_vtx(vtx_TR_up, vtx_TR, vtx_TL, vtx_TL_up, vtx_cnt, -this.get_deathplane_dz()));
                        vtx_cnt += 4;
                    }
                    if (fake_layer.check_SOUTH_neighbor_for(x, y, UnobstructedTiles))
                    {
                        // add the quad that defines a LEFT wall
                        OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF]));
                        OBJ_file.WriteLine(string_quad_from_vtx(vtx_BL_up, vtx_BL, vtx_BR, vtx_BR_up, vtx_cnt, -this.get_deathplane_dz()));
                        vtx_cnt += 4;
                    }
                }
            }

            // then iterate over all the real layers
            foreach (Layer layer in this.layers)
            {
                // set up their tile matrices again (in case they weren't setup yet AND to restore the matrix of layer-0)
                layer.calc_tile_matrix();
                System.Console.WriteLine(layer.dz);
                // then iterate over all the tiles
                for (int y = 0; y < (Section.DIM * layer.y_extent); y++)
                {
                    for (int x = 0; x < (Section.DIM * layer.x_extent); x++)
                    {
                        // create a Vertex String for every possible corner of the tile
                        String vtx_TL = String.Format("v {0} {1} {2}", (x + 0), layer.z, (y + 0));
                        String vtx_TR = String.Format("v {0} {1} {2}", (x + 1), layer.z, (y + 0));
                        String vtx_BL = String.Format("v {0} {1} {2}", (x + 0), layer.z, (y + 1));
                        String vtx_BR = String.Format("v {0} {1} {2}", (x + 1), layer.z, (y + 1));
                        // and for all elevated corners (for walls)
                        int elevated_hight = layer.z;
                        int OBJ_layer_hight = Math.Min(layer.dz, 10); // MAGIC NUMBER (basically determines how high the highest walls are)
                        elevated_hight += OBJ_layer_hight;
                        String vtx_TL_up = String.Format("v {0} {1} {2}", (x + 0), elevated_hight, (y + 0));
                        String vtx_TR_up = String.Format("v {0} {1} {2}", (x + 1), elevated_hight, (y + 0));
                        String vtx_BL_up = String.Format("v {0} {1} {2}", (x + 0), elevated_hight, (y + 1));
                        String vtx_BR_up = String.Format("v {0} {1} {2}", (x + 1), elevated_hight, (y + 1));

                        // then we grab the collision ID of the tile
                        int coll_ID = Tile.calc_coll_ID(layer.tile_matrix[x, y]);
                        // and enter a giant switch statement
                        switch (coll_ID)
                        {
                            case (0x1): // Floor
                            case (0x2): // Warp
                            case (0x9): // KillZone
                            case (0xA): // Effect_1
                            case (0xB): // Effect_2
                                // add the quad that defines the tile on the bottom
                                OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                OBJ_file.WriteLine(string_quad_from_vtx(vtx_TL, vtx_BL, vtx_BR, vtx_TR, vtx_cnt, 1));
                                vtx_cnt += 4;
                                break;

                            case (0x3): // D-Ramp
                                // only look at a D-Ramp if we are on the most NORTHERN tile of it
                                // so if NORTH is another D-Ramp, we skip this entirely
                                if (layer.check_NORTH_neighbor_for(x, y, 0x3))
                                    continue;
                                // now we KNOW we are the most NORTHERN tile of this D-Ramp
                                // next, we find the len of this Ramp
                                ramp_len = 1;
                                ramp_y = y;
                                // check how many D-Ramps are SOUTH
                                while (layer.check_SOUTH_neighbor_for(x, ramp_y, 0x3))
                                {
                                    ramp_len++;
                                    ramp_y++;
                                }
                                // now we can calculate the ramp slope
                                ramp_slope = (layer.dz) / ramp_len;

                                // and now draw the ramp-surface for each ramp_len segment
                                for (int i = 0; i < ramp_len; i++)
                                {
                                    float lower_z = layer.z + ((i + 0) * ramp_slope);
                                    float upper_z = layer.z + ((i + 1) * ramp_slope);

                                    String ramp_vtx_TL = String.Format("v {0} {1} {2}", (x + 0), lower_z, (y + i + 0));
                                    String ramp_vtx_TR = String.Format("v {0} {1} {2}", (x + 1), lower_z, (y + i + 0));
                                    String ramp_vtx_BL = String.Format("v {0} {1} {2}", (x + 0), lower_z, (y + i + 1));
                                    String ramp_vtx_BR = String.Format("v {0} {1} {2}", (x + 1), lower_z, (y + i + 1));
                                    String ramp_vtx_TL_up = String.Format("v {0} {1} {2}", (x + 0), upper_z, (y + i + 0));
                                    String ramp_vtx_TR_up = String.Format("v {0} {1} {2}", (x + 1), upper_z, (y + i + 0));
                                    String ramp_vtx_BL_up = String.Format("v {0} {1} {2}", (x + 0), upper_z, (y + i + 1));
                                    String ramp_vtx_BR_up = String.Format("v {0} {1} {2}", (x + 1), upper_z, (y + i + 1));
                                    // I hate this
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(ramp_vtx_BR_up, ramp_vtx_TR, ramp_vtx_TL, ramp_vtx_BL_up, vtx_cnt, 1));
                                    vtx_cnt += 4;
                                }
                                break;
                            case (0x4): // U-Ramp
                                // only look at a U-Ramp if we are on the most SOUTHERN tile of it
                                // so if SOUTH is another U-Ramp, we skip this entirely
                                if (layer.check_SOUTH_neighbor_for(x, y, 0x4))
                                    continue;
                                // now we KNOW we are the most SOUTHERN tile of this U-Ramp
                                // next, we find the len of this Ramp
                                ramp_len = 1;
                                ramp_y = y;
                                // check how many U-Ramps are NORTH
                                while (layer.check_NORTH_neighbor_for(x, ramp_y, 0x4))
                                {
                                    ramp_len++;
                                    ramp_y--;
                                }
                                // now we can calculate the ramp slope
                                ramp_slope = (layer.dz) / ramp_len;

                                // and now draw the ramp-surface for each ramp_len segment
                                for (int i = 0; i < ramp_len; i++)
                                {
                                    float lower_z = layer.z + ((i + 0) * ramp_slope);
                                    float upper_z = layer.z + ((i + 1) * ramp_slope);

                                    String ramp_vtx_TL = String.Format("v {0} {1} {2}", (x + 0), lower_z, (y - i + 0));
                                    String ramp_vtx_TR = String.Format("v {0} {1} {2}", (x + 1), lower_z, (y - i + 0));
                                    String ramp_vtx_BL = String.Format("v {0} {1} {2}", (x + 0), lower_z, (y - i + 1));
                                    String ramp_vtx_BR = String.Format("v {0} {1} {2}", (x + 1), lower_z, (y - i + 1));
                                    String ramp_vtx_TL_up = String.Format("v {0} {1} {2}", (x + 0), upper_z, (y - i + 0));
                                    String ramp_vtx_TR_up = String.Format("v {0} {1} {2}", (x + 1), upper_z, (y - i + 0));
                                    String ramp_vtx_BL_up = String.Format("v {0} {1} {2}", (x + 0), upper_z, (y - i + 1));
                                    String ramp_vtx_BR_up = String.Format("v {0} {1} {2}", (x + 1), upper_z, (y - i + 1));
                                    // I hate this
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(ramp_vtx_TL_up, ramp_vtx_BL, ramp_vtx_BR, ramp_vtx_TR_up, vtx_cnt, 1));
                                    vtx_cnt += 4;
                                }
                                break;
                            case (0x5): // R-Ramp
                                // only look at a R-Ramp if we are on the most LEFT tile of it
                                // so if LEFT is another R-Ramp, we skip this entirely
                                if (layer.check_LEFT_neighbor_for(x, y, 0x5))
                                    continue;
                                // now we KNOW we are the most LEFT tile of this R-Ramp
                                // next, we find the len of this Ramp
                                ramp_len = 1;
                                ramp_x = x;
                                // check how many R-Ramps are RIGHT
                                while (layer.check_RIGHT_neighbor_for(ramp_x, y, 0x5))
                                {
                                    ramp_len++;
                                    ramp_x++;
                                }
                                // now we can calculate the ramp slope
                                ramp_slope = (layer.dz) / ramp_len;

                                // and now draw the ramp-surface for each ramp_len segment
                                for (int i = 0; i < ramp_len; i++)
                                {
                                    float lower_z = layer.z + ((i + 0) * ramp_slope);
                                    float upper_z = layer.z + ((i + 1) * ramp_slope);

                                    String ramp_vtx_TL = String.Format("v {0} {1} {2}", (x + i + 0), lower_z, (y + 0));
                                    String ramp_vtx_TR = String.Format("v {0} {1} {2}", (x + i + 1), lower_z, (y + 0));
                                    String ramp_vtx_BL = String.Format("v {0} {1} {2}", (x + i + 0), lower_z, (y + 1));
                                    String ramp_vtx_BR = String.Format("v {0} {1} {2}", (x + i + 1), lower_z, (y + 1));
                                    String ramp_vtx_TL_up = String.Format("v {0} {1} {2}", (x + i + 0), upper_z, (y + 0));
                                    String ramp_vtx_TR_up = String.Format("v {0} {1} {2}", (x + i + 1), upper_z, (y + 0));
                                    String ramp_vtx_BL_up = String.Format("v {0} {1} {2}", (x + i + 0), upper_z, (y + 1));
                                    String ramp_vtx_BR_up = String.Format("v {0} {1} {2}", (x + i + 1), upper_z, (y + 1));
                                    // I hate this
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(ramp_vtx_TR_up, ramp_vtx_TL, ramp_vtx_BL, ramp_vtx_BR_up, vtx_cnt, 1));
                                    vtx_cnt += 4;
                                }
                                break;
                            case (0x6): // L-Ramp
                                // only look at a L-Ramp if we are on the most RIGHT tile
                                // so if RIGHT is another L-Ramp, we skip this entirely
                                if (layer.check_RIGHT_neighbor_for(x, y, 0x6))
                                    continue;
                                // now we KNOW we are the most RIGHT tile of this L-Ramp
                                // next, we find the len of this Ramp
                                ramp_len = 1;
                                ramp_x = x;
                                // check how many L-Ramps are LEFT
                                while (layer.check_LEFT_neighbor_for(ramp_x, y, 0x6))
                                {
                                    ramp_len++;
                                    ramp_x--;
                                }
                                // now we can calculate the ramp slope
                                ramp_slope = (layer.dz) / ramp_len;

                                // and now draw the ramp-surface for each ramp_len segment
                                for (int i = 0; i < ramp_len; i++)
                                {
                                    float lower_z = layer.z + ((i + 0) * ramp_slope);
                                    float upper_z = layer.z + ((i + 1) * ramp_slope);

                                    String ramp_vtx_TL = String.Format("v {0} {1} {2}", (x - i + 0), lower_z, (y + 0));
                                    String ramp_vtx_TR = String.Format("v {0} {1} {2}", (x - i + 1), lower_z, (y + 0));
                                    String ramp_vtx_BL = String.Format("v {0} {1} {2}", (x - i + 0), lower_z, (y + 1));
                                    String ramp_vtx_BR = String.Format("v {0} {1} {2}", (x - i + 1), lower_z, (y + 1));
                                    String ramp_vtx_TL_up = String.Format("v {0} {1} {2}", (x - i + 0), upper_z, (y + 0));
                                    String ramp_vtx_TR_up = String.Format("v {0} {1} {2}", (x - i + 1), upper_z, (y + 0));
                                    String ramp_vtx_BL_up = String.Format("v {0} {1} {2}", (x - i + 0), upper_z, (y + 1));
                                    String ramp_vtx_BR_up = String.Format("v {0} {1} {2}", (x - i + 1), upper_z, (y + 1));
                                    // I hate this
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(ramp_vtx_BL_up, ramp_vtx_BR, ramp_vtx_TR, ramp_vtx_TL_up, vtx_cnt, 1));
                                    vtx_cnt += 4;
                                }
                                break;


                            case (0x7): // Wall-Corner UL
                                // draw the floor tri IFF a floor tile is to the RIGHT or DOWN
                                if (layer.check_RIGHT_neighbor_for(x, y, FloorTiles) || layer.check_SOUTH_neighbor_for(x, y, FloorTiles))
                                {
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0x1])); // corner floors still use the floor tex
                                    OBJ_file.WriteLine(string_tri_from_vtx(vtx_BL, vtx_BR, vtx_TR, Tile.CollisionID[coll_ID], vtx_cnt));
                                    vtx_cnt += 3;
                                }
                                // add the angled wall
                                OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF])); // corner walls still use the wall tex
                                OBJ_file.WriteLine(string_quad_from_vtx(vtx_TR_up, vtx_TR, vtx_BL, vtx_BL_up, vtx_cnt, OBJ_layer_hight));
                                vtx_cnt += 4;
                                break;
                            case (0x8): // Wall-Corner DL
                                // draw the floor tri IFF a floor tile is to the RIGHT or UP
                                if (layer.check_RIGHT_neighbor_for(x, y, FloorTiles) || layer.check_NORTH_neighbor_for(x, y, FloorTiles))
                                {
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0x1])); // corner floors still use the floor tex
                                    OBJ_file.WriteLine(string_tri_from_vtx(vtx_BR, vtx_TR, vtx_TL, Tile.CollisionID[coll_ID], vtx_cnt));
                                    vtx_cnt += 3;
                                }
                                // add the angled wall
                                OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF])); // corner walls still use the wall tex
                                OBJ_file.WriteLine(string_quad_from_vtx(vtx_BR_up, vtx_BR, vtx_TL, vtx_TL_up, vtx_cnt, OBJ_layer_hight));
                                vtx_cnt += 4;
                                break;


                            case (0xC): // Wall-Corner UR
                                // draw the floor tri IFF a floor tile is to the LEFT or DOWN
                                if (layer.check_LEFT_neighbor_for(x, y, FloorTiles) || layer.check_SOUTH_neighbor_for(x, y, FloorTiles))
                                {
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0x1])); // corner floors still use the floor tex
                                    OBJ_file.WriteLine(string_tri_from_vtx(vtx_BR, vtx_BL, vtx_TL, Tile.CollisionID[coll_ID], vtx_cnt));
                                    vtx_cnt += 3;
                                }
                                // add the angled wall
                                OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF])); // corner walls still use the wall tex
                                OBJ_file.WriteLine(string_quad_from_vtx(vtx_TL_up, vtx_TL, vtx_BR, vtx_BR_up, vtx_cnt, OBJ_layer_hight));
                                vtx_cnt += 4;
                                break;
                            case (0xD): // Wall-Corner DR
                                // draw the floor tri IFF a floor tile is to the LEFT or UP
                                if (layer.check_LEFT_neighbor_for(x, y, FloorTiles) || layer.check_NORTH_neighbor_for(x, y, FloorTiles))
                                {
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0x1])); // corner floors still use the floor tex
                                    OBJ_file.WriteLine(string_tri_from_vtx(vtx_TR, vtx_BL, vtx_TL, Tile.CollisionID[coll_ID], vtx_cnt));
                                    vtx_cnt += 3;
                                }
                                // add the angled wall
                                OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0xF])); // corner walls still use the wall tex
                                OBJ_file.WriteLine(string_quad_from_vtx(vtx_TR_up, vtx_TR, vtx_BL, vtx_BL_up, vtx_cnt, OBJ_layer_hight));
                                vtx_cnt += 4;
                                break;


                            case (0xF): // Wall
                                // Draw Wall IFF a floor or air tile is adjecent (else this wall tile is "inside")
                                if (layer.check_LEFT_neighbor_for(x, y, UnobstructedTiles))
                                {
                                    // add the quad that defines a LEFT wall
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(vtx_TL_up, vtx_TL, vtx_BL, vtx_BL_up, vtx_cnt, OBJ_layer_hight));
                                    vtx_cnt += 4;
                                }
                                if (layer.check_RIGHT_neighbor_for(x, y, UnobstructedTiles))
                                {
                                    // add the quad that defines a RIGHT wall
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(vtx_BR_up, vtx_BR, vtx_TR, vtx_TR_up, vtx_cnt, OBJ_layer_hight));
                                    vtx_cnt += 4;
                                }
                                if (layer.check_NORTH_neighbor_for(x, y, UnobstructedTiles))
                                {
                                    // add the quad that defines a LEFT wall
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(vtx_TR_up, vtx_TR, vtx_TL, vtx_TL_up, vtx_cnt, OBJ_layer_hight));
                                    vtx_cnt += 4;
                                }
                                if (layer.check_SOUTH_neighbor_for(x, y, UnobstructedTiles))
                                {
                                    // add the quad that defines a LEFT wall
                                    OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[coll_ID]));
                                    OBJ_file.WriteLine(string_quad_from_vtx(vtx_BL_up, vtx_BL, vtx_BR, vtx_BR_up, vtx_cnt, OBJ_layer_hight));
                                    vtx_cnt += 4;
                                }
                                // checking if I should draw a CEILING UNDER the wall aswell
                                if (layer.index > 0)
                                {
                                    if (UnobstructedTiles.Contains(this.layers[(layer.index - 1)].tile_matrix[x, y]))
                                    {
                                        System.Console.WriteLine("Ceiling");
                                        // add the quad that defines a CEILING (use Floor texture)
                                        OBJ_file.WriteLine(String.Format("usemtl {0}", Tile.CollisionID[0x1]));
                                        OBJ_file.WriteLine(string_quad_from_vtx(vtx_TL_up, vtx_TR_up, vtx_BR_up, vtx_BL_up, vtx_cnt, 1));
                                        vtx_cnt += 4;
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            // and done
            OBJ_file.Close();
            System.Console.WriteLine("Finished Writing to {0}", OBJ_filename);
        }
    }
}

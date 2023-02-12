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
    }

    public class Section
    {
        // A Section consists of 8x8 Tiles and has an (x,y) coord
        public static int DIM = 8;
        public int x, y;
        public Tile[] tiles;

        public Image rep;

        // Proper Constructor needed, or the Tiles init to NULL...
        public Section(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.tiles = new Tile[DIM * DIM];
            for (int i = 0; i < DIM * DIM; i++)
                this.tiles[i] = new Tile(0);
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
                        int index = x + (y * Section.DIM);
                        loc.X = (x * scaled_TileDIM);
                        loc.Y = (y * scaled_TileDIM);

                        int coll_id = this.tiles[index].get_coll_ID();
                        if (coll_id != 0x0)
                        {
                            g.DrawImage(Tile.std_images[coll_id], loc);

                            int obj_id = this.tiles[index].get_obj_ID();
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
            return chosen_section.tiles[(Section.DIM * coord.Y) + coord.X];
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
                int hight = input[layer_offsets[i] + 2];
                // add the layer and expand it
                this.add_layer(hight);
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

                                if (current_tile.get_coll_ID() == 0x9 || current_tile.get_coll_ID() == 0xE)
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
    }
}

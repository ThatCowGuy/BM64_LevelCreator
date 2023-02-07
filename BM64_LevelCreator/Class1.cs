using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BM64_LevelCreator
{
    class Tile
    {
        // A Tile is 10x10 units wide
        public static int DIM = 32;
        public int type = 0;
    }

    class Section : IComparable<Section>
    {
        // A Section consists of 8x8 Tiles and has an (x,y) coord
        public static int DIM = 8;
        public int x, y;
        public Tile[] tiles;

        // Proper Constructor needed, or the Tiles init to NULL...
        public Section(int x, int y)
        {
            this.x = x;
            this.y = y;

            this.tiles = new Tile[DIM * DIM];
            for (int i = 0; i < DIM * DIM; i++)
                this.tiles[i] = new Tile();
        }
        public int CompareTo(Section comp)
        {
            return 1;
        }
    }
    class Layer
    {
        // A Layer has a z-coord for hight and contains several Sections
        public int index = 0;
        public int z;
        public int x_extent = 0;
        public int y_extent = 0;
        public List<Section> sections = new List<Section>();

        public Layer(int index, int z)
        {
            this.index = index;
            this.z = z;
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
    }

    class Map
    {
        public int layer_cnt = 0;
        public int hight = 0;
        public List<Layer> layers = new List<Layer>();

        public void add_layer(int z_diff)
        {
            this.hight += z_diff;
            this.layers.Add(new Layer(this.layer_cnt, this.hight));

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

        public void put_tile(int layer, Point section, Point coord, int type)
        {
            // get a hold of the correct section first
            Layer chosen_layer = this.layers[layer];
            Section chosen_section = chosen_layer.sections[(chosen_layer.x_extent * section.Y) + section.X];
            // then place the tile there
            chosen_section.tiles[(Section.DIM * coord.Y) + coord.X].type = type;
        }
    }
}

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
        public static int DIM = 30;
        public int type = 0;
    }

    class Section
    {
        // A Section consists of 8x8 Tiles and has an (x,y) coord
        public static int DIM = 8;
        public int x, y;
        public Tile[] tiles;

        // Proper Constructor needed, or the Tiles init to NULL...
        public Section()
        {
            this.tiles = new Tile[DIM * DIM];
            for (int i = 0; i < DIM * DIM; i++)
                this.tiles[i] = new Tile();
        }
    }
    class Layer
    {
        // A Layer has a z-coord for hight and contains several Sections
        public int z;
        public List<Section> sections = new List<Section>();

        public Layer(int z)
        {
            this.z = z;
        }
    }

    class Map
    {
        public List<Layer> layers = new List<Layer>();
    }
}

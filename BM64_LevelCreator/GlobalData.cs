using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM64_LevelCreator
{
    internal class GlobalData
    {
        // some memory size constants
        public static int SECTION_HEADSIZE = 0x2;
        public static int LAYER_HEADSIZE = 0x3;
        public static int MAP_HEADSIZE = 0x5;
        public static int TILE_MEMSIZE = 0x2;
        public static int SECTION_MEMSIZE = SECTION_HEADSIZE + (Section.DIM * Section.DIM) * TILE_MEMSIZE;

        public static int ROM_SIZE = 8388608; // 8.128 KB

        public static int LAST_LAYER_VISUAL_HIGHT = 10;

        // DO NOT CHANGE - this bad boy is just a path to the ripper, I'm sure there's a better way of doing it but I like this way :)
        // Note: System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) == "./"
        public static readonly string RipperPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "deps\\bm64romtool.exe");

        // where the ripped files go to
        public static string BM64_DataDir = "../../../bm64data";

        public static string BM64_CollisionDir = "../../../bm64data/data/";
        // https://github.com/Coockie1173/BomerhackerThree/blob/main/FileList.txt
        // this dict is sorted by internal file order
        /*public static Dictionary<String, String> collision_files = new Dictionary<string, string>()
        {
            // Test Rooms (inaccessable ingame)
            { "Test Room 1 (Actual Map)", "file307.bin" },
            { "Test Room 2 (TR-1 no blocks)", "file309.bin" },
            { "Test Room 3 (Flat Floor)", "file311.bin" },
            { "Test Room 4 (Jumbled Mess)", "file313.bin" },
            // BR-1+3
            { "BR-1 Starting Room", "file320.bin" },
            { "BR-1 Room A (Flower Courtyard)", "file321.bin" },
            { "BR-1 Room A-1 (Canon Island)", "file322.bin" },
            { "BR-1/3 Room A-2 (The Top)", "file323.bin" },
            { "BR-3 Starting Area", "file324.bin" },
            { "BR-3 Exit Room", "file325.bin" },
            // BR-1+3 Low Water Layers (for swap-ins ?)
            { "BR-1 (LowWater)  St Layer0", "file340.bin" },
            { "BR-1 (LowWater)  St Layer1", "file341.bin" },
            { "BR-1 (LowWater)  A-1 Layer0", "file342.bin" },
            { "BR-1 (LowWater)  A-1 Layer1", "file343.bin" },
            { "BR-1 (LowWater)  A-1 Layer2", "file344.bin" },
            { "BR-1 (LowWater)  A-2 Layer2", "file345.bin" },
            { "BR-3 (LowWater)  St Layer0", "file346.bin" },
            { "BR-3 (LowWater)  Ex Layer0", "file347.bin" },
            // BR-2
            { "BR-2 (VS) Artemis", "file368.bin" },
            // BR-4
            { "BR-4 (VS) Leviathan", "file374.bin" },

            // RM-1
            { "RM-1 Starting Room", "file400.bin" },
            { "RM-1 Branch-A Room 1", "file401.bin" },
            { "RM-1 Branch-A Room 2", "file402.bin" },
            { "RM-1 Exit Room", "file403.bin" },
            { "RM-1 Branch-B Room 1", "file404.bin" },
            { "RM-1 Little Room 1", "file405.bin" },
            { "RM-1 Little Room 2", "file406.bin" },
            { "RM-1 Little Room 3", "file407.bin" },
            { "RM-1 Little Room 4", "file408.bin" },
            // RM-3
            { "RM-3 Starting Room", "file421.bin" },
            { "RM-3 North Room 1 (Furnace)", "file422.bin" },
            { "RM-3 North Room 2 (Switches)", "file423.bin" },
            { "RM-3 West Room 1 (Worksite)", "file424.bin" },
            { "RM-3 Exit Room", "file425.bin" },
            // RM-2
            { "RM-2 (VS) Orion", "file427.bin" },
            // RM-4
            { "RM-4 (VS) Hades", "file466.bin" },

            // BF-1
            { "BF1 Starting Room", "file481.bin" },
            { "BF1 Tunnel 1", "file482.bin" },
            { "BF1 Outside Area 1", "file483.bin" },
            { "BF1 Tunnel 2", "file484.bin" },
            { "BF1 Outside Area 2", "file485.bin" },
            { "BF1 Exit Room", "file486.bin" },
            { "BF1 (LowFloor) Ex Layer0", "file487.bin" },
            { "BF1 (LowFloor) Ex Layer1", "file488.bin" },
            // BF-3
            { "BF-3 Starting Room", "file499.bin" },
            { "BF-3 Floor-2", "file500.bin" },
            { "BF-3 (Special ?) Floor-2 Layer4", "file501.bin" },
            { "BF-3 Floor-3 (Puzzle Floor)", "file502.bin" },
            { "BF-3 Floor-3 (Exit Room)", "file503.bin" },
            { "BF-3 Floor-4 (Elevator Tower)", "file504.bin" },
            { "BF-3 Floor-5 (Extra)", "file505.bin" },
            // BF-2
            { "BF-4 (VS) Altair", "file507.bin" },

            // Multiplayer Arenas
            { "Multiplayer - Rock Garden", "file537.bin" },
            { "Multiplayer - Up and Down", "file538.bin" },
            { "Multiplayer - Pyramid", "file539.bin" },
            { "Multiplayer - Greedy Trap", "file540.bin" },
            { "Multiplayer - Top Rules", "file541.bin" },
            { "Multiplayer - Field of Grass", "file542.bin" },
            { "Multiplayer - In the Gutter", "file543.bin" },
            { "Multiplayer - Sea Sick", "file544.bin" },
            { "Multiplayer - Blizzard Battle", "file545.bin" },
            { "Multiplayer - Lost at Sea", "file546.bin" },

            // WG-1
            { "WG-1 Starting Room", "file567.bin" },
            { "WG-1 Westside Slope", "file568.bin" }, // might have confused east and west
            { "WG-1 Exit Room", "file569.bin" },
            { "WG-1 Eastside Slope", "file570.bin" },
            { "WG-1 (DUPE A?) Westside Slope", "file571.bin" },
            { "WG-1 (DUPE B?) Westside Slope", "file572.bin" },
            // WG-3
            { "WG-3 Starting Room", "file573.bin" },
            { "WG-3 Right Path", "file574.bin" },
            { "WG-3 Left Path", "file575.bin" },
            { "WG-3 Slope Room", "file576.bin" },
            // WG-2
            { "WG-2 (VS) Regulus", "file577.bin" },

            // GG-1
            { "GG-1 Starting Room", "file588.bin" },
            { "GG-1 Backside", "file589.bin" },
            { "GG-1 Hidden Room 1", "file590.bin" },
            { "GG-1 Hidden Room 2", "file591.bin" },
            { "GG-1 Hidden Room 3", "file592.bin" },
            // GG-3
            { "GG-3 Starting Room", "file593.bin" },
            { "GG-3 Backside", "file594.bin" },
            { "GG-3 Hidden Room", "file595.bin" },
            { "GG-3 Tower Area", "file596.bin" },
            // GG-2
            { "GG-2 (VS) Sirius", "file597.bin" },

            // ???
            { "??? Broken Test Room 1 ?", "file599.bin" },

            // WG-4
            { "WG-4 (VS) Mantis", "file635.bin" },
            // GG-4
            { "GG-4 (VS) Draco", "file661.bin" },
            // BF-2
            { "BF-2 (VS) Harvester", "file701.bin" },

            // RP-1
            { "RP-1 Starting Room", "file735.bin" },
            { "RP-1 Side Area", "file736.bin" },
            // RP-3
            { "RP-3 Starting Room", "file737.bin" },
            { "RP-3 Outside", "file738.bin" },
            // RP-4
            { "RP-4 (VS) Sirius Phase 1", "file739.bin" }, // no walls around
            { "RP-4 (VS) Sirius Phase 2", "file740.bin" }, // has surrounding walls
            // RP-2
            { "RP-2 (VS) Spellmaker", "file743.bin" },

            // ???
            { "??? Broken Test Room 2 ?", "file825.bin" },
            // ???
            { "??? Broken Test Room 3 ?", "file862.bin" },
        };*/

        // this dict is sorted by ingame progression order
        public static Dictionary<String, String> collision_files = new Dictionary<string, string>()
        {
            //=================================================
            // GG-1
            { "GG-1 Starting Room", "file588.bin" },
            { "GG-1 Backside", "file589.bin" },
            { "GG-1 Hidden Room 1", "file590.bin" },
            { "GG-1 Hidden Room 2", "file591.bin" },
            { "GG-1 Hidden Room 3", "file592.bin" },
            // GG-2
            { "GG-2 (VS) Sirius", "file597.bin" },
            // GG-3
            { "GG-3 Starting Room", "file593.bin" },
            { "GG-3 Backside", "file594.bin" },
            { "GG-3 Hidden Room", "file595.bin" },
            { "GG-3 Tower Area", "file596.bin" },
            // GG-4
            { "GG-4 (VS) Draco", "file661.bin" },
            //=================================================
            // BR-1 - remember, BR-1+3 share these in a weird way
            { "BR-1 Starting Room", "file320.bin" },
            { "BR-1 (LowWater) St Layer0", "file340.bin" },
            { "BR-1 (LowWater) St Layer1", "file341.bin" },
            { "BR-1 Room A (Flower Courtyard)", "file321.bin" },
            { "BR-1 Room A-1 (Canon Island)", "file322.bin" },
            { "BR-1 (LowWater) A-1 Layer0", "file342.bin" },
            { "BR-1 (LowWater) A-1 Layer1", "file343.bin" },
            { "BR-1 (LowWater) A-1 Layer2", "file344.bin" },
            { "BR-1/3 Room A-2 (The Top)", "file323.bin" },
            { "BR-1 (LowWater) A-2 Layer2", "file345.bin" },
            // BR-2
            { "BR-2 (VS) Artemis", "file368.bin" },
            // BR-3
            { "BR-3 Starting Area", "file324.bin" },
            { "BR-3 (LowWater) St Layer0", "file346.bin" },
            { "BR-3 Exit Room", "file325.bin" },
            { "BR-3 (LowWater) Ex Layer0", "file347.bin" },
            // BR-4
            { "BR-4 (VS) Leviathan", "file374.bin" },
            //=================================================
            // RM-1
            { "RM-1 Starting Room", "file400.bin" },
            { "RM-1 Branch-A Room 1", "file401.bin" },
            { "RM-1 Branch-A Room 2", "file402.bin" },
            { "RM-1 Exit Room", "file403.bin" },
            { "RM-1 Branch-B Room 1", "file404.bin" },
            { "RM-1 Little Room 1", "file405.bin" },
            { "RM-1 Little Room 2", "file406.bin" },
            { "RM-1 Little Room 3", "file407.bin" },
            { "RM-1 Little Room 4", "file408.bin" },
            // RM-2
            { "RM-2 (VS) Orion", "file427.bin" },
            // RM-3
            { "RM-3 Starting Room", "file421.bin" },
            { "RM-3 North Room 1 (Furnace)", "file422.bin" },
            { "RM-3 North Room 2 (Switches)", "file423.bin" },
            { "RM-3 West Room 1 (Worksite)", "file424.bin" },
            { "RM-3 Exit Room", "file425.bin" },
            // RM-4
            { "RM-4 (VS) Hades", "file466.bin" },
            //=================================================
            // WG-1
            { "WG-1 Starting Room", "file567.bin" },
            { "WG-1 Westside Slope", "file568.bin" }, // might have confused east and west
            { "WG-1 (DUPE A?) Westside Slope", "file571.bin" },
            { "WG-1 (DUPE B?) Westside Slope", "file572.bin" },
            { "WG-1 Eastside Slope", "file570.bin" },
            { "WG-1 Exit Room", "file569.bin" },
            // WG-2
            { "WG-2 (VS) Regulus", "file577.bin" },
            // WG-3
            { "WG-3 Starting + Exit Room", "file573.bin" },
            { "WG-3 Right Path", "file574.bin" },
            { "WG-3 Left Path", "file575.bin" },
            { "WG-3 Slope Room", "file576.bin" },
            // WG-4
            { "WG-4 (VS) Mantis", "file635.bin" },
            //=================================================
            // BF-1
            { "BF1 Starting Room", "file481.bin" },
            { "BF1 Tunnel 1", "file482.bin" },
            { "BF1 Outside Area 1", "file483.bin" },
            { "BF1 Tunnel 2", "file484.bin" },
            { "BF1 Outside Area 2", "file485.bin" },
            { "BF1 Exit Room", "file486.bin" },
            { "BF1 (LowFloor) Ex Layer0", "file487.bin" },
            { "BF1 (LowFloor) Ex Layer1", "file488.bin" },
            // BF-2
            { "BF-2 (VS) Harvester", "file701.bin" },
            // BF-3
            { "BF-3 Starting Room", "file499.bin" },
            { "BF-3 Floor-2", "file500.bin" },
            { "BF-3 (Special ?) Floor-2 Layer4", "file501.bin" },
            { "BF-3 Floor-3 (Puzzle Floor)", "file502.bin" },
            { "BF-3 Floor-3 (Exit Room)", "file503.bin" },
            { "BF-3 Floor-4 (Elevator Tower)", "file504.bin" },
            { "BF-3 Floor-5 (Extra)", "file505.bin" },
            // BF-4
            { "BF-4 (VS) Altair", "file507.bin" },
            //=================================================
            // RP-1
            { "RP-1 Starting Room", "file735.bin" },
            { "RP-1 Side Area", "file736.bin" },
            // RP-2
            { "RP-2 (VS) Spellmaker", "file743.bin" },
            // RP-3
            { "RP-3 Starting Room", "file737.bin" },
            { "RP-3 Outside", "file738.bin" },
            // RP-4
            { "RP-4 (VS) Sirius Phase 1", "file739.bin" }, // no walls around
            { "RP-4 (VS) Sirius Phase 2", "file740.bin" }, // has surrounding walls
            //=================================================
            // Multiplayer Arenas
            { "Multiplayer - Rock Garden", "file537.bin" },
            { "Multiplayer - Up and Down", "file538.bin" },
            { "Multiplayer - Pyramid", "file539.bin" },
            { "Multiplayer - Greedy Trap", "file540.bin" },
            { "Multiplayer - Top Rules", "file541.bin" },
            { "Multiplayer - Field of Grass", "file542.bin" },
            { "Multiplayer - In the Gutter", "file543.bin" },
            { "Multiplayer - Sea Sick", "file544.bin" },
            { "Multiplayer - Blizzard Battle", "file545.bin" },
            { "Multiplayer - Lost at Sea", "file546.bin" },
            // Test Rooms (inaccessable ingame)
            { "Test Room 1 (Actual Map)", "file307.bin" },
            { "Test Room 2 (TR-1 no blocks)", "file309.bin" },
            { "Test Room 3 (Flat Floor)", "file311.bin" },
            { "Test Room 4 (Jumbled Mess)", "file313.bin" },
            // ???
            { "??? Broken Test Room 1 ?", "file599.bin" },
            { "??? Broken Test Room 2 ?", "file825.bin" },
            { "??? Broken Test Room 3 ?", "file862.bin" },
        };
    }
}

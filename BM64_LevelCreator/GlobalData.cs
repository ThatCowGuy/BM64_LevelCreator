using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM64_LevelCreator
{
    internal class GlobalData
    {
        //make everything here public static, so you can access this stuff from p much everywhere
        //OR const! you can use consts here too! just a general place for the global stuff you might need everywhere
        //if const doesn't work use readonly ;)

        //I'm not sure if we want to store this for the next run, I don't think we want to but you know
        public static string ProjectDir = "";

        //DO NOT CHANGE - this bad boy is just a path to the ripper, I'm sure there's a better way of doing it but I like this way :)
        public static readonly string RipperPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "deps\\bm64romtool.exe");
    }
}

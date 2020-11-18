using System;
namespace LitematicaTR.TileSchem
{
    public class TileSchemeReader
    {
        public int[] origin;
        public int width;
        public int height;
        public int[,] types;
        public short[,,] frames;
        public bool[,] actuated;
    }
}

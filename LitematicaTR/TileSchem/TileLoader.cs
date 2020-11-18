using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using LitematicaTR.Utiles;
using Microsoft.Xna.Framework.Graphics;

namespace LitematicaTR.TileSchem
{
    public class TileScheme
    {
        public int[] origin;
        public int width;
        public int height;
        public int[,] types;
        public short[,,] frames;
        public bool[,] actuated;
        public TileScheme(string jsonFilePath)
        {
            string raw;
            using (StreamReader jsonFile = new StreamReader(jsonFilePath))
            {
                raw = jsonFile.ReadToEnd();
            }
            TileSchemeReader tiles = JsonConvert.DeserializeObject<TileSchemeReader>(raw);
            origin = tiles.origin;
            width = tiles.width;
            height = tiles.height;
            types = tiles.types;
            frames = tiles.frames;
            actuated = tiles.actuated;
        }

        public TileScheme(Tile[,] tiles, int[] tileOrigin, short originLocation)
        {
            origin = new int[2];
            origin[0] = tileOrigin[0];
            origin[1] = tileOrigin[1];
            width = tiles.GetLength(0) - 1;
            height = tiles.GetLength(1) - 1;
            if (originLocation == OriginLocation.topLeft);
            else if (originLocation == OriginLocation.topRight)
            {
                width = -width;
            }
            else if (originLocation == OriginLocation.bottomLeft)
            {
                height = -height;
            }
            else if (originLocation == OriginLocation.bottomRight)
            {
                width = -width;
                height = -height;
            }
            else
            {
                Main.NewText("Unhandled origin location, at TileLoader.cs, line 53");
            }
            types = new int[tiles.GetLength(0),tiles.GetLength(1)];
            frames = new short[tiles.GetLength(0), tiles.GetLength(1), 2];
            actuated = new bool[tiles.GetLength(0), tiles.GetLength(1)];
            for (int w = 0; w < tiles.GetLength(0); ++w)
            {
                for (int h = 0; h < tiles.GetLength(1); ++h)
                {
                    Tile t = tiles[w, h];
                    if (t != null)
                    {
                        if (t.active())
                        {
                            actuated[w,h] = t.inActive();
                            types[w, h] = t.type;
                            frames[w, h, 0] = t.frameX;
                            frames[w, h, 1] = t.frameY;
                        }
                        else
                        {
                            actuated[w, h] = false;
                            types[w, h] = -1;
                            frames[w, h, 0] = -1;
                            frames[w, h, 1] = -1;
                        }
                    }
                    else
                    {
                        actuated[w, h] = false;
                        types[w, h] = -1;
                        frames[w, h, 0] = -1;
                        frames[w, h, 1] = -1;
                    }
                }
            }
        }

        public void writeToFile(string fileName)
        {
            if (Directory.Exists(LitematicaTR.schemePath))
            {
                ///Users/yuyanzuo/Library/Application Support/Terraria/ModLoader/Schems/
                string path = LitematicaTR.schemePath+ "/" + fileName + ".json";
                string result = JsonConvert.SerializeObject(this);
                if (File.Exists(path))
                    File.Delete(path);
                using (StreamWriter targetFile = new StreamWriter(path))
                {
                    Main.NewText("Writting File");
                    targetFile.Write(result);
                }
            }
            else
                Main.NewText("Please enter a valid scheme path in Modconfig.");
        }

        public int[] getOrigin()
        {
            int[] result = new int[2];
            origin.CopyTo(result, 0);
            return origin;
        }

        public int[] getCorner()
        {
            int[] corner = new int[2];
            if (origin[0]<0)
            {
                corner[0] = -1;
                corner[1] = -1;
            }
            else
            {
                corner[0] = origin[0] + width;
                corner[1] = origin[1] + height;
            }
            return corner;
        }

        public int[] getTopLeft()
        {
            int[] topLeft = new int[2];
            int[] corner = getCorner();
            topLeft[0] = Math.Min(origin[0], corner[0]);
            topLeft[1] = Math.Min(origin[1], corner[1]);
            return topLeft;
        }

        public int[] getTypeList()
        {
            List<int> typeList = new List<int>();
            foreach (int t in types)
            {
                if (!typeList.Contains(t) && t!=-1)
                    typeList.Add(t);
            }
            return typeList.ToArray();
        }

        public void setOrigin(int[] tileOrigin)
        {
            origin[0] = tileOrigin[0];
            origin[1] = tileOrigin[1];
        }

        public void removeOrigin()
        {
            origin[0] = -1;
            origin[1] = -1;
        }

        public void drawTileScheme(SpriteBatch spriteBatch)
        {
            if (origin[0] >= 0)
            {
                int hl = Math.Abs(width);
                int vl = Math.Abs(height);
                int[] cord = getTopLeft();
                int cordX = cord[0];
                int cordY = cord[1];
                for (int dx = 0; dx <= hl; ++dx)
                {
                    cordY = cord[1];
                    for(int dy=0; dy<=vl; ++dy)
                    {
                        int type = types[dx, dy];
                        Tile worldTile = Main.tile[cordX, cordY];
                        if (type == -1)
                        {
                            if(worldTile!=null)
                                if(worldTile.active())
                                    DrawHelper.DrawRedundantBlockMask(cordX, cordY, spriteBatch);
                        }
                        else if (type != -1 && type < Main.tileTexture.Length)
                        { //Draw Block Texture
                            Texture2D tex = Main.tileTexture[type];
                            if (tex != null)
                            {
                                if (worldTile != null)
                                {
                                    if (worldTile.active())
                                    {
                                        if(worldTile.type != type)
                                            DrawHelper.DrawBlockTexture(cordX, cordY, frames[dx, dy, 0], frames[dx, dy, 1], spriteBatch, tex, actuated[dx, dy], true);
                                    }
                                    else
                                        DrawHelper.DrawBlockTexture(cordX, cordY, frames[dx, dy, 0], frames[dx, dy, 1], spriteBatch, tex, actuated[dx, dy]);
                                } else //worldtile 是 null, 代表该位置不可能有方块
                                    DrawHelper.DrawBlockTexture(cordX, cordY, frames[dx, dy, 0], frames[dx, dy, 1], spriteBatch, tex, actuated[dx, dy]);
                            }
                        }
                        ++cordY;
                    }
                    ++cordX;
                }
            }
        }
    }
}

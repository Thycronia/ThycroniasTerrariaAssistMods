using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LitematicaTR;

namespace LitematicaTR.Utiles
{
    public static class DrawHelper
    {
        public static Texture2D outlineV;
        public static Texture2D outlineH;
        public static Texture2D redundantMask;

        private static void drawTileOutlines(int cordX, int cordY,int offsetX, int offsetY, SpriteBatch spriteBatch, Texture2D texture)
        {
            float x = cordX;
            float y = cordY;
            x *= 16;
            y *= 16;
            x += offsetX;
            y += offsetY;
            Vector2 pos = new Vector2(x, y);
            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            screenCenter += Main.screenPosition;
            pos = screenCenter - Main.screenPosition + Main.GameZoomTarget * (pos - screenCenter);
            spriteBatch.Draw(texture, pos, null, Color.White, 0f, Vector2.Zero, Main.GameZoomTarget, SpriteEffects.None, 0f);
        }

        public static void DrawBlockTexture(int cordX,int cordY, int frameX, int frameY, SpriteBatch spriteBatch, Texture2D texture, bool actuated = false, bool incorrect=false)
        {
            float x = cordX;
            float y = cordY;
            x *= 16;
            y *= 16;
            Vector2 pos = new Vector2(x, y);
            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            screenCenter += Main.screenPosition;
            pos = screenCenter - Main.screenPosition + Main.GameZoomTarget * (pos - screenCenter);
            if(incorrect)
                spriteBatch.Draw(texture, pos, new Rectangle(frameX, frameY, 16, 16), Color.Red * LitematicaTR.incorrectBlockAlpha, 0f, Vector2.Zero, Main.GameZoomTarget, SpriteEffects.None, 0f);
            else if (actuated)
                spriteBatch.Draw(texture, pos, new Rectangle(frameX, frameY, 16, 16), Color.Gray*LitematicaTR.actuatedBlockAlpha, 0f, Vector2.Zero, Main.GameZoomTarget, SpriteEffects.None,0f);
            else
                spriteBatch.Draw(texture, pos, new Rectangle(frameX,frameY,16,16), Color.White*LitematicaTR.normalBlockAlpha, 0f, Vector2.Zero, Main.GameZoomTarget, SpriteEffects.None,0f);
        }

        public static void DrawBlockBox(int cordX, int cordY, SpriteBatch spriteBatch, Texture2D texture)
        {
            drawTileOutlines(cordX, cordY, -2, -2, spriteBatch, texture);
        }

        public static void DrawRedundantBlockMask(int cordX, int cordY, SpriteBatch spriteBatch)
        {
            float x = cordX;
            float y = cordY;
            x *= 16;
            y *= 16;
            Vector2 pos = new Vector2(x, y);
            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            screenCenter += Main.screenPosition;
            pos = screenCenter - Main.screenPosition + Main.GameZoomTarget * (pos - screenCenter);
            spriteBatch.Draw(redundantMask, pos, null, Color.White * LitematicaTR.redundantBlockAlpha, 0f, Vector2.Zero, Main.GameZoomTarget, SpriteEffects.None, 0f);
        }

        public static void DrawOutlineR(int cordX, int cordY, SpriteBatch spriteBatch)
        {
            drawTileOutlines(cordX, cordY, 16, -2, spriteBatch, outlineV);
        }

        public static void DrawOutlineL(int cordX, int cordY, SpriteBatch spriteBatch)
        {
            drawTileOutlines(cordX, cordY, -2, -2, spriteBatch, outlineV);
        }

        public static void DrawOutlineT(int cordX, int cordY, SpriteBatch spriteBatch)
        {
            drawTileOutlines(cordX, cordY, -2, -2, spriteBatch, outlineH);
        }

        public static void DrawOutlineB(int cordX, int cordY, SpriteBatch spriteBatch)
        {
            drawTileOutlines(cordX, cordY, -2, 16, spriteBatch, outlineH);
        }

        public static void DrawRectOutline(int[] origin, int[] corner, SpriteBatch spriteBatch)
        {
            int x1 = Math.Min(origin[0], corner[0]);
            int x2 = Math.Max(origin[0], corner[0]);
            int y1 = Math.Min(origin[1], corner[1]);
            int y2 = Math.Max(origin[1], corner[1]);
            for (int x = x1; x <= x2; ++x)
            {
                DrawOutlineT(x, y1, spriteBatch);
                DrawOutlineB(x, y2, spriteBatch);
            }
            for (int y = y1; y <= y2; ++y)
            {
                DrawOutlineL(x1, y, spriteBatch);
                DrawOutlineR(x2, y, spriteBatch);
            }
        }
    }
}

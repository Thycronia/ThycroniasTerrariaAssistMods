using System;
using Terraria.ModLoader;
using Terraria.ID;
using LitematicaTR.Utiles;
using LitematicaTR.UIs;
using LitematicaTR.TileSchem;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LitematicaTR.Items
{
    public class LitematicaPanel : ModItem
    {
        private static Texture2D selectedCordBox;
        private static Texture2D originBox;
        private static Texture2D cornerBox;

        public LitematicaPanel()
        {
            selectedCordBox = LitematicaTR.selectedCordBox;
            originBox = LitematicaTR.originBox;
            cornerBox = LitematicaTR.cornerBox;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helper Panel"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Left click to toggle the litematica control panel.\n This item cannot become a dropped item in the world.");
        }

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.useTime = 20;
			item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
			item.noUseGraphic = true;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = false;
		}

        public override void HoldItem(Player player)
        {
            player.rulerLine = true;
            base.HoldItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                int mouseCordX = 0;
                int mouseCordY = 0;
                mouseCordX = (int)Main.MouseWorld.X / 16;
                mouseCordY = (int)Main.MouseWorld.Y / 16;
                ControlPanelUI.selectCord(mouseCordX, mouseCordY);
            }
            else
            {
                ControlPanelUI.visible = !ControlPanelUI.visible;
                if (ControlPanelUI.visible)
                    ControlPanelUI.setPanelPos(Main.MouseScreen);
            }
            return true;
        }

        public static void DrawWriteBoxLines(int[] origin, int[] corner, SpriteBatch spriteBatch)
        {
            if (origin[2] == 1)
                DrawHelper.DrawBlockBox(origin[0], origin[1], spriteBatch, originBox);
            if (corner[2] == 1)
                DrawHelper.DrawBlockBox(corner[0], corner[1], spriteBatch, cornerBox);
            if (corner[2] == 1 && origin[2] == 1) //Draw Rectangular outline
                DrawHelper.DrawRectOutline(origin, corner, spriteBatch);
        }

        public static void DrawReadBoxLine(int[] origin, TileScheme tileScheme, SpriteBatch spriteBatch)
        {
            if (origin[2] == 1)
                DrawHelper.DrawBlockBox(origin[0], origin[1], spriteBatch, originBox);
            if (tileScheme != null)
            {
                if (tileScheme.getOrigin()[0] >= 0)
                {
                    int[] corner = tileScheme.getCorner();
                    DrawHelper.DrawBlockBox(corner[0], corner[1], spriteBatch, cornerBox);
                    DrawHelper.DrawRectOutline(origin, corner, spriteBatch);
                }
            }
        }

        public static void DrawScheme(SpriteBatch spriteBatch)
        {
            if (ControlPanelUI.getSelectedCord().Item2)
            {
                DrawHelper.DrawBlockBox(ControlPanelUI.getSelectedCord().Item1[0], ControlPanelUI.getSelectedCord().Item1[1], spriteBatch, selectedCordBox);
            }
            switch (ControlPanelUI.getFuncMode())
            {
                case 0:
                    DrawReadBoxLine(ControlPanelUI.getReadOrigin(), ControlPanelUI.getLoadedTileScheme(), spriteBatch);
                    if (ControlPanelUI.getSchemeVis() && ControlPanelUI.getLoadedTileScheme() != null)
                        ControlPanelUI.getLoadedTileScheme().drawTileScheme(spriteBatch);
                    break;

                case 1:
                    DrawWriteBoxLines(ControlPanelUI.getWriteOrigin(), ControlPanelUI.getWriteCorner(), spriteBatch);
                    break;

                default:
                    break;
            }
        }

        //public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        //{
        //    DrawScheme(spriteBatch);
        //    return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        //}

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            item.active = false;
            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}

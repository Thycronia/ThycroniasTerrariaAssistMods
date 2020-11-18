using Terraria;
using Terraria.ModLoader;
using LitematicaTR.UIs;
using LitematicaTR.Utiles;
using LitematicaTR.Items;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LitematicaTR
{
	public class LitematicaTR : Mod
	{
		internal ControlPanelUI controlPanelUI;
		internal UserInterface panelUserInterface;
        //internal ControlPanelUI schemeDrawer;
        //internal UserInterface drawerInterface;
        public static float normalBlockAlpha = 0.4f;
        public static float actuatedBlockAlpha = 0.4f;
        public static float incorrectBlockAlpha = 0.4f;
        public static float redundantBlockAlpha = 0.4f;
        public static string schemePath = "";
        public static Texture2D outlineV;
        public static Texture2D outlineH;
        public static Texture2D redundantMask;
        public static Texture2D selectedCordBox;
        public static Texture2D originBox;
        public static Texture2D cornerBox;

        public override void Load()
        {
            controlPanelUI = new ControlPanelUI();
            controlPanelUI.Activate();
            panelUserInterface = new UserInterface();
            panelUserInterface.SetState(controlPanelUI);
            base.Load();
        }

        private void setUpTextures()
        {
            outlineV = ModContent.GetTexture("LitematicaTR/HelperImages/BorderLineV");
            outlineH = ModContent.GetTexture("LitematicaTR/HelperImages/BorderLineH");
            redundantMask = ModContent.GetTexture("LitematicaTR/HelperImages/RedundantMask");
            selectedCordBox = ModContent.GetTexture("LitematicaTR/HelperImages/Selected");
            originBox = ModContent.GetTexture("LitematicaTR/HelperImages/Origin");
            cornerBox = ModContent.GetTexture("LitematicaTR/HelperImages/Corner");
            DrawHelper.outlineH = outlineH;
            DrawHelper.outlineV = outlineV;
            DrawHelper.redundantMask = redundantMask;
        }

        public override void PostAddRecipes()
        {
            setUpTextures();
            base.PostAddRecipes();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            if (ControlPanelUI.visible)
                panelUserInterface.Update(gameTime);
            //drawerInterface.Update(gameTime);
            LitConfig config = ModContent.GetInstance<LitConfig>();
            normalBlockAlpha = config.normalBlockTransparency / 100f;
            actuatedBlockAlpha = config.actuatedBlockTransparency / 100f;
            incorrectBlockAlpha = config.incorrectBlockTransparency / 100f;
            redundantBlockAlpha = config.redundantBlockTransparency / 100f;
            schemePath = config.schemePath;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextInd = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if(mouseTextInd >= 0)
            {
                layers.Insert(mouseTextInd, new LegacyGameInterfaceLayer(
                //这里是绘制层的名字
                "Litematica: Control Panel",
                //这里是匿名方法
                delegate
                {
                    //当Visible开启时（当UI开启时）
                    if (ControlPanelUI.visible)
                        //绘制UI（运行exampleUI的Draw方法）
                        controlPanelUI.Draw(Main.spriteBatch);
                   return true;
                },
                //这里是绘制层的类型
                InterfaceScaleType.UI)
                );
            }

            //schemeDrawer = new ControlPanelUI();
            //schemeDrawer.Activate();
            //drawerInterface = new UserInterface();
            //drawerInterface.SetState(schemeDrawer);

            int litPanelInd = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Achievement Complete Popups"));
            if (litPanelInd >= 0)
            {
                layers.Insert(litPanelInd, new LegacyGameInterfaceLayer(
                //这里是绘制层的名字
                "Litematica: Scheme Drawing",
                //这里是匿名方法
                delegate
                {
                    LitematicaPanel.DrawScheme(Main.spriteBatch);
                    return true;
                },
                //这里是绘制层的类型
                InterfaceScaleType.UI)
                );
            }
            base.ModifyInterfaceLayers(layers);
        }
    }
}
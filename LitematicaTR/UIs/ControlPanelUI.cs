using System;
using LitematicaTR.Utiles;
using LitematicaTR.TileSchem;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LitematicaTR.UIs
{
    public class ControlPanelUI : UIState
    {
        //private static Texture2D selectedCordBox;
        //private static Texture2D originBox;
        //private static Texture2D cornerBox;

        public static bool visible = false;

        private static int[] selectedCord;

        private static bool hasSelected;

        private static UIPanel panel = new UIPanel();

        private static FuncPanel funcPanel = new FuncPanel();

        private int mode = funcModes.welcomeMode;

        public static void setPanelPos(float left, float top)
        {
            panel.Left.Set(left, 0f);
            panel.Top.Set(top, 0f);
            panel.Recalculate();
        }

        public static void setPanelPos(Vector2 vec)
        {
            setPanelPos(vec.X, vec.Y);
        }

        public ControlPanelUI()
        {
            selectedCord = new int[2];
            hasSelected = false;
        }

        public static void selectCord(int x, int y)
        {
            if(x==selectedCord[0] && y == selectedCord[1])
            {
                clearSelectedCord();
            }
            else
            {
                selectedCord[0] = x;
                selectedCord[1] = y;
                hasSelected = true;
            }
        }

        public static void clearSelectedCord()
        {
            hasSelected = false;
            selectedCord[0] = -1;
            selectedCord[1] = -1;
        }

        public static (int[], bool) getSelectedCord()
        {
            return (selectedCord, hasSelected);
        }

        public static int getFuncMode()
        {
            return funcPanel.getMode();
        }

        public static int[] getWriteOrigin()
        {
            return funcPanel.getWriteOrigin();
        }

        public static int[] getWriteCorner()
        {
            return funcPanel.getWriteCorner();
        }

        public static int[] getReadOrigin()
        {
            return funcPanel.getReadOrigin();
        }

        public static bool getSchemeVis()
        {
            return funcPanel.getSchemeVis();
        }

        public static TileScheme getLoadedTileScheme()
        {
            return funcPanel.getLoadedTileScheme();
        }

        private void funcButtonReadOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            mode = funcModes.readMode;
            funcPanel.updateFuncPanel(mode);
        }

        private void funcButtonWriteOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            mode = funcModes.writeMode;
            funcPanel.updateFuncPanel(mode);
        }

        private void funcButtonConfigOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            mode = funcModes.configMode;
            funcPanel.updateFuncPanel(mode);
        }

        public override void OnInitialize()
        {
            //selectedCordBox = LitematicaTR.selectedCordBox;
            //originBox = LitematicaTR.originBox;
            //cornerBox = LitematicaTR.cornerBox;
            {
                panel.Width.Set(500f, 0f);//488,0
                panel.Height.Set(300f, 0f);//568,0
                panel.Left.Set(0f, 0f);//-244,0
                panel.Top.Set(0f, 0f);//-284,0
                Append(panel);
            } //Add the main panel

            {
                funcPanel.Width.Set(500f, 0f);
                funcPanel.Height.Set(250f,0f);
                funcPanel.Left.Set(0f, 0f);
                funcPanel.Top.Set(-250f, 1f);
                panel.Append(funcPanel);
            } //Add the function panel

            Texture2D buttonTexture = ModContent.GetTexture("Terraria/UI/Bestiary/Button_Wide_Border");

            UIImageButton funcButtonRead = new UIImageButton(buttonTexture);
            {
                funcButtonRead.Width.Set(buttonTexture.Width, 0f);
                funcButtonRead.Height.Set(buttonTexture.Height, 0f);
                funcButtonRead.Left.Set(0f, 0f);
                funcButtonRead.Top.Set(0f, 0f);
                funcButtonRead.OnClick += funcButtonReadOnClick;
                panel.Append(funcButtonRead);

                UIText funcTextRead = new UIText("Read",1.3f);
                funcTextRead.HAlign = funcTextRead.VAlign = 0.5f;
                funcButtonRead.Append(funcTextRead);
            } //Add the read function button
            
            UIImageButton funcButtonWrite = new UIImageButton(buttonTexture);
            {
                funcButtonWrite.Width.Set(buttonTexture.Width, 0f);
                //设置按钮高度
                funcButtonWrite.Height.Set(buttonTexture.Height, 0f);
                //设置按钮距离所属ui部件的最左端的距离
                funcButtonWrite.Left.Set(-buttonTexture.Width/2, 0.5f);
                //设置按钮距离所属ui部件的最顶端的距离
                funcButtonWrite.Top.Set(0f, 0f);
                //注册一个事件，这个事件将会在按钮按下时被激活
                funcButtonWrite.OnClick += funcButtonWriteOnClick;
                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                panel.Append(funcButtonWrite);

                UIText funcTextWrite = new UIText("Write", 1.3f);
                funcTextWrite.HAlign = funcTextWrite.VAlign = 0.5f;
                funcButtonWrite.Append(funcTextWrite);
            } //Add the write function button

            UIImageButton funcButtonConfig = new UIImageButton(buttonTexture);
            {
                funcButtonConfig.Width.Set(buttonTexture.Width, 0f);
                funcButtonConfig.Height.Set(buttonTexture.Height, 0f);
                funcButtonConfig.Left.Set(-buttonTexture.Width, 1f);
                funcButtonConfig.Top.Set(0f, 0f);
                funcButtonConfig.OnClick += funcButtonConfigOnClick;
                panel.Append(funcButtonConfig);

                UIText funcTextConfig = new UIText("Dev", 1.3f);
                funcTextConfig.HAlign = funcTextConfig.VAlign = 0.5f;
                funcButtonConfig.Append(funcTextConfig);
            } //Add the config function button

            base.OnInitialize();
        }

        //public static void DrawWriteBoxLines(int[] origin, int[] corner, SpriteBatch spriteBatch)
        //{
        //    if (origin[2] == 1)
        //        DrawHelper.DrawBlockBox(origin[0], origin[1], spriteBatch, originBox);
        //    if (corner[2] == 1)
        //        DrawHelper.DrawBlockBox(corner[0], corner[1], spriteBatch, cornerBox);
        //    if (corner[2] == 1 && origin[2] == 1) //Draw Rectangular outline
        //        DrawHelper.DrawRectOutline(origin, corner, spriteBatch);
        //}

        //public static void DrawReadBoxLine(int[] origin, TileScheme tileScheme, SpriteBatch spriteBatch)
        //{
        //    if (origin[2] == 1)
        //        DrawHelper.DrawBlockBox(origin[0], origin[1], spriteBatch, originBox);
        //    if (tileScheme != null)
        //    {
        //        if (tileScheme.getOrigin()[0] >= 0)
        //        {
        //            int[] corner = tileScheme.getCorner();
        //            DrawHelper.DrawBlockBox(corner[0], corner[1], spriteBatch, cornerBox);
        //            DrawHelper.DrawRectOutline(origin, corner, spriteBatch);
        //        }
        //    }
        //}

        //public void DrawScheme(SpriteBatch spriteBatch)
        //{
        //    if (getSelectedCord().Item2)
        //    {
        //        DrawHelper.DrawBlockBox(getSelectedCord().Item1[0], getSelectedCord().Item1[1], spriteBatch, selectedCordBox);
        //    }
        //    switch (getFuncMode())
        //    {
        //        case 0:
        //            DrawReadBoxLine(getReadOrigin(), getLoadedTileScheme(), spriteBatch);
        //            if (getSchemeVis() && getLoadedTileScheme() != null)
        //                getLoadedTileScheme().drawTileScheme(spriteBatch);
        //            break;

        //        case 1:
        //            DrawWriteBoxLines(getWriteOrigin(), getWriteCorner(), spriteBatch);
        //            break;

        //        default:
        //            break;
        //    }
        //}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (panel.ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;
        } //鼠标在UI上不会开火
    }
}

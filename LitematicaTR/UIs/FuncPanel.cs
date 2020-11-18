using System;
using System.IO;
using LitematicaTR.Utiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using LitematicaTR.TileSchem;
using Terraria.GameContent.UI.Elements;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LitematicaTR.UIs
{
    public class FuncPanel : UIElement
    {
        private int mode;
        private Texture2D buttonTexture = ModContent.GetTexture("Terraria/UI/Bestiary/Button_Wide_Border");
        private int[] writeOrigin;
        private int[] writeCorner;
        private int[] readOrigin;
        private bool schemeVisible;
        private TileScheme loadedTileScheme = null;
        private NewUITextBox writeFileNameBox = new NewUITextBox("");
        private NewUITextBox readFileNameBox = new NewUITextBox("");
        private UIText readOriginButtonText = new UIText("readOriginButtonText");
        private UIText readOriginCordText = new UIText("readOriginCordText");
        private UIText readFileNameText = new UIText("Not Loaded");
        private UIText writeOriginButtonText = new UIText("writeOriginButtonText");
        private UIText writeOriginCordText = new UIText("Co-ord not selected");
        private UIText writeCornerButtonText = new UIText("writeCornerButtonText");
        private UIText writeCornerCordText = new UIText("Co-ord not selected");

        public FuncPanel()
        {
            mode = -1;
            writeOrigin = new int[3];
            writeCorner = new int[3];
            readOrigin = new int[3];
            schemeVisible = false;
        } // 0:x, 1:y, 2:可用(未被清除)

        public void updateFuncPanel(int mode)
        {
            if (this.mode == mode)
                this.mode = -1;
            else
                this.mode = mode;

            if (this.mode == funcModes.welcomeMode)
                LoadWelcomePanel();
            else if (this.mode == funcModes.readMode)
                LoadReadPanel();
            else if (this.mode == funcModes.writeMode)
                LoadWritePanel();
            else if (this.mode == funcModes.configMode)
                LoadConfigPanel();
            else
                Main.NewText("Unhandled mode type, at FuncPanel.cs, line 39.");
        }

        public int getMode()
        {
            return mode;
        }

        public int[] getWriteOrigin()
        {
            return writeOrigin;
        }

        public int[] getWriteCorner()
        {
            return writeCorner;
        }

        public int[] getReadOrigin()
        {
            return readOrigin;
        }

        public bool getSchemeVis()
        {
            return schemeVisible;
        }

        public TileScheme getLoadedTileScheme()
        {
            return loadedTileScheme;
        }

        private void PrintRead(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.NewText("You are currently in read mode");
        }

        private void clearWriteOrigin()
        {
            writeOrigin[0] = -1;
            writeOrigin[1] = -1;
            writeOrigin[2] = 0;
        }

        private void setWriteOrigin(int x, int y)
        {
            writeOrigin[0] = x;
            writeOrigin[1] = y;
            writeOrigin[2] = 1;
        }

        private void writeOriginButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (writeOrigin[2] == 1)
                clearWriteOrigin();
            else if (writeOrigin[2] == 0)
            {
                (int[], bool) selectedCord = ControlPanelUI.getSelectedCord();
                if (selectedCord.Item2)
                    setWriteOrigin(selectedCord.Item1[0], selectedCord.Item1[1]);
                else
                    Main.NewText("Please select a tile first.");
            }
            else
                Main.NewText("Unhandled value, at FuncPanel.cs, line 85");
        }

        private void clearWriteCorner()
        {
            writeCorner[0] = -1;
            writeCorner[1] = -1;
            writeCorner[2] = 0;
        }

        private void setWriteCorner(int x, int y)
        {
            writeCorner[0] = x;
            writeCorner[1] = y;
            writeCorner[2] = 1;
        }

        private void writeCornerButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (writeCorner[2] == 1)
                clearWriteCorner();
            else if (writeCorner[2] == 0)
            {
                (int[], bool) selectedCord = ControlPanelUI.getSelectedCord();
                if (selectedCord.Item2)
                    setWriteCorner(selectedCord.Item1[0], selectedCord.Item1[1]);
                else
                    Main.NewText("Please select a tile first.");
            }
            else
                Main.NewText("Unhandled value, at FuncPanel.cs, line 120");
        }

        private void writeFileNameButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (writeOrigin[2] == 1 && writeCorner[2] == 1)
            {
                if (writeFileNameBox.getCurrentText().Length > 0)
                {
                    int x1 = Math.Min(writeOrigin[0], writeCorner[0]);
                    int x2 = Math.Max(writeOrigin[0], writeCorner[0]);
                    int y1 = Math.Min(writeOrigin[1], writeCorner[1]);
                    int y2 = Math.Max(writeOrigin[1], writeCorner[1]);
                    short originLocation = -1;
                    if (x1 == writeOrigin[0])
                    {
                        if (y1 == writeOrigin[1])
                            originLocation = OriginLocation.topLeft;
                        else
                            originLocation = OriginLocation.bottomLeft;
                    }
                    else
                    {
                        if (y1 == writeOrigin[1])
                            originLocation = OriginLocation.topRight;
                        else
                            originLocation = OriginLocation.bottomRight;
                    }

                    Tile[,] tiles = new Tile[x2 - x1 + 1, y2 - y1 + 1];
                    for (int x = x1; x <= x2; ++x)
                        for (int y = y1; y <= y2; ++y)
                            tiles[x - x1, y - y1] = Main.tile[x, y];
                    int[] cordOrigin = new int[2];
                    cordOrigin[0] = -1;
                    cordOrigin[1] = -1;
                    TileScheme tileScheme = new TileScheme(tiles, cordOrigin, originLocation);
                    tileScheme.writeToFile(writeFileNameBox.getCurrentText());
                }
                else
                    Main.NewText("Please enter the file name");
            }
            else
                Main.NewText("Please make sure the origin and corner are selected.");
        }

        private void clearReadOrigin()
        {
            readOrigin[0] = -1;
            readOrigin[1] = -1;
            readOrigin[2] = 0;
        }

        private void setReadOrigin(int x, int y)
        {
            readOrigin[0] = x;
            readOrigin[1] = y;
            readOrigin[2] = 1;
        }

        private void readOriginButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (readOrigin[2] == 1)
            {
                clearReadOrigin();
                if (loadedTileScheme != null)
                    loadedTileScheme.removeOrigin();
            }
            else if (readOrigin[2] == 0)
            {
                (int[], bool) selectedCord = ControlPanelUI.getSelectedCord();
                if (selectedCord.Item2)
                {
                    setReadOrigin(selectedCord.Item1[0], selectedCord.Item1[1]);
                    if (loadedTileScheme != null)
                        loadedTileScheme.setOrigin(readOrigin);
                }
                else
                    Main.NewText("Please select a tile first.");
            }
            else
                Main.NewText("Unhandled value, at FuncPanel.cs, line 215");
        }

        private void preLoadRequiredTexture(TileScheme tileScheme)
        {
            foreach(int tileType in tileScheme.getTypeList())
            {
                try
                {
                    Main.instance.LoadTiles(tileType);
                }
                catch
                {
                    Main.NewText("Warning: This file might contains tiles of unloaded mods.");
                }
            }
        }

        private void readFileNameButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            string fileName = readFileNameBox.getCurrentText();
            if (fileName.Length > 0)
            {
                if (fileName.Equals(readFileNameText.Text))
                { //Unload
                    loadedTileScheme = null;
                    readFileNameText.SetText("Not Loaded");
                    clearReadOrigin();
                }
                else
                { //Load
                    string path = LitematicaTR.schemePath + "/" + fileName + ".json";
                    if (File.Exists(path))
                    {
                        loadedTileScheme = new TileScheme(path);
                        readFileNameText.SetText(fileName);
                        if (readOrigin[2] == 1)
                            loadedTileScheme.setOrigin(readOrigin);
                        preLoadRequiredTexture(loadedTileScheme);
                    }
                    else
                        Main.NewText("File not found. Please make sure you entered the correct name.");
                }
            }
            else
                Main.NewText("Please enter the file name.");
        }

        private void readToggleVisibButOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            schemeVisible = !schemeVisible;
        }

        private void LoadReadPanel()
        {
            this.RemoveAllChildren();
            {
                UIImageButton readOriginButton = new UIImageButton(buttonTexture);
                readOriginButton.Width.Set(buttonTexture.Width, 0f);
                readOriginButton.Height.Set(buttonTexture.Height, 0f);
                readOriginButton.Left.Set(0f, 0f);
                readOriginButton.Top.Set(0f, 0.1f);
                readOriginButton.OnClick += readOriginButtonOnClick;
                this.Append(readOriginButton);
                if (readOrigin[2] == 1)
                    readOriginButtonText.SetText("Clear Origin");
                else if (readOrigin[2] == 0)
                    readOriginButtonText.SetText("Set Origin");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 236");
                readOriginButtonText.VAlign = readOriginButtonText.HAlign = 0.5f;
                readOriginButton.Append(readOriginButtonText);

                readOriginCordText.VAlign = 0.5f;
                readOriginCordText.Left.Set(70f, 1f);
                if (readOrigin[2] == 1)
                    readOriginCordText.SetText("(" + readOrigin[0] + ", " + readOrigin[1] + ")");
                else if (readOrigin[2] == 0)
                    readOriginCordText.SetText("Not selected.");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 247");
                readOriginButton.Append(readOriginCordText);
            }//readOrigin button, cord

            {
                readFileNameBox.Width.Set(buttonTexture.Width + 60f, 0f);
                readFileNameBox.Height.Set(buttonTexture.Height, 0f);
                readFileNameBox.Left.Set(0f, 0f);
                readFileNameBox.Top.Set(160f, 0f);
                this.Append(readFileNameBox);

                UIImageButton readFileNameButton = new UIImageButton(buttonTexture);
                readFileNameButton.HAlign = 0.5f;
                readFileNameButton.Top.Set(160f, 0f);
                readFileNameButton.OnClick += readFileNameButtonOnClick;
                this.Append(readFileNameButton);

                UIText readFileButtonText = new UIText("Load/Unload");
                readFileButtonText.HAlign = readFileButtonText.VAlign = 0.5f;
                readFileNameButton.Append(readFileButtonText);

                UIText readFileNameIndicator = new UIText("Loaded File: ");
                readFileNameIndicator.Left.Set(0f, 0f);
                readFileNameIndicator.Top.Set(200f, 0f);
                this.Append(readFileNameIndicator);

                readFileNameText.VAlign = 0.5f;
                readFileNameText.Left.Set(20f, 1f);
                readFileNameIndicator.Append(readFileNameText);
            } //Read Load File TextBox, button

            {
                UIImageButton readToggleVisibBut = new UIImageButton(buttonTexture);
                readToggleVisibBut.HAlign = readToggleVisibBut.VAlign = 0.5f;
                readToggleVisibBut.OnClick += readToggleVisibButOnClick;
                this.Append(readToggleVisibBut);

                UIText readToggleText = new UIText("Toggle Vis");
                readToggleText.HAlign = readToggleText.VAlign = 0.5f;
                readToggleVisibBut.Append(readToggleText);
            } //Toggle Visibility
        }

        private void LoadWritePanel()
        {
            this.RemoveAllChildren();
            {
                //Origin 按钮
                UIImageButton writeOriginButton = new UIImageButton(buttonTexture);
                writeOriginButton.Width.Set(buttonTexture.Width, 0f);
                writeOriginButton.Height.Set(buttonTexture.Height, 0f);
                writeOriginButton.Left.Set(0f, 0f);
                writeOriginButton.Top.Set(0f, 0.1f);
                writeOriginButton.OnClick += writeOriginButtonOnClick;
                this.Append(writeOriginButton);
                if (writeOrigin[2] == 1)
                    writeOriginButtonText.SetText("Clear Origin");
                else if (writeOrigin[2] == 0)
                    writeOriginButtonText.SetText("Set Origin");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 116");
                writeOriginButtonText.VAlign = writeOriginButtonText.HAlign = 0.5f;
                writeOriginButton.Append(writeOriginButtonText);

                //Origin 坐标显示
                writeOriginCordText.VAlign = 0.5f;
                writeOriginCordText.Left.Set(70f, 1f);
                if (writeOrigin[2] == 1)
                    writeOriginCordText.SetText("(" + writeOrigin[0] + ", " + writeOrigin[1] + ")");
                else if (writeOrigin[2] == 0)
                    writeOriginCordText.SetText("Not selected.");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 131");
                writeOriginButton.Append(writeOriginCordText);
            } //Origin按钮及坐标显示

            {
                //Corner 按钮
                UIImageButton writeCornerButton = new UIImageButton(buttonTexture);
                writeCornerButton.Width.Set(buttonTexture.Width, 0f);
                writeCornerButton.Height.Set(buttonTexture.Height, 0f);
                writeCornerButton.Left.Set(0f, 0f);
                writeCornerButton.Top.Set(70f, 0.1f);
                writeCornerButton.OnClick += writeCornerButtonOnClick;
                this.Append(writeCornerButton);
                if (writeCorner[2] == 1)
                    writeCornerButtonText.SetText("Clear Corner");
                else if (writeCorner[2] == 0)
                    writeCornerButtonText.SetText("Set Corner");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 181");
                writeCornerButtonText.VAlign = writeCornerButtonText.HAlign = 0.5f;
                writeCornerButton.Append(writeCornerButtonText);

                //Corner 坐标显示
                writeCornerCordText.VAlign = 0.5f;
                writeCornerCordText.Left.Set(70f, 1f);
                if (writeCorner[2] == 1)
                    writeCornerCordText.SetText("(" + writeCorner[0] + ", " + writeCorner[1] + ")");
                else if (writeCorner[2] == 0)
                    writeCornerCordText.SetText("Not selected.");
                else
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 194");
                writeCornerButton.Append(writeCornerCordText);
            } //Corner按钮及坐标显示

            {
                writeFileNameBox.Width.Set(buttonTexture.Width + 60f, 0f);
                writeFileNameBox.Height.Set(buttonTexture.Height, 0f);
                writeFileNameBox.Left.Set(0f, 0f);
                writeFileNameBox.Top.Set(160f, 0f);
                this.Append(writeFileNameBox);
                UIImageButton writeFileNameButton = new UIImageButton(buttonTexture);

                writeFileNameButton.HAlign = 0.5f;
                writeFileNameButton.Top.Set(160f, 0f);
                writeFileNameButton.OnClick += writeFileNameButtonOnClick;
                this.Append(writeFileNameButton);

                UIText writeFileButtonText = new UIText("Save");
                writeFileButtonText.HAlign = writeFileButtonText.VAlign = 0.5f;
                writeFileNameButton.Append(writeFileButtonText);
            } //TextBox
        }

        private void LoadWelcomePanel()
        {
            this.RemoveAllChildren();
            UIText welcome = new UIText("Welcome to Terraria Litematica");
            welcome.HAlign = welcome.VAlign = 0.5f;
            this.Append(welcome);
        }

        private void LoadConfigPanel()
        {
            this.RemoveAllChildren();
            UIText notice = new UIText("Working in Progress...");
            notice.HAlign = notice.VAlign = 0.5f;
            this.Append(notice);
        }


        public override void Update(GameTime gameTime)
        {
            switch (writeOrigin[2])
            {
                case 0:
                    writeOriginButtonText.SetText("Set Origin");
                    writeOriginCordText.SetText("Not selected.");
                    break;

                case 1:
                    writeOriginButtonText.SetText("Clear Origin");
                    writeOriginCordText.SetText("(" + writeOrigin[0] + ", " + writeOrigin[1] + ")");
                    break;

                default:
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 168.");
                    break;
            }

            switch (writeCorner[2])
            {
                case 0:
                    writeCornerButtonText.SetText("Set Corner");
                    writeCornerCordText.SetText("Not selected.");
                    break;

                case 1:
                    writeCornerButtonText.SetText("Clear Corner");
                    writeCornerCordText.SetText("(" + writeCorner[0] + ", " + writeCorner[1] + ")");
                    break;

                default:
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 168.");
                    break;
            }

            switch (readOrigin[2])
            {
                case 0:
                    readOriginButtonText.SetText("Set Origin");
                    readOriginCordText.SetText("Not selected.");
                    break;

                case 1:
                    readOriginButtonText.SetText("Clear Origin");
                    readOriginCordText.SetText("(" + readOrigin[0] + ", " + readOrigin[1] + ")");
                    break;

                default:
                    Main.NewText("Unhandled value, at FuncPanel.cs, line 400.");
                    break;
            }

            base.Update(gameTime);
        }
    }
}

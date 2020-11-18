using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace LitematicaTR
{
    public class LitConfig: ModConfig
    {
        public LitConfig()
        {
        }

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(40)]
        [Label("Block Render Darkness")]
        [Tooltip("Set the alpha value of rendered non-actuated blocks (40 by default)")]
        public int normalBlockTransparency;

        [DefaultValue(40f)]
        [Label("Actuated Block Darkness")]
        [Tooltip("Set the darkness multiplier value of rendered non-actuated blocks (40 by default)")]
        public int actuatedBlockTransparency;

        [DefaultValue(40f)]
        [Label("Incorrect Block Red Darkness")]
        [Tooltip("Set the darkness multiplier value of rendered incorrect block (40 by default)")]
        public int incorrectBlockTransparency;

        [DefaultValue(40f)]
        [Label("Redundant Block Darkness")]
        [Tooltip("Set the alpha value of rendered redundant block (40 by default)")]
        public int redundantBlockTransparency;

        [DefaultValue("")]
        [Label("Scheme File Path")]
        [Tooltip("Enter the full path of your scheme folder")]
        public string schemePath;
    }
}

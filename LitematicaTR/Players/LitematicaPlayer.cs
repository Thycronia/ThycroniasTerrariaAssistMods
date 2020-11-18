using System;
using Terraria;
using LitematicaTR.UIs;
using Terraria.ModLoader;

namespace LitematicaTR.Players
{
    public class LitematicaPlayer: ModPlayer
    {
        public LitematicaPlayer()
        {
        }

        public override void OnEnterWorld(Player player)
        {
            ControlPanelUI.visible = false;
            base.OnEnterWorld(player);
        }
    }
}

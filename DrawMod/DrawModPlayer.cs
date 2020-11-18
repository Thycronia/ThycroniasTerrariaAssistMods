using Terraria;
using Terraria.ModLoader;

namespace DrawMod
{
    public class DrawModPlayer:ModPlayer
    {
        private Mod calamity;

        public override void OnEnterWorld(Player player)
        {
            calamity = ModLoader.GetMod("CalamityMod");
            if (calamity != null)
            {
                NPC DOGBody = calamity.GetNPC("DevourerofGodsBodyS").npc;
                Main.npcHeadBossTexture[DOGBody.GetBossHeadTextureIndex()] = null;
            }
            base.OnEnterWorld(player);
        }
    }
}

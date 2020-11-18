using Terraria;
using CalamityMod.World;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DrawMod.Items
{
    public class DOGP2Spawner : ModItem
    {
        private Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Scourge"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("The true battle starts at its climax.");
        }

		public override void SetDefaults()
		{
            if (calamity != null)
            {
                Item cosmicWorm = calamity.GetItem("CosmicWorm").item;
                //int cosmicWormType = ModContent.ItemType<CalamityMod.Items.SummonItems.CosmicWorm>();
                item.CloneDefaults(cosmicWorm.type);
            }
		}

        public override bool UseItem(Player player)
        {
            if (calamity != null)
            {
                int DOGType = calamity.GetNPC("DevourerofGodsHeadS").npc.type;
                //int DOGType = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsHeadS>();
                Vector2 OS = new Vector2(1000f, 1000f);
                Vector2 dest = player.position + OS;
                NPC.NewNPC((int)dest.X, (int)dest.Y, DOGType);
            }
            return true;
        }

        public override void AddRecipes()
        {
            if(calamity != null)
            {
                int cosmicWormType = ModContent.ItemType<CalamityMod.Items.SummonItems.CosmicWorm>();
                ModRecipe spawner = new ModRecipe(mod);
                spawner.AddIngredient(cosmicWormType);
                spawner.AddTile(TileID.LunarCraftingStation);
                spawner.SetResult(this);
                spawner.AddRecipe();
            }
            base.AddRecipes();
        }
    }
}

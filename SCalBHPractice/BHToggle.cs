using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SCalBHPractice
{
    public class BHToggle: ModItem
    {
        public BHToggle()
        {
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bullet Hell Toggle"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            Tooltip.SetDefault("Toggle the bulllet hell 5 of Supreme Calamitas.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useTime = 120;
            item.useAnimation = 120;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = 0;
            item.rare = ItemRarityID.Red;
            item.autoReuse = false;
        }

        public override bool UseItem(Player player)
        {
            if (BH5.Activate)
                BH5.endBH5();
            else
                BH5.activateBH5(player);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
            base.AddRecipes();
        }
    }
}

using Terraria.ModLoader;

namespace SCalBHPractice
{
	public class SCalBHPractice : Mod
	{
        public override void MidUpdatePlayerNPC()
        {
            if (BH5.Activate)
                BH5.BH5Practice();

            base.MidUpdatePlayerNPC();
        }
    }
}
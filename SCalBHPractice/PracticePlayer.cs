using System;
using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Terraria.ModLoader;

namespace SCalBHPractice
{
    public class PracticePlayer:ModPlayer
    {
        public PracticePlayer()
        {
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (BH5.Activate)
            {
                damage = 10;
                int SCalDebuffType = ModContent.BuffType<VulnerabilityHex>();
                player.buffImmune[SCalDebuffType] = true;
                SCalDebuffType = ModContent.BuffType<AbyssalFlames>();
                player.buffImmune[SCalDebuffType] = true;
                base.PreUpdateBuffs();
            }
            base.ModifyHitByProjectile(proj, ref damage, ref crit);
        }
    }
}

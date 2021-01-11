using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace SCalBHPractice.Projectiles
{
    public class MonsterOrb:ModProjectile
    {
        public MonsterOrb()
        {
			calamity = ModLoader.GetMod("CalamityMod");
        }

        private float speedAdd;

        private float speedLimit;

		private Mod calamity;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone Monster");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            int brimMonsterType = ModContent.ProjectileType<BrimstoneMonster>();
            projectile.CloneDefaults(brimMonsterType);
            base.SetDefaults();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(speedAdd);
            writer.Write(projectile.localAI[0]);
            writer.Write(speedLimit);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            speedAdd = reader.ReadSingle();
            projectile.localAI[0] = reader.ReadSingle();
            speedLimit = reader.ReadSingle();
        }

		public override void AI()
		{
			int num = (int)projectile.ai[1];
			if (projectile.localAI[0] == 0f)
			{
				projectile.soundDelay = 1125 - num * 225;
				if(calamity!=null)
					Main.PlaySound(calamity.GetLegacySoundSlot((SoundType)50, "Sounds/Custom/BrimstoneMonsterSpawn"), projectile.Center);
				projectile.localAI[0] += 1f;
				switch (num)
				{
					case 0:
						speedLimit = 10f;
						break;
					case 1:
						speedLimit = 20f;
						break;
					case 2:
						speedLimit = 30f;
						break;
					case 3:
						speedLimit = 40f;
						break;
				}
			}
			if (speedAdd < speedLimit)
			{
				speedAdd += 0.04f;
			}
			if (projectile.soundDelay <= 0 && (num == 0 || num == 2))
			{
				projectile.soundDelay = 420;
				if(calamity!=null)
					Main.PlaySound(calamity.GetLegacySoundSlot((SoundType)50, "Sounds/Custom/BrimstoneMonsterDrone"), projectile.Center);
			}
			bool num2 = CalamityWorld.revenge;
			bool flag = CalamityWorld.death;
			Lighting.AddLight(projectile.Center, 3f, 0f, 0f);
			float num3 = (num2 ? 4.5f : 5f) + speedAdd;
			float num4 = (num2 ? 1.5f : 1.35f) + speedAdd * 0.25f;
			float num5 = 160f;
			if (projectile.timeLeft < 90)
			{
				projectile.Opacity=MathHelper.Clamp(projectile.timeLeft / 90f, 0f, 1f);
			}
			else
			{
				projectile.Opacity=MathHelper.Clamp(1f - (projectile.timeLeft - 35910) / 90f, 0f, 1f);
			}
			int num6 = (int)projectile.ai[0];
			if (num6 >= 0 && Main.player[num6].active && !Main.player[num6].dead)
			{
				if (projectile.Distance(Main.player[num6].Center) > num5)
				{
					Vector2 val = projectile.DirectionTo(Main.player[num6].Center);
					if (Utils.HasNaNs(val))
					{
						val = Vector2.UnitY;
					}
					projectile.velocity = (projectile.velocity * (num3 - 1f) + val * num4) / num3;
				}
			}
			else if (projectile.ai[0] != -1f)
			{
				projectile.ai[0] = -1f;
				projectile.netUpdate = true;
			}
			if (flag)
			{
				return;
			}
			float num7 = 0.05f;
			for (int i = 0; i < 1000; i++)
			{
				Projectile val2 = Main.projectile[i];
				if (!(val2).active || i == (projectile).whoAmI)
				{
					continue;
				}
				bool num8 = val2.type == projectile.type;
				float num9 = Vector2.Distance(projectile.Center, val2.Center);
				if (num8 && num9 < 320f)
				{
					if ((projectile).position.X < (val2).position.X)
					{
						(projectile).velocity.X -= num7;
					}
					else
					{
						(projectile).velocity.X += num7;
					}
					if ((projectile).position.Y < (val2).position.Y)
					{
						(projectile).velocity.Y -= num7;
					}
					else
					{
						(projectile).velocity.Y += num7;
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float num = Vector2.Distance(projectile.Center, Utils.TopLeft(targetHitbox));
			float num2 = Vector2.Distance(projectile.Center, Utils.TopRight(targetHitbox));
			float num3 = Vector2.Distance(projectile.Center, Utils.BottomLeft(targetHitbox));
			float num4 = Vector2.Distance(projectile.Center, Utils.BottomRight(targetHitbox));
			float num5 = num;
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			if (num4 < num5)
			{
				num5 = num4;
			}
			return num5 <= 170f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D val = Main.projectileTexture[projectile.type];
			lightColor.R=(byte)(255f * projectile.Opacity);
			spriteBatch.Draw(val, projectile.Center - Main.screenPosition, (Rectangle?)null, projectile.GetAlpha(lightColor), projectile.rotation, Utils.Size(val) / 2f, projectile.scale, (SpriteEffects)0, 0f);
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return projectile.Opacity == 1f;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (projectile.Opacity == 1f)
			{
				target.AddBuff(ModContent.BuffType<AbyssalFlames>(), 900, true);
				target.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 300, true);
			}
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			target.GetModPlayer<CalamityPlayer>().lastProjectileHit = projectile;
		}

	}
}

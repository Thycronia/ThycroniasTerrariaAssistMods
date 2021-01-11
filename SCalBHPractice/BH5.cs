using SCalBHPractice.Projectiles;
using CalamityMod.Projectiles.Boss;
using Terraria;
using Terraria.ModLoader;

namespace SCalBHPractice
{
    public static class BH5
    {
		private static int BHTimeCounter;
		private static int tempBHCounter;
		private static int projectileDamage = 10;
		private static Player targetPlayer = null;

		public static bool Activate;

		public static void activateBH5(Player targetPlayer)
        {
			BHTimeCounter = 0;
			tempBHCounter = 0;
			Activate = true;
			BH5.targetPlayer = targetPlayer;

			for(int i = 0; i < 4; ++i)
				Projectile.NewProjectile(targetPlayer.position.X + Main.rand.Next(-1000, 1001), targetPlayer.position.Y - 1000f, 0f, 1f, ModContent.ProjectileType<MonsterOrb>(), projectileDamage, 0f, Main.myPlayer, 0f,i);
		}

		public static void endBH5()
        {
			BHTimeCounter = 0;
			tempBHCounter = 0;
			Activate = false;
			targetPlayer = null;

			for (int l = 0; l < 1000; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == ModContent.ProjectileType<MonsterOrb>() && proj.timeLeft > 90)
				{
					proj.timeLeft = 90;
				}
			}
		}

        public static void BH5Practice() //不判断合理性，玩家死亡/未开启时不要调用
        {
			/* 
			 * 在源码中BHCounter(BulletHellCounter2)从3600增加到4500
			 * 源码中BulletHellCounter是temporarry变量
			 * 每14帧生成一个BrimstoneHellblast2
			 */
			if (targetPlayer.dead || !targetPlayer.active)
            {
				endBH5();
				return;
			}

			BHTimeCounter++;
			if (BHTimeCounter % 240 == 0)
			{
				Projectile.NewProjectile(targetPlayer.position.X + (float)Main.rand.Next(-1000, 1001), targetPlayer.position.Y - 1000f, 0f, 5f, ModContent.ProjectileType<BrimstoneGigaBlast>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
			}
			if (BHTimeCounter % 360 == 0)
			{
				Projectile.NewProjectile(targetPlayer.position.X + (float)Main.rand.Next(-1000, 1001), targetPlayer.position.Y - 1000f, 0f, 10f, ModContent.ProjectileType<BrimstoneFireblast>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
			}
			if (BHTimeCounter % 30 == 0)
			{
				int num18 = Main.rand.Next(-1000, 1001);
				Projectile.NewProjectile(targetPlayer.position.X + 1000f, targetPlayer.position.Y + (float)num18, -5f, 0f, ModContent.ProjectileType<BrimstoneWave>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(targetPlayer.position.X - 1000f, targetPlayer.position.Y - (float)num18, 5f, 0f, ModContent.ProjectileType<BrimstoneWave>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
			}
			tempBHCounter++;
			if (tempBHCounter > 14)
			{
				tempBHCounter = 0;
				if (BHTimeCounter < 300) //从3600到3900生成从上往下的
				{
					Projectile.NewProjectile(targetPlayer.position.X + (float)Main.rand.Next(-1000, 1001), targetPlayer.position.Y - 1000f, 0f, 4f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
				}
				else if (BHTimeCounter < 600) //3900到4200生成左右的
				{
					Projectile.NewProjectile(targetPlayer.position.X + 1000f, targetPlayer.position.Y + (float)Main.rand.Next(-1000, 1001), -3.5f, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(targetPlayer.position.X - 1000f, targetPlayer.position.Y + (float)Main.rand.Next(-1000, 1001), 3.5f, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
				}
				else //4200到4500同时生成
				{
					Projectile.NewProjectile(targetPlayer.position.X + (float)Main.rand.Next(-1000, 1001), targetPlayer.position.Y - 1000f, 0f, 3f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(targetPlayer.position.X + 1000f, targetPlayer.position.Y + (float)Main.rand.Next(-1000, 1001), -3f, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(targetPlayer.position.X + 1000f, targetPlayer.position.Y + (float)Main.rand.Next(-1000, 1001), 3f, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), projectileDamage, 0f, Main.myPlayer, 0f, 0f);
				}
			}
			if (BHTimeCounter >= 900)
				BHTimeCounter = 0;
        }
    }
}

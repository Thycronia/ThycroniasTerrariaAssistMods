using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DrawMod
{
	public class DrawMod : Mod
	{
		private Mod calamity;
		private List<int> exceptedNPCTypeList;

		public override void PostDrawFullscreenMap(ref string mouseText)
        {
			if (Main.mouseRight && Main.keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
				int mapWidth = Main.maxTilesX * 16;
				int mapHeight = Main.maxTilesY * 16;
				Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

				cursorPosition.X -= Main.screenWidth / 2;
				cursorPosition.Y -= Main.screenHeight / 2;

				Vector2 mapPosition = Main.mapFullscreenPos;
				Vector2 cursorWorldPosition = mapPosition;

				cursorPosition /= 16;
				cursorPosition *= 16 / Main.mapFullscreenScale;
				cursorWorldPosition += cursorPosition;
				cursorWorldPosition *= 16;

				Player player = Main.player[Main.myPlayer];
				cursorWorldPosition.Y -= player.height;
				if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
				else if (cursorWorldPosition.X + player.width > mapWidth) cursorWorldPosition.X = mapWidth - player.width;
				if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
				else if (cursorWorldPosition.Y + player.height > mapHeight) cursorWorldPosition.Y = mapHeight - player.height;

				player.Teleport(cursorWorldPosition, 1, 0);
				player.position = cursorWorldPosition;
				player.velocity = Vector2.Zero;
				player.fallStart = (int)(player.position.Y / 16f);
			}

			base.PostDrawFullscreenMap(ref mouseText);
        }

		private void DrawBossHeadOnBG(NPC boss, SpriteBatch spriteBatch, float scale =1f)
		{
			//Player I = Main.clientPlayer;
			int texInd = boss.GetBossHeadTextureIndex();
            if (texInd >=0)
            {
				Texture2D tex = Main.npcHeadBossTexture[texInd];
				if (tex != null)
				{
					Vector2 P2B = boss.position - Main.LocalPlayer.position;
					P2B /= Main.mapOverlayScale * 2f;
					P2B += Main.LocalPlayer.position - Main.screenPosition;
					spriteBatch.Draw(tex, P2B, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				}
			}
		}

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
			if (Main.mapStyle == 0 || Main.mapStyle ==1)
            {
				foreach (NPC n in Main.npc)
                {
					if (n.boss && n.active)
						DrawBossHeadOnBG(n, spriteBatch);
					else if (exceptedNPCTypeList.Contains(n.type) && n.active)
						DrawBossHeadOnBG(n, spriteBatch);
				}
			}

			base.PostDrawInterface(spriteBatch);
        }

        public override void Load()
        {
			calamity = ModLoader.GetMod("CalamityMod");
			exceptedNPCTypeList = new List<int>();
			if (calamity != null)
			{
                //Add all texture for any NPC you want to draw while doesn't have a boss head texture.

                //int DOGGuardType = ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsHead2>();
				int DOGGuardType = calamity.GetNPC("DevourerofGodsHead2").npc.type;
                Texture2D DOGGuardTex = GetTexture("Textures/CosmicGuardianHeadTex");
				AddBossHeadTexture(DOGGuardTex.Name, DOGGuardType);
				
				int[] provGuardTypes = new int[3];
				provGuardTypes[0] = calamity.GetNPC("ProvSpawnHealer").npc.type;
				provGuardTypes[1] = calamity.GetNPC("ProvSpawnDefense").npc.type;
				provGuardTypes[2] = calamity.GetNPC("ProvSpawnOffense").npc.type;
				Texture2D profGuardTex = GetTexture("Textures/ProfGuards/3");
				AddBossHeadTexture(profGuardTex.Name, provGuardTypes[0]);
				profGuardTex = GetTexture("Textures/ProfGuards/2");
				AddBossHeadTexture(profGuardTex.Name, provGuardTypes[1]);
				profGuardTex = GetTexture("Textures/ProfGuards/0");
				AddBossHeadTexture(profGuardTex.Name, provGuardTypes[2]);


				//Add those NPCs to the exception list.

				exceptedNPCTypeList.Add(DOGGuardType);

				//exceptedNPCTypeList.Add(ModContent.NPCType<CalamityMod.NPCs.ProfanedGuardians.ProfanedGuardianBoss>());
				//exceptedNPCTypeList.Add(ModContent.NPCType<CalamityMod.NPCs.ProfanedGuardians.ProfanedGuardianBoss2>());
				//exceptedNPCTypeList.Add(ModContent.NPCType<CalamityMod.NPCs.ProfanedGuardians.ProfanedGuardianBoss3>());
				
				exceptedNPCTypeList.Add(calamity.GetNPC("SlimeGod").npc.type);
				exceptedNPCTypeList.Add(calamity.GetNPC("SlimeGodRun").npc.type);
				exceptedNPCTypeList.Add(calamity.GetNPC("SlimeGodSplit").npc.type);
				exceptedNPCTypeList.Add(calamity.GetNPC("SlimeGodRunSplit").npc.type);

				exceptedNPCTypeList.Add(calamity.GetNPC("PolterPhantom").npc.type);

				exceptedNPCTypeList.Add(calamity.GetNPC("CalamitasRun").npc.type);
				exceptedNPCTypeList.Add(calamity.GetNPC("CalamitasRun2").npc.type);

				exceptedNPCTypeList.Add(provGuardTypes[0]);
				exceptedNPCTypeList.Add(provGuardTypes[1]);
				exceptedNPCTypeList.Add(provGuardTypes[2]);

				exceptedNPCTypeList.Add(calamity.GetNPC("SupremeCataclysm").npc.type);
				exceptedNPCTypeList.Add(calamity.GetNPC("SupremeCatastrophe").npc.type);
			}
			base.Load();
        }
    }
}
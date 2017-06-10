﻿using CheatSheet.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace CheatSheet
{
	class CheatSheetPlayer : ModPlayer
	{
		public static int MaxExtraAccessories = 6;
		public Item[] ExtraAccessories = new Item[MaxExtraAccessories];
		public int numberExtraAccessoriesEnabled = 0;

		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			for (int i = 0; i < numberExtraAccessoriesEnabled; i++)
			{
				player.VanillaUpdateEquip(ExtraAccessories[i]);
			}
			for (int i = 0; i < numberExtraAccessoriesEnabled; i++)
			{
				player.VanillaUpdateAccessory(player.whoAmI, ExtraAccessories[i], false, ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
			}
		}

		public override void Initialize()
		{
			ExtraAccessories = new Item[MaxExtraAccessories];
			for (int i = 0; i < MaxExtraAccessories; i++)
			{
				ExtraAccessories[i] = new Item();
				ExtraAccessories[i].SetDefaults();
			}
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				["ExtraAccessories"] = ExtraAccessories.Select(ItemIO.Save).ToList(),
				["NumberExtraAccessoriesEnabled"] = numberExtraAccessoriesEnabled
			};
		}

		public override void Load(TagCompound tag)
		{
			tag.GetList<TagCompound>("ExtraAccessories").Select(ItemIO.Load).ToList().CopyTo(ExtraAccessories);
			numberExtraAccessoriesEnabled = tag.GetInt("NumberExtraAccessoriesEnabled");
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (CheatSheet.ToggleCheatSheetHotbarHotKey.JustPressed)
			{
				if (CheatSheet.instance.hotbar.hidden)
				{
					CheatSheet.instance.hotbar.Show();
				}
				else
				{
					CheatSheet.instance.hotbar.Hide();
				}
			}
		}
	}
}
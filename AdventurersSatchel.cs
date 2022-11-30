using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShimmerBankTransmutations
{
	internal class AdventurersSatchel : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Adventurer's Satchel");
			Tooltip.SetDefault("Contains all the storage an adventuerer could ever need");
		}

		public override void SetDefaults() {
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(0, 15);
		}
	}

	internal class TravelerShop : GlobalNPC
	{

		public override void SetupShop(int type, Chest shop, ref int nextSlot) {
			if (type == NPCID.SkeletonMerchant && Main.moonPhase >= (int)MoonPhase.Empty || Main.moonPhase >= (int)MoonPhase.Full && NPC.downedBoss3) {
				shop.item[nextSlot] = new Item(ModContent.ItemType<AdventurersSatchel>());
				nextSlot++;
			}
		}
	}
}

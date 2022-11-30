using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ShimmerBankTransmutations
{
	internal class Configs : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public override void OnLoaded() => ShimmerBankModSystem.ClientConfig = this;

		[DrawTicks]
		[Label("Display Origin")]
		[Tooltip("Choose the origin of the bank accessing buttons")]
		[OptionStrings(new string[] { "Armor & Accessories", "Coins & Ammo" })]
		[DefaultValue("Armor & Accessories")]
		public string ButtonOrigin { get; set; }
	}
}

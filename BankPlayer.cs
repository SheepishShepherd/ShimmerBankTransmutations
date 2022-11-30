using ShimmerBankTransmutations.BankSlotUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ShimmerBankTransmutations
{
	internal class BankPlayer : ModPlayer
	{
		public override void PreUpdate() {
			if (ShimmerBankModSystem.Instance.slots?.selectedBank != -1) {
				Player.chest = -1;
			}
		}

		public override void PostUpdate() {
			if (ShimmerBankModSystem.Instance.slots?.selectedBank != -1) {
				Player.chest = ShimmerBankModSystem.Instance.slots.selectedBank;
			}
		}

	}
}

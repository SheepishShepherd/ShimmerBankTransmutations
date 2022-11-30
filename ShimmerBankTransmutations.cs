using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ShimmerBankTransmutations.BankSlotUI;

namespace ShimmerBankTransmutations
{
	public class ShimmerBankTransmutations : Mod
	{

	}

	public class ShimmerBankModSystem : ModSystem
    {
        public static ShimmerBankModSystem Instance { get; private set; }

        internal UserInterface slotsUI;
        internal BankSlots slots;
		internal static Configs ClientConfig;

		public override void Load() {
			Instance = this;
            if (!Main.dedServ) {
                slots = new BankSlots();
                slots.Activate();
                slotsUI = new UserInterface();
                slotsUI.SetState(slots);
            }
        }

		public override void Unload() {
			ClientConfig = null;
		}

		public override void PostDrawInterface(SpriteBatch spriteBatch) {
            //base.PostDrawInterface(spriteBatch);
            // ItemSlot.Draw(Main.spriteBatch, ref Main.player[Main.myPlayer].trashItem, 6, new Vector2((float)453, (float)426), default(Microsoft.Xna.Framework.Color));
        }

        public override void UpdateUI(GameTime gameTime) {
			if (slotsUI != null) 
                slotsUI.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int PigText = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (PigText != -1) {
                layers.Insert(PigText, new LegacyGameInterfaceLayer(
                    "ShimmerBankTransmutations: Bank Slots",
                    delegate {
						slotsUI.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
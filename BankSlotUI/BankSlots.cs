using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ShimmerBankTransmutations.BankSlotUI
{
    internal class BankSlots : UIState {
        BankItemSlot piggyBank;
        BankItemSlot safeBank;
        BankItemSlot forgeBank;
        BankItemSlot voidBank;

        public int selectedBank = -1;

        public override void OnInitialize() {
            piggyBank = new BankItemSlot(ItemID.PiggyBank, -2);
			piggyBank.Left.Set(Main.screenWidth - 92 + (3 * -47), 0f);
			piggyBank.Top.Set(BankItemSlot.AdjustmentShenanigans() + 174 + (0 * 47), 0f);

			safeBank = new BankItemSlot(ItemID.Safe, -3);
			safeBank.Left.Set(Main.screenWidth - 92 + (3 * -47), 0f);
			safeBank.Top.Set(BankItemSlot.AdjustmentShenanigans() + 174 + (1 * 47), 0f);

			forgeBank = new BankItemSlot(ItemID.DefendersForge, -4);
			forgeBank.Left.Set(Main.screenWidth - 92 + (3 * -47), 0f);
			forgeBank.Top.Set(BankItemSlot.AdjustmentShenanigans() + 174 + (2 * 47), 0f);

			voidBank = new BankItemSlot(ItemID.VoidLens, -5);
			voidBank.Left.Set(Main.screenWidth - 92 + (3 * -47), 0f);
			voidBank.Top.Set(BankItemSlot.AdjustmentShenanigans() + 174 + (3 * 47), 0f);

			Append(piggyBank);
            Append(safeBank);
            Append(forgeBank);
            Append(voidBank);
        }

        public override void Update(GameTime gameTime) {
			if (!Main.LocalPlayer.HasItem(ModContent.ItemType<AdventurersSatchel>())) {
				selectedBank = -1;
			}

			if (!Main.playerInventory) {
				selectedBank = -1;
			}
		}
	}

    internal class BankItemSlot : UIElement {
        internal Item _item;
        internal int _type;
        internal int _bank;
        private readonly int _context = ItemSlot.Context.BankItem;

        public BankItemSlot(int itemType, int bank) {
            _item = new Item(itemType);
            _type = itemType;
            _bank = bank;

			if (_bank == -3 && ModLoader.TryGetMod("ChesterOpensSafe", out Mod mod)) {
				_item.SetDefaults(ItemID.ChesterPetItem);
			}

			Width.Set(TextureAssets.InventoryBack.Width(), 0f);
			Height.Set(TextureAssets.InventoryBack.Height(), 0f);
		}

		public override void Click(UIMouseEvent evt) {
			Main.LocalPlayer.SetTalkNPC(-1);
			BankSlots UI = ShimmerBankModSystem.Instance.slots;
			if (UI.selectedBank == _bank || Main.LocalPlayer.chest == _bank) {
				Main.LocalPlayer.chest = -1;
				UI.selectedBank = -1;
				if (_bank == -5) {
					SoundEngine.PlaySound(SoundID.Item130);
				}
				else if (_bank == -2) {
					SoundEngine.PlaySound(SoundID.Item59);
				}
				else if (_bank == -3 && ModLoader.TryGetMod("ChesterOpensSafe", out Mod mod)) {
					SoundEngine.PlaySound(SoundID.ChesterClose);
				}
				else {
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
			}
			else {
				Main.playerInventory = true;
				UI.selectedBank = _bank;
				if (_bank == -5) {
					SoundEngine.PlaySound(SoundID.Item130);
				}
				else if (_bank == -2) {
					SoundEngine.PlaySound(SoundID.Item59);
				}
				else if (_bank == -3 && ModLoader.TryGetMod("ChesterOpensSafe", out Mod mod)) {
					SoundEngine.PlaySound(SoundID.ChesterOpen);
				}
				else {
					SoundEngine.PlaySound(SoundID.MenuOpen);
				}
			}
		}

		// TODO: Right click logic for Void Bag

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			// Player must have an Adventurer's Satchel to access these options // TODO: Change into shimmer transmutation
			if (!Main.LocalPlayer.HasItem(ModContent.ItemType<AdventurersSatchel>()))
				return;

			// The Equipment page must be open to draw the slots
			string origin = ShimmerBankModSystem.ClientConfig.ButtonOrigin;
			if (origin == "Armor & Accessories" && (!Main.playerInventory || Main.EquipPageSelected != 0))
				return;

			if (origin == "Coins & Ammo" && !Main.playerInventory)
				return;

			// While hovering mouse over the UI...
			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface) {
                // No interactions should be done outside of the UI
                Main.player[Main.myPlayer].mouseInterface = true;

                // Text on hover should not draw
                Main.mouseText = true;

                // Item icons should not show on hover as well
                Main.LocalPlayer.cursorItemIconEnabled = false;
                Main.LocalPlayer.cursorItemIconID = -1;
                Main.ItemIconCacheUpdate(0);
            }

			float oldScale = Main.inventoryScale;
			Asset<Texture2D> oldTexture = TextureAssets.InventoryBack2;

			if (origin == "Armor & Accessories") {
				// Position the UI elements
				Left.Set(Main.screenWidth - 92 + (3 * -47), 0f);
				Top.Set(AdjustmentShenanigans() + 174 + ((_bank + 2) * -47), 0f);

				// Adjust scale and color
				Main.inventoryScale = 0.85f;
				TextureAssets.InventoryBack2 = TextureAssets.InventoryBack11;

				// Draw the item slot
				ItemSlot.Draw(spriteBatch, ref _item, _context, GetDimensions().ToRectangle().TopLeft());

				// Reset scale and color
				Main.inventoryScale = oldScale;
				TextureAssets.InventoryBack2 = oldTexture;
			}
			else if (origin == "Coins & Ammo") {
				// Position the UI elements
				Left.Set(571, 0f);
				Top.Set((int)(85f + ((_bank + 2) * -56 * 0.6f) + 20f), 0f);

				// Adjust scale and color
				Main.inventoryScale = 0.6f;
				TextureAssets.InventoryBack2 = TextureAssets.InventoryBack;

				// Draw the item slot
				ItemSlot.Draw(spriteBatch, ref _item, _context, GetDimensions().ToRectangle().TopLeft());

				// Reset scale and color
				Main.inventoryScale = oldScale;
				TextureAssets.InventoryBack2 = oldTexture;

				if (_bank == -2) {
					Vector2 vector3 = FontAssets.MouseText.Value.MeasureString("Bank");
					Vector2 vector4 = FontAssets.MouseText.Value.MeasureString(Language.GetTextValue("RandomWorldName_Location.Bank"));
					float num3 = vector3.X / vector4.X;
					DynamicSpriteFontExtensionMethods.DrawString(
						spriteBatch,
						FontAssets.MouseText.Value,
						Language.GetTextValue("RandomWorldName_Location.Bank"),
						new Vector2(570f, 84f + (vector3.Y - vector3.Y * num3) / 2f),
						Colors.AlphaDarken(Color.White),
						0f,
						default,
						0.75f,
						SpriteEffects.None,
						0f
					);
				}
			}
		}

		internal static int AdjustmentShenanigans() {
			int num = 0;
			if (Main.mapEnabled) {
				if (!Main.mapFullscreen && Main.mapStyle == 1) {
					num = 256;
				}
				PlayerInput.SetZoom_UI();
				int pushUp = 600;
				if (Main.LocalPlayer.CanDemonHeartAccessoryBeShown()) {
					pushUp = 610 + PlayerInput.SettingsForUI.PushEquipmentAreaUp.ToInt() * 30;
				}
				if (num + pushUp > Main.screenHeight) {
					num = Main.screenHeight - pushUp;
				}
			}
			return num;
		}
	}
}

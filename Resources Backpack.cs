using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Commands;

/*
		This is the gump (Graphical User Menu Pop-up) that displays the count of each item in the player's resource backpack. 
		Crafting options have been severly reduced to cut down on the total number of items needed for crafting in total across all craft systems. 
		The Resource Backpack should never hold more than 71 items. 

		Each item has a max stack value of 60,000, but still counts as one item that needs to be iterated over. 
		Since the total items is limited, iteration should never be an issue since we need to gather data each time the gump is called or refreshed.

		Containers already have an array property to retain what items are inside it, so we use that to get the items in the resource backpack and get 
		the stack amount of each one to display here.

		Additional edits are needed to check the stack values before dropping resources into the resource backpack and to decline the drop if needed.
		This will be in addition to checking the expandable option of MaxPerEntry property of the Resource Backpack, which could be a potential gold sink to allow players upgradeability.
 */

namespace Server.Gumps
{
	public class ResourcesBackpackGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("RGT", AccessLevel.Owner, RGT_OnCommand);

			// command that is available for testing, only available in Debug mode to avoid accidents
#if DEBUG
			CommandSystem.Register("DeleteResourceBackpack", AccessLevel.Owner, DeleteResourceBackpack_OnCommand);
#endif
			
		}

		public static void RGT_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			
			if( from.HasGump(typeof(ResourcesBackpackGump)) ) 
			{
				from.CloseGump(typeof(ResourcesBackpackGump));
			}

			from.SendGump( new ResourcesBackpackGump( from ) );

			Container pack = from.ResourceBackpack;

			if (pack != null)
			{
				pack.DisplayTo(from);
			}
		}

		
#if DEBUG
		// Deletes the resource backpack from command user so we can test if any/all other avenues that reference the resource backpack will follow the correct path(s) to create a new one 
		public static void DeleteResourceBackpack_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			Container pack = from.ResourceBackpack;

			if (pack != null)
			{
				pack.Delete();
			}
		}
#endif
		
		public Type[] ResourceTypes;

		private int Page = 0;
		private int Subpage = 0; // reserved for possible future implementation, to allow for more items to be displayed within a single category

		public static readonly int firstcolumnItem = 25;
		public static readonly int firstcolumnText = 90;

		public static readonly int secondcolumnItem = 275;
		public static readonly int secondcolumnText = 350;

		public static readonly int yStarting = 90;

		public static readonly int ingotID = 7156;

		public ResourcesBackpackGump( Mobile from ) : base( 100, 100 )
		{
			PlayerMobile pm = (PlayerMobile)from;

			Container pack = pm.ResourceBackpack;

			ResourceTypes = LoadResourceTypes();

			if (pack == null)
			{
				pack = new ResourceBackpack
				{
					Movable = false
				};

				pm.AddItem(pack);
			}

			// gumps do not retain any information themselves, so we use the player as an object to store which page they're on
			// this could be done by sending the page number each time a gump is resent, but this way page numbers can be retained across restarts
			Page = pm.ResourcesBackpack_PageID;
			Subpage = pm.ResourcesBackpack_SubpageID;

			Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;

			// Page 0 acts as the background/back drop
			// information displayed on top of it is controlled by the switch case below
			AddPage(0);
			AddBackground(0, 0, 520, 625, 1755);

			// background for the title display of each switch case
			AddImage(155, 10, 1764);

			// Tools above buttons
			int[] itemsArray = { 7868, 0x13E3, 0x1034, 0x0E3B, 0x14EB, 0xF9D, 0x097F, 0x0E9B, 4130 };

			for (int i = 0; i < itemsArray.Length; i++)
			{
				AddItem(i == 0 ? 35 : 35 + (50 * i), 515, itemsArray[i]);
				AddButton(i == 0 ? 40 : 40 + (50 * i), 565, Page == i ? 2154 : 2151, 2151, (i + 1), GumpButtonType.Reply, 0);
			}

			// switch case pages
			switch (Page)
			{
				case 0: // Tinkering
					{
						this.AddPageName(this, "Tinkering");

						DisplayIngots(this, pack);

						break;
					}
				case 1: // Blacksmith
					{
						AddPageName(this, "Blacksmith");

						DisplayIngots(this, pack);

						break;
					}
				case 2: // Carpentry
					{
						AddPageName(this, "Carpentry");

						int boardID = 7137;

						/* 
							NOTE:

							All arrays used follow the same format:

						 	Item ID, ResourceTypes array indexID, hue #
						*/

						// boards
						int[,,] carpentryArray = new int[,,]
						{ 	{
								{ boardID, 22, CraftResources.GetHue((CraftResource)301) },
								{ boardID, 23, CraftResources.GetHue((CraftResource)302) },
								{ boardID, 24, CraftResources.GetHue((CraftResource)303) },
								{ boardID, 25, CraftResources.GetHue((CraftResource)304) },
								{ boardID, 26, CraftResources.GetHue((CraftResource)305) },
								{ boardID, 27, CraftResources.GetHue((CraftResource)306) },
								{ boardID, 28, CraftResources.GetHue((CraftResource)307) },
								{ 1, 7, 0 },	// skip item
								{ 5991, 8, 0 }, // cloth
						} 	};

						DisplayIngots(this, pack);
						DisplayItems(this, pack, carpentryArray, false, true);


						break;
					}
				case 3: // Inscription
					{
						AddPageName(this, "Inscription");

						int[,,] reagentArray = new int[,,]
						{ 	{ 
								{3962, 29, 0}, // black pearl
								{3963, 30, 0}, // blood moss
								{3972, 31, 0}, // garlic
								{3973, 32, 0}, // ginseng 
								{3974, 33, 0}, // mandrake root
								{3976, 34, 0}, // nightshade
								{3980, 35, 0}, // sulfurous ash
								{3981, 36, 0}, // spider silk
						} 	};

						int[,,] variousItems = new int[,,]
						{ 	{
								{3827, 41, 0}, // blank scroll
								{7122, 39, 0}, // feathers
								{8011, 54, 0}, // recall scroll
								{8033, 55, 0}, // gate scroll
								{4157, 42, 0} // wood pulp
						} 	};

						DisplayItems(this, pack, reagentArray, true);
						DisplayItems(this, pack, variousItems, false, true);

						break;
					}
				case 4: // Cartography
					{
						AddPageName(this, "Cartography");

						int[,,] blankMap = new int[,,]
						{ 	{
								{5356, 56, 0}, // blank map
						} 	};

						DisplayItems(this, pack, blankMap);

						break;
					}
				case 5: // Tailoring
					{
						AddPageName(this, "Sewing");

						int leatherID = 4225;

						int[,,] leather = new int[,,]
						{ 	{ 
								{leatherID, 38, CraftResources.GetHue((CraftResource)101)},
								{leatherID, 37, CraftResources.GetHue((CraftResource)102)},
								{leatherID, 57, CraftResources.GetHue((CraftResource)103)},
								{leatherID, 58, CraftResources.GetHue((CraftResource)104)},
						} 	};

						int[,,] tailoringItems = new int[,,]
						{ 	{ 
								{5991, 38, 0}, // cut cloth
								{3576, 37, 0}, // wool (used to make yarn)
								{3613, 57, 0}, // yarn (to make bolts of cloth) 
								{3991, 58, 0}, // bolt of cloth
						} 	};

						DisplayItems(this, pack, leather);
						DisplayItems(this, pack, tailoringItems, false, true);

						break;
					}
				case 6: // Cooking
					{
						AddPageName(this, "Cooking");

						int[,,] cookingitems = new int[,,]
						{	{
								{2490,60, 0}, // RawBird
								{2426,61, 0}, // RawFishSteak
								{5641,62, 0}, // RawLambLeg
								{2485,63, 0}, // Eggs
								{2512,64, 0}, // Apple
								{2513,65, 0}, // Grapes
								{3852,66, 0}, // GreaterHealPotion
								{3849,67, 0}, // GreaterStrengthPotion
						}	};

						int[,,] cookingitemsColumnTwo = new int[,,]
						{	{
								{4157,68, 0}, // Dough
								{4154,69, 0}, // SackFlourOpen
								{4088,71, 0}, // Pitcher
								{5991,22, 0}, // Board
								{4108,70, 0}, // WheatSheaf
						} 	};

						DisplayItems(this, pack, cookingitems, true, false);
						DisplayItems(this, pack, cookingitemsColumnTwo, false, true);


						break;
					}
				case 7: // Alchemy
					{
						AddPageName(this, "Alchemy");

						int[,,] reagentsArray = new int[,,]
						{ 	{
								{3962, 29, 0}, // black pearl
								{3963, 30, 0}, // blood moss
								{3972, 31, 0}, // garlic
								{3973, 32, 0}, // ginseng
								{3974, 33, 0}, // mandrake root
								{3976, 34, 0}, // nighshade
								{3980, 35, 0}, // sulfurous ash
								{3981, 36, 0}, // spider silk
						} 	};

						DisplayItems(this, pack, reagentsArray, true, false);

						break;
					}
				case 8: // Bowcraft
					{
						AddPageName(this, "Bowcraft");

						int boardID = 7137;

						// Boards
						int[,,] bowcraftArray = new int[,,]
						{ 	{
								{ boardID, 22, CraftResources.GetHue((CraftResource)301) },
								{ boardID, 23, CraftResources.GetHue((CraftResource)302) },
								{ boardID, 24, CraftResources.GetHue((CraftResource)303) },
								{ boardID, 25, CraftResources.GetHue((CraftResource)304) },
								{ boardID, 26, CraftResources.GetHue((CraftResource)305) },
								{ boardID, 27, CraftResources.GetHue((CraftResource)306) },
								{ boardID, 28, CraftResources.GetHue((CraftResource)307) },
						} 	};

						int[,,] bowcraftVarious = new int[,,]
						{ 	{
								{7125, 59, 0}, // shaft
								{7122, 39, 0} // feather
						} 	};

						DisplayItems(this, pack, bowcraftArray);
						DisplayItems(this, pack, bowcraftVarious, false, true);

						break;
					}
			}
		}


		/*
			Format for displaying ingots is done differently since all items use the same ItemID, and the index values for ResourceTypes & CraftResources match
			If custom ingots are added in, this format may need to be changed
		*/
		private void DisplayIngots(Gump gump, Container pack)
		{
			ResourceBackpack backpack = (ResourceBackpack)pack;

			for (int i = 0; i < 9; i++)
			{
				Item[] item = pack.FindItemsByType(ResourceTypes[i], false);

				gump.AddItem(firstcolumnItem, yStarting + (40 * i), 7156, CraftResources.GetHue((CraftResource)(i + 1)));

				if (item.Length > 0)
					gump.AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { backpack.MaxPerEntry.ToString("#,0") }", false, false);
				else
					gump.AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { backpack.MaxPerEntry.ToString("#,0") }", false, false);
			}
		}

		/*
			Method used to populate other pages from data stored in the arrays.
			Optional parameters are available to indicate if the item should be
		*/
		private void DisplayItems(Gump gump, Container pack, int[,,] array, bool displayDoubleItem = false, bool secondColumn = false)
		{
			ResourceBackpack backpack = (ResourceBackpack)pack;

			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int k = 0; k < array.GetLength(1); k++)
				{
					int itemid = array[i, k, 0];
					int index = array[i, k, 1];
					int hue = array[i, k, 2];

					int itemlocationX = !secondColumn ? firstcolumnItem : secondcolumnItem;
					int textlocationX = !secondColumn ? firstcolumnText : secondcolumnText;

					// skip item, indicates that we want a space between entries 
					if (itemid == 1)
						continue; 

					Item[] item = pack.FindItemsByType(ResourceTypes[index], false);

					gump.AddItem(itemlocationX, yStarting + (40 * k), itemid, hue);
					if (displayDoubleItem)
						gump.AddItem(itemlocationX + 5, yStarting + (40 * k) + 5, itemid, hue);

					if (item.Length > 0)
						gump.AddHtml(textlocationX, yStarting + (40 * k), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { backpack.MaxPerEntry.ToString("#,0") }", false, false);
					else
						gump.AddHtml(textlocationX, yStarting + (40 * k), 138, 22, $"<basefont color=#FFFFFF>0 / { backpack.MaxPerEntry.ToString("#,0") }", false, false);
				}
			}
		}

		private Type[] LoadResourceTypes()
		{
			// These IDs are used to search for particular items in the Resource Backpack

			Type[] resourceTypes = new Type[]
			{
				typeof(IronIngot),	// 0 
				typeof(DullCopperIngot),
				typeof(ShadowIronIngot),
				typeof(CopperIngot),
				typeof(BronzeIngot),
				typeof(GoldIngot),
				typeof(AgapiteIngot),
				typeof(VeriteIngot),
				typeof(ValoriteIngot), // 8

				typeof(Granite), // 9
				typeof(DullCopperGranite),
				typeof(ShadowIronGranite),
				typeof(CopperGranite),
				typeof(BronzeGranite),
				typeof(GoldGranite),
				typeof(AgapiteGranite),
				typeof(VeriteGranite),
				typeof(ValoriteGranite), // 17

				typeof(Leather), // 18
				typeof(SpinedLeather),
				typeof(HornedLeather),
				typeof(BarbedLeather), // 21

				typeof(Board), // 22
				typeof(OakBoard),
				typeof(AshBoard),
				typeof(YewBoard),
				typeof(HeartwoodBoard),
				typeof(BloodwoodBoard),
				typeof(FrostwoodBoard), // 28

				typeof(BlackPearl), // 29
				typeof(Bloodmoss),
				typeof(Garlic),
				typeof(Ginseng),
				typeof(MandrakeRoot),
				typeof(Nightshade),
				typeof(SulfurousAsh),
				typeof(SpidersSilk), // 36

				typeof(Wool), // 37
				typeof(Cloth), // 38
				typeof(Feather), // 39
				typeof(Bottle), // 40
				typeof(BlankScroll), // 41
				typeof(WoodPulp), // 42

				typeof(StarSapphire), // 43
				typeof(Emerald),
				typeof(Sapphire),
				typeof(Ruby),
				typeof(Citrine),
				typeof(Amethyst),
				typeof(Tourmaline),
				typeof(Amber),
				typeof(Diamond), // 51

				typeof(Arrow), // 52
				typeof(Bolt), // 53

				typeof(RecallScroll), // 54
				typeof(GateTravelScroll), // 55

				typeof(BlankMap), // 56

				typeof(DarkYarn), // 57
				typeof(BoltOfCloth), // 58
				typeof(Shaft), // 59

				typeof(RawBird), // 60
				typeof(RawFishSteak), // 61
				typeof(RawLambLeg), // 62

				typeof(Eggs), // 63
				typeof(Apple), // 64
				typeof(Grapes), // 65

				typeof(GreaterHealPotion), // 66
				typeof(GreaterStrengthPotion), // 67

				typeof(Dough), // 68
				typeof(SackFlourOpen), // 69
				typeof(WheatSheaf), // 70
				typeof(Pitcher), // 71
			};

			return resourceTypes;
		}

		private void AddPageName(Gump gump, string name)
		{
			gump.AddHtml(169, 22, 172, 22, $"<center>{name}", false, false);
		}
		
		public void Resend( Mobile from )
		{
			if( from.HasGump(typeof(ResourcesBackpackGump)) ) 
			{
				from.CloseGump(typeof(ResourcesBackpackGump));
			}
			from.SendGump( new ResourcesBackpackGump( from ) );
		}

		public static void ResendIfOpen(Mobile from)
		{
			if (from.HasGump(typeof(ResourcesBackpackGump)))
			{
				from.CloseGump(typeof(ResourcesBackpackGump));
				from.SendGump(new ResourcesBackpackGump(from));
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;

			if (info.ButtonID > 0 && info.ButtonID <= 9)
			{
				((PlayerMobile)from).ResourcesBackpack_PageID = (info.ButtonID - 1);
				Resend(from);
			}
		}
	}
}

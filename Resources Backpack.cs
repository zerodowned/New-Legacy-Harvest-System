using System;
using Server;
using Server.Network;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Commands;

using System.Linq;

namespace Server.Gumps
{
	public class ResourcesBackpackGump : Gump
	{
        public static void Initialize()
        {
            CommandSystem.Register("RGT", AccessLevel.Owner, RGT_OnCommand);
/*
#if DEBUG
			CommandSystem.Register("DeleteResourceBackpack", AccessLevel.Owner, DeleteResourceBackpack_OnCommand);
			CommandSystem.Register("DeleteResourceBackpackList", AccessLevel.Owner, DeleteResourceBackpackList_OnCommand);
#endif
*/
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

/*
#if DEBUG
		public static void DeleteResourceBackpack_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			Container pack = from.ResourceBackpack;

			if (pack != null)
			{
				pack.Delete();
			}
		}

		public static void DeleteResourceBackpackList_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;
			Container pack = from.ResourceBackpack;

			if (pack != null)
			{
				((ResourceBackpack)pack).ResourcesBackpackDictionary = null;
			}
		}
#endif
*/

		public const int ResourceMax = 60000;
		private int Page = 0;
		private int Subpage = 0;

		public ResourcesBackpackGump( Mobile from ) : base( 100, 100 )
		{
			PlayerMobile pm = (PlayerMobile)from;

			Container pack = pm.ResourceBackpack;

			if (pack == null)
			{
				pack = new ResourceBackpack
				{
					Movable = false
				};

				pm.AddItem(pack);
			}

			int Max = ((ResourceBackpack)pack).MaxPerEntry;

			Page = pm.ResourcesBackpack_PageID;
			Subpage = pm.ResourcesBackpack_SubpageID;

			Closable =true; 
			Disposable=true; 
			Dragable=true; 
			Resizable=false;

			AddPage(0);

			AddBackground(0, 0, 520, 625, 1755);
			AddImage(155, 10, 1764);

			// Tools
			int[] itemsArray = { 7868, 0x13E3, 0x1034, 0x0E3B, 0x14EB, 0xF9D, 0x097F, 0x0E9B, 4130 };

			//AddItem(35, 515, 7868); // Tinker
			
			for(int i = 0; i < itemsArray.Length; i++)
			{
				AddItem(i == 0 ? 35 : 35 + (50 * i), 515, itemsArray[i] );
				AddButton( i == 0 ? 40 : 40 + (50 * i), 565, Page == i ? 2154 : 2151, 2151, (i + 1), GumpButtonType.Reply, 0);
			}

			int firstcolumnItem = 25;
			int firstcolumnText = 90;
			int secondcolumnItem = 275;
			int secondcolumnText = 350;

			int yStarting = 90;

			Item[] item;

			switch (Page)
			{
				case 0: // Tinkering
					{
						AddPageName(this, "Tinkering");

						for (int i = 0; i < 9; i++)
						{
							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(firstcolumnItem, yStarting + (40 * i), 7156, CraftResources.GetHue((CraftResource)(i + 1)));

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
				case 1: // Blacksmith
					{
						AddPageName(this, "Blacksmith");

						for (int i = 0; i < 9; i++)
						{
							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(firstcolumnItem, yStarting + (40 * i), 7156, CraftResources.GetHue((CraftResource)(i + 1)));

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
				case 2: // Carpentry
					{
						AddPageName(this, "Carpentry");

						for (int i = 22; i < 29; i++)
						{
							int j = i - 22;

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(firstcolumnItem, yStarting + (40 * j), 7137, CraftResources.GetHue((CraftResource)(j + 301))); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * j), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * j), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[38], false);

						AddItem(firstcolumnItem, yStarting + (40 * 8), 5991, 0);

						if (item.Length > 0)
							AddHtml(firstcolumnText, yStarting + (40 * 8), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
						else
							AddHtml(firstcolumnText, yStarting + (40 * 8), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);

						for (int i = 0; i < 9; i++)
						{
							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(secondcolumnItem, yStarting + (40 * i), 7156, CraftResources.GetHue((CraftResource)(i + 1)));

							if (item.Length > 0)
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
				case 3: // Inscription
					{
						AddPageName(this, "Inscription");

						// reagents
						int[,] reagents = new int[,]
						{
							/* itemID, indexID */
							{3962, 29},
							{3963, 30},
							{3972, 31},
							{3973, 32},
							{3974, 33},
							{3976, 34},
							{3980, 35},
							{3981, 36},
						};

						for (int i = 0; i < reagents.GetLength(0); i++)
						{
							int itemid = reagents[i, 0];
							int index = reagents[i, 1];

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(firstcolumnItem, yStarting + (40 * i), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301
							AddItem(firstcolumnItem + 5, yStarting + (40 * i) + 5, itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						int[,] array = new int[,]
						{
							/* itemID, indexID */
							{3827, 41}, // blank scroll
							{7122, 39}, // feathers
							{8011, 54}, // recall scroll
							{8033, 55}, // gate scroll
							{4157, 42} // wood pulp
						};

						for (int i = 0; i < array.GetLength(0); i++)
						{
							int itemid = array[i, 0];
							int index = array[i, 1];

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(secondcolumnItem, yStarting + (40 * i), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
				case 4: // Cartography
					{
						AddPageName(this, "Cartography");

						item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[56], false);

						AddItem(firstcolumnItem, yStarting, 5356, 0); // ResourceInfo.cs, wood starts at enum value 301

						if (item.Length > 0)
							AddHtml(firstcolumnText, yStarting, 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
						else
							AddHtml(firstcolumnText, yStarting, 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);

						break;
					}
				case 5: // Sewing
					{
						AddPageName(this, "Sewing");

						// leather
						for (int i = 18; i < 22; i++)
						{
							int offset = i - 18;
							int hue = offset + 101;

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(firstcolumnItem, yStarting + (40 * offset), 4225, CraftResources.GetHue((CraftResource)(hue)));

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * offset), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * offset), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						

						// extras
						int[,] array = new int[,]
						{
							/* itemID, indexID */
							{5991, 38},
							{3576, 37},
							{3613, 57}
						};

						for (int i = 0; i < array.GetLength(0); i++)
						{
							int itemid = array[i, 0];
							int index = array[i, 1];

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(secondcolumnItem, yStarting + (40 * i), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						// bolt of cloth
						item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[58], false);

						AddItem(secondcolumnItem, yStarting + (40 * 3), 3991, 0);

						if (item.Length > 0)
							AddHtml(secondcolumnText, yStarting + (40 * 3), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
						else
							AddHtml(secondcolumnText, yStarting + (40 * 3), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);


						break;
					}
				case 6: // Cooking
					{
						AddPageName(this, "Cooking");

						int[,] array = new int[,]
						{
							/* itemID, indexID */
							{2490,60}, // RawBird // 60
							{2426,61}, // RawFishSteak // 61
							{5641,62}, // RawLambLeg // 62

							{2485,63}, // Eggs // 63
							{2512,64}, // Apple // 64
							{2513,65}, // Grapes // 65

							{3852,66}, // GreaterHealPotion // 66
							{3849,67}, // GreaterStrengthPotion // 67

							// second column
							{4157,68}, // Dough // 68
							{4154,69}, // SackFlourOpen // 69
							{4088,71}, // Pitcher // 72
							{5991,22}, // Board // 22
							{4108,70}, // WheatSheaf // 71
						};

						for (int i = 0; i < array.GetLength(0); i++)
						{
							int itemid = array[i, 0];
							int index = array[i, 1];

							int itemX = i < 8 ? firstcolumnItem : secondcolumnItem;
							int textX = i < 8 ? firstcolumnText : secondcolumnText;

							int multiplier = i < 8 ? i : i - 8;

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(itemX, yStarting + (40 * multiplier), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301
							AddItem(itemX + 5, yStarting + (40 * multiplier) + 5, itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(textX, yStarting + (40 * multiplier), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(textX, yStarting + (40 * multiplier), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}


						break;
					}
				case 7: // Alchemy
					{
						AddPageName(this, "Alchemy");

						// reagents
						int[,] array = new int[,]
						{
							/* itemID, indexID */
							{3962, 29},
							{3963, 30},
							{3972, 31},
							{3973, 32},
							{3974, 33},
							{3976, 34},
							{3980, 35},
							{3981, 36},
						};

						for (int i = 0; i < array.GetLength(0); i++)
						{
							int itemid = array[i, 0];
							int index = array[i, 1];

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(firstcolumnItem, yStarting + (40 * i), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301
							AddItem(firstcolumnItem + 5, yStarting + (40 * i) + 5, itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
				case 8: // Bowcraft
					{
						AddPageName(this, "Bowcraft");

						for (int i = 22; i < 29; i++)
						{
							int j = i - 22;

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[i], false);

							AddItem(firstcolumnItem, yStarting + (40 * j), 7137, CraftResources.GetHue((CraftResource)(j + 301))); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(firstcolumnText, yStarting + (40 * j), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(firstcolumnText, yStarting + (40 * j), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						int[,] array = new int[,]
						{
							/* itemID, indexID */
							{7125, 59}, // shaft
							{7122, 39} // feather
						};

						for (int i = 0; i < array.GetLength(0); i++)
						{
							int itemid = array[i, 0];
							int index = array[i, 1];

							item = pack.FindItemsByType(((ResourceBackpack)pack).ResourceTypes[index], false);

							AddItem(secondcolumnItem, yStarting + (40 * i), itemid, 0); // ResourceInfo.cs, wood starts at enum value 301

							if (item.Length > 0)
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>{ item[0].Amount.ToString("#,0") } / { Max.ToString("#,0") }", false, false);
							else
								AddHtml(secondcolumnText, yStarting + (40 * i), 138, 22, $"<basefont color=#FFFFFF>0 / { Max.ToString("#,0") }", false, false);
						}

						break;
					}
			}

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
﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                            //
//           Credits to MarioGK for his Lib & Template also Credits to Joker for Parts of his Lib             //
//                                                                                                            //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;

namespace T2IN1_Pantheon
{
    public static class Consumables
    {
        // Cleansers
        public static Item Biscuit = new Item(ItemId.Total_Biscuit_of_Rejuvenation);
        public static Item Health = new Item(ItemId.Health_Potion);
        public static Item Refillable = new Item(ItemId.Refillable_Potion);
        public static Item Hunters = new Item(ItemId.Hunters_Potion);
        public static Item Corrupting = new Item(ItemId.Corrupting_Potion);

        public static List<Item> ItemList = new List<Item>
        {
            Biscuit,
            Health,
            Refillable,
            Hunters,
            Corrupting
        };
    }
}

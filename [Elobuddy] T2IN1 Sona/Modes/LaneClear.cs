﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                            //
//           Credits to MarioGK for his Lib & Template also Credits to Joker for Parts of his Lib             //
//                                                                                                            //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using T2IN1_Lib;
using static T2IN1_Sona.Menus;
using static T2IN1_Sona.SpellsManager;

namespace T2IN1_Sona
{
    internal static class LaneClear
    {
        public static void Execute()
        {
            if (LaneClearMenu["R"].Cast<CheckBox>().CurrentValue)
                if ((Player.Instance.CountEnemyMinionsInRange(900) >= 4) && R.IsReady() && R.IsLearned)
                    R.Cast(R.GetBestCircularFarmPosition(4));
        }
    }
}
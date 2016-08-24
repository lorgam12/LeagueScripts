﻿using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using static RaINAIO.TeemoMenus;

namespace RaINAIO
{
    internal class TeemoCombo
    {
        /*
        Targeted spells are like Katarina`s Q
        Active spells are like Katarina`s W
        Skillshots are like Ezreal`s Q
        Circular Skillshots are like Lux`s E and Tristana`s W
        Cone Skillshots are like Annie`s W and ChoGath`s W
        */

        //Declares Spells
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;

        public static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();

        public static void ComboSpells()
        {
            //Declare Q Spell Values
            Q = new Spell.Targeted(spellSlot: SpellSlot.Q, spellRange: 580);

            //Triggers with every Core Tick
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            //Returns true if Combo Mode is Active in the Orbwalker 
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
                Combo();
        }

        public static void Combo()
        {
            //Returns TargetSelector Target from the Range set
            var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            //Return true of Target doesnt exist or null
            if (qtarget == null)
            {
                return;
            }

            //Returns true if the Checkbox Q is enabled
            if (ComboMenu["Q"].Cast<CheckBox>().CurrentValue)
            {
                //Returns true if Target si valid in Q Range
                if (qtarget.IsValidTarget(Q.Range) && Q.IsReady())
                {
                    //Cast already applies Prediction so its not needed to use Qpred.CastPostion
                    Q.Cast(qtarget);
                }
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;
using static RaINAIO.TeemoMenus;

namespace RaINAIO
{
    public static class TeemoSpellManager
    {

        #region Spell Declare Function

        /*
        Targeted spells are like Katarina`s Q
        Active spells are like Katarina`s W
        Skillshots are like Ezreal`s Q
        Circular Skillshots are like Lux`s E and Tristana`s W
        Cone Skillshots are like Annie`s W and ChoGath`s W
        */

        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;

        public static List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();

        public static void InitializeSpells()
        {

            SpellList.Add(Q);
            SpellList.Add(W);
            SpellList.Add(E);
            SpellList.Add(R);

            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        #endregion Spell Declare Function

        #region Spell Damages and Stuff Function

        /// It will return the damage but you need to set them before getting the damage
        public static float GetDamage(this Obj_AI_Base target, SpellSlot slot)
        {
            const DamageType damageType = DamageType.Magical;
            var AD = Player.Instance.FlatPhysicalDamageMod;
            var AP = Player.Instance.FlatMagicDamageMod;
            var sLevel = Player.GetSpell(slot).Level - 1;

            //You can get the damage information easily on http://de.leagueoflegends.wikia.com/wiki/League_of_Legends_Wiki

            var dmg = 0f;

            switch (slot)
            {
                case SpellSlot.Q:
                    if (Q.IsReady())
                    {
                        //Information Q Damage
                        dmg += new float[] { 80, 125, 170, 215, 260 }[sLevel] + 0.8f * AP;
                    }
                    break;
                case SpellSlot.W:
                    if (W.IsReady())
                    {
                        //Information W Damage
                        dmg += new float[] { 0, 0, 0, 0, 0 }[sLevel] + 0.0f * AP;
                    }
                    break;
                case SpellSlot.E:
                    if (E.IsReady())
                    {
                        //Information E Damage
                        dmg += new float[] { 10, 20, 30, 40, 50 }[sLevel] + 0.3f * AP;
                    }
                    break;
                case SpellSlot.R:
                    if (R.IsReady())
                    {
                        //Information R Damage
                        dmg += new float[] { 200, 325, 450 }[sLevel] + 0.5f * AP;
                    }
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(target, damageType, dmg - 10);
        }

        #endregion Spell Damages and Stuff Function

        #region Unit Level Up Function

        /// This event is triggered when a unit levels up
        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (MiscMenu.GetCheckBoxValue("activateAutoLVL") && sender.IsMe)
            {
                var delay = MiscMenu.GetSliderValue("delaySlider");
                Core.DelayAction(LevelUPSpells, delay);

            }
        }

        /// It will level up the spell using the values of the comboboxes on the menu as a priority
        private static void LevelUPSpells()
        {
            if (Player.Instance.Spellbook.CanSpellBeUpgraded(SpellSlot.R))
            {
                Player.Instance.Spellbook.LevelSpell(SpellSlot.R);
            }

            var firstFocusSlot = GetSlotFromComboBox(MiscMenu.GetComboBoxValue("firstFocus"));
            var secondFocusSlot = GetSlotFromComboBox(MiscMenu.GetComboBoxValue("secondFocus"));
            var thirdFocusSlot = GetSlotFromComboBox(MiscMenu.GetComboBoxValue("thirdFocus"));

            var secondSpell = Player.GetSpell(secondFocusSlot);
            var thirdSpell = Player.GetSpell(thirdFocusSlot);

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(firstFocusSlot))
            {
                if (!secondSpell.IsLearned)
                {
                    Player.Instance.Spellbook.LevelSpell(secondFocusSlot);
                }
                if (!thirdSpell.IsLearned)
                {
                    Player.Instance.Spellbook.LevelSpell(thirdFocusSlot);
                }
                Player.Instance.Spellbook.LevelSpell(firstFocusSlot);
            }

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(secondFocusSlot))
            {
                if (!thirdSpell.IsLearned)
                {
                    Player.Instance.Spellbook.LevelSpell(thirdFocusSlot);
                }
                Player.Instance.Spellbook.LevelSpell(firstFocusSlot);
                Player.Instance.Spellbook.LevelSpell(secondFocusSlot);
            }

            if (Player.Instance.Spellbook.CanSpellBeUpgraded(thirdFocusSlot))
            {
                Player.Instance.Spellbook.LevelSpell(thirdFocusSlot);
            }
        }

        #endregion Unit Level Up Function

        #region Combobox Value into Spellslot Function

        /// It will transform the value of the combobox into a SpellSlot
        private static SpellSlot GetSlotFromComboBox(this int value)
        {
            switch (value)
            {
                case 0:
                    return SpellSlot.Q;
                case 1:
                    return SpellSlot.W;
                case 2:
                    return SpellSlot.E;
            }
            Chat.Print("Failed getting slot");
            return SpellSlot.Unknown;
        }

        #endregion Combobox Value into Spellslot Function

        #region Combo Function

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

#endregion Combo Function
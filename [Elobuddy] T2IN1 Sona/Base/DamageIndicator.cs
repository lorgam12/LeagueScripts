﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                            //
//           Credits to MarioGK for his Lib & Template also Credits to Joker for Parts of his Lib             //
//                                                                                                            //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using SharpDX.Direct3D9;
using T2IN1_Lib;
using Color = System.Drawing.Color;
using Line = EloBuddy.SDK.Rendering.Line;

namespace T2IN1_Sona.Base
{
    internal class DamageIndicator
    {
        //Offsets
        private const float YOff = 9.8f;
        private const float XOff = 0;
        private const float Width = 107;
        private const float Thick = 9.82f;
        //Offsets
        private static Font _font, _font2;

        public static void Init()
        {
            Drawing.OnEndScene += Drawing_OnEndScene;

            _font = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoi UI",
                    Height = 16,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.TrueType,
                    Quality = FontQuality.ClearType
                });

            _font2 = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Segoi UI",
                    Height = 11,
                    Weight = FontWeight.Bold,
                    OutputPrecision = FontPrecision.TrueType,
                    Quality = FontQuality.ClearType
                });
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            foreach (
                var enemy in
                EntityManager.Heroes.Enemies.Where(e => e.IsValid && e.IsHPBarRendered && (e.TotalShieldHealth() > 10))
            )
            {
                var damage = enemy.GetTotalDamage();

                if (Menus.DrawingsMenu.GetCheckBoxValue("damageDraw"))
                {
                    var dmgPer = (enemy.TotalShieldHealth() - damage > 0 ? enemy.TotalShieldHealth() - damage : 0)/
                                 enemy.TotalShieldMaxHealth();
                    var currentHpPer = enemy.TotalShieldHealth()/enemy.TotalShieldMaxHealth();
                    var initPoint = new Vector2((int) (enemy.HPBarPosition.X + XOff + dmgPer*Width),
                        (int) enemy.HPBarPosition.Y + YOff);
                    var endPoint = new Vector2((int) (enemy.HPBarPosition.X + XOff + currentHpPer*Width) + 1,
                        (int) enemy.HPBarPosition.Y + YOff);

                    var colour = Color.FromArgb(180, Menus.DamageIndicatorColorSlide.GetSystemColor());
                    Line.DrawLine(colour, Thick, initPoint, endPoint);
                }

                if (Menus.DrawingsMenu.GetCheckBoxValue("statDraw"))
                {
                    //Statistics
                    var posXStat = (int) enemy.HPBarPosition[0] - 46;
                    var posYStat = (int) enemy.HPBarPosition[1] + 12;
                    var mathStat = "- " + Math.Round(damage) + " / " +
                                   Math.Round(enemy.Health - damage);
                    _font2.DrawText(null, mathStat, posXStat, posYStat, Menus.DamageIndicatorColorSlide.GetSharpColor());
                }

                if (Menus.DrawingsMenu.GetCheckBoxValue("perDraw"))
                {
                    //Percent
                    var posXPer = (int) enemy.HPBarPosition[0] - 28;
                    var posYPer = (int) enemy.HPBarPosition[1];
                    _font.DrawText(null, string.Concat(Math.Ceiling((int) damage/enemy.TotalShieldHealth()*100), "%"),
                        posXPer, posYPer, Menus.DamageIndicatorColorSlide.GetSharpColor());
                }
            }
        }
    }
}
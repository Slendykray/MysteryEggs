using System;
using System.Collections.Generic;
using System.Text;

namespace MysteryEggs
{
    internal class Tokens
    {

        public static void Init()
        {
            Language.Add("GOLDEGG_NAME", "Golden Beetle Egg");
            Language.Add("GOLDEGG_PICKUP", $"Recruit a {UtilityText("Beetle Guard")}. Drops {DamageText("Gold")} on kill.");
            Language.Add("GOLDEGG_DESC", $"Every {UtilityText("30")} {StackText("-50%")} seconds summon a {UtilityText("Beetle Guard")}. Drops {DamageText("Gold chunks")} on kill. Can have up to {UtilityText("1")} {StackText("+1")}.");
            Language.Add("GOLDEGG_LORE", "goldddddddd");

            Language.Add("LUNAREGG_NAME", "Newt's Egg");
            Language.Add("LUNAREGG_PICKUP", $"Recruit a {UtilityText("Lemurian")}. Drops {UtilityText("Lunar coins")} on kill.");
            Language.Add("LUNAREGG_DESC", $"Every {UtilityText("10")} {StackText("-50%")} seconds summon a {UtilityText("Lemurian")}. Drops {UtilityText("Lunar coins")} on kill. Can have up to {UtilityText("1")} {StackText("+1")}.");
            Language.Add("LUNAREGG_LORE", "lunarrrrrr");


            string coinString = UtilityText("Lunar coins");
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("bubbet.bubbetsitems"))
            {
                coinString = VoidText("Void markers");;
            }


            Language.Add("VOIDEGG_NAME", "V01? 366");
            Language.Add("VOIDEGG_PICKUP", $"Recruit a {VoidText("Void ally")}. Drops {coinString} on kill. {VoidText("Corrupts all Golden Beetle Eggs")}.");
            Language.Add("VOIDEGG_DESC", $"Every {UtilityText("60")} {StackText("-50%")} seconds summon a {VoidText("Void ally")}. Drops {coinString} on kill. Can have up to {UtilityText("1")} {StackText("+1")}. {VoidText("Corrupts all Golden Beetle Eggs")}.");
            Language.Add("VOIDEGG_LORE", "voiddddd");
        }


        public static string UtilityText(string text)
        {
            return $"<style=cIsUtility>{text}</style>";
        }

        public static string StackText(string text)
        {
            return $"<style=cStack>({text} per stack)</style>";
        }

        public static string DamageText(string text)
        {
            return $"<style=cIsDamage>{text}</style>";
        }

        public static string VoidText(string text)
        {
            return $"<style=cIsVoid>{text}</style>";
        }


    }
}

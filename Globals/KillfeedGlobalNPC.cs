using Killfeed.UI;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Killfeed.Globals
{
    public class KillfeedGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            // Due to fuckery, I cant save these to dictonarys. Therefore I do this here instead of in
            // PreKill/OnKill, which would be preferable.
            if(npc.life <= 0)
            {
                // The NPC has died, lets save some variables.
                string npcName = npc.FullName;
                // The player who killed the NPC.
                string playerName = Main.player[projectile.owner].name;
                // The Damage Class of the projectile.
                string damageClass = GetDamageClassAsString(projectile.DamageType);

                // Set Info

                KillfeedUIManager.NpcName = npcName;
                KillfeedUIManager.PlayerName = playerName;
                KillfeedUIManager.DamageType = damageClass;
                KillfeedUIManager.UITimer = KillfeedUIManager.lengthToDisplay;
                KillfeedUIManager.TrueMelee = false;
                KillfeedUIManager.Critter = !npc.chaseable;
                KillfeedUIManager.ListsNeedUpdating = true;
                KillfeedNetworking.SendListData(Main.myPlayer);

            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            // Same as above but for true melee hits.
            if (npc.life <= 0)
            {
                // The NPC has died, lets save some variables.
                string npcName = npc.FullName;
                // The player who killed the NPC.
                string playerName = player.name;
                // The Damage Class of the projectile.
                string damageClass = GetDamageClassAsString(item.DamageType);
                // Set the fields
                KillfeedUIManager.NpcName = npcName;
                KillfeedUIManager.PlayerName = playerName;
                KillfeedUIManager.DamageType = damageClass;
                KillfeedUIManager.UITimer = KillfeedUIManager.lengthToDisplay;
                KillfeedUIManager.TrueMelee = true;
                KillfeedUIManager.Critter = !npc.chaseable;
                KillfeedUIManager.ListsNeedUpdating = true;
                KillfeedNetworking.SendListData(Main.myPlayer);
            }
        }

        public static string GetDamageClassAsString(DamageClass damageClass)
        {
            // This is what we will change and return.
            string textToReturn;

            // I would have liked to use a switch for this but it doesn't work with a class.
            // So unfortunately, if else chain is needed. Thankfully its rather short.

            // Mark it as null. This shouldnt ever happen really, but its just in case.
            if (damageClass == null)
                textToReturn = "null";
            else if (damageClass == DamageClass.Melee)
                textToReturn = "Melee";
            else if (damageClass == DamageClass.Ranged)
                textToReturn = "Ranged";
            else if (damageClass == DamageClass.Magic || damageClass == DamageClass.MagicSummonHybrid)
                textToReturn = "Magic";
            else if (damageClass == DamageClass.Summon || damageClass == DamageClass.SummonMeleeSpeed)
                textToReturn = "Summoner";       
            else
                textToReturn = "Generic";

            // If calamity is enabled
            if(Killfeed.Calamity != null)
            {
                // If the damage class is rogue
                if (damageClass == Killfeed.Calamity.Find<DamageClass>("RogueDamageClass"))
                {
                    textToReturn = "Rogue";
                }
            }
            return textToReturn;
        }
        public override bool InstancePerEntity => true;
    }
}
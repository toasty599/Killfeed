using Killfeed.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Killfeed
{
	public class Killfeed : Mod
	{
        // Calamity support.
		public static Mod Calamity { get; private set; }
        public static Killfeed Instance { get; private set; }
        // Create a static instance of our UI.
        internal static KillfeedUIManager killfeedUIManager = new();
        public override void Load()
        {
            // If calamity is loaded, set 'Calamity' to it.
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                Calamity = calamity;
            }
            // Load our fonts
            FontAndColorManager.LoadFonts();
            FontAndColorManager.LoadColors();
            Instance = this;
        }
        public override void Unload()
        {
            // Unload Mods.
            Calamity = null;
            Instance = null;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            // When reciving a packet, run this.
            KillfeedNetworking.RecieveListData(this,reader, whoAmI);
        }
    }
    // This doesnt currently work properly.
    public static class KillfeedNetworking
    {
        public static void SendListData(int fromWho)
        {

            // We don't need to run this on singleplayer.
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                // Create a packet
                ModPacket packet = Killfeed.Instance.GetPacket();
                packet.Write((byte)0); // Give this the identifier of 0.
                if (Main.netMode == NetmodeID.Server)
                {
                    packet.Write(fromWho);
                }
                packet.Write(KillfeedUIManager.ListsNeedUpdating);
                packet.Write(KillfeedUIManager.NpcName);
                packet.Write(KillfeedUIManager.PlayerName);
                packet.Write(KillfeedUIManager.DamageType);
                packet.Write(KillfeedUIManager.UITimer);
                packet.Write(KillfeedUIManager.TrueMelee);
                packet.Write(KillfeedUIManager.Critter);
                packet.Send(-1);
            }
        }
        public static void RecieveListData(this Mod mod, BinaryReader reader, int fromWho)
        {
            // Get the identifier.
            byte msgType = reader.ReadByte();
            switch (msgType)
            {
                case 0: // If the identifier is 0.
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        fromWho = reader.ReadInt32();
                    }
                    KillfeedUIManager.ListsNeedUpdating = reader.ReadBoolean();
                    KillfeedUIManager.NpcName = reader.ReadString();
                    KillfeedUIManager.PlayerName = reader.ReadString();
                    KillfeedUIManager.DamageType = reader.ReadString();
                    KillfeedUIManager.UITimer = reader.ReadInt32();
                    KillfeedUIManager.TrueMelee = reader.ReadBoolean();
                    KillfeedUIManager.Critter = reader.ReadBoolean();
                    if(Main.netMode == NetmodeID.Server)
                    {
                        SendListData(fromWho);
                    }
                    else
                    {
                        //KillfeedUIManager.ListsNeedUpdating = LNU;
                        //KillfeedUIManager.NpcName = NN;
                        //KillfeedUIManager.PlayerName = PN;
                        //KillfeedUIManager.DamageType = DT;
                        //KillfeedUIManager.UITimer = UIT;
                        //KillfeedUIManager.TrueMelee = TM;
                        //KillfeedUIManager.Critter = C;
                    }
                    break;
            }
        }
        //bool LNU = reader.ReadBoolean();
        //string NN = reader.ReadString();
        //string PN = reader.ReadString();
        //string DT = reader.ReadString();
        //int UIT = reader.ReadInt32();
        //bool TM = reader.ReadBoolean();
        //bool C = reader.ReadBoolean();

    }
}
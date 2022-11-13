using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ID;

namespace Killfeed.UI
{
    public class FontAndColorManager
    {
        // Fonts
        public static DynamicSpriteFont CurrentFont { get; internal set; }

        public static DynamicSpriteFont FortniteFont { get; private set; }
        public static DynamicSpriteFont TF2Font { get; private set; }
        public static DynamicSpriteFont CSGOFont { get; private set; }
        public static DynamicSpriteFont XFont { get; private set; }

        public static bool useWhiteBackgroundForTF2 { get; internal set; }

        // Colors
        // Order: Player, Default, Enemy, Teammate/Critter

        internal static Color[] CurrentColors;

        internal readonly static Color[] FortniteColors = { new Color(70, 197, 117), new Color(204, 204, 204), new Color(194, 87, 79), new Color(72, 137, 175) };
        internal readonly static Color[] TF2Colors = { new Color(85, 124, 131), new Color(61, 57, 55), new Color(163, 87, 74), new Color(85, 124, 131) };
        internal readonly static Color[] CSGOColors = { new Color(79, 158, 222), new Color(255, 255, 255), new Color(199, 162, 71), new Color(79, 158, 222) };
        internal readonly static Color[] XColors = { new Color(114, 246, 251), new Color(229, 224, 228), new Color(231, 39, 52), new Color(114, 246, 251) };
        internal readonly static Color[] BaseColors = { new Color(32, 206, 69), new Color(230, 230, 230), new Color(207, 48, 32), new Color(93, 145, 244)};

        public readonly static Color tf2ColorWhite = new(241, 233, 203);
        public readonly static Color tf2ColorNonWhite = new(61, 57, 35);
        public static void LoadFonts()
        {
            // Dont do this on a dedicated server.
            if (!Main.dedServ)
            {
                // This was crashing on Linux and such in Calamity, i am unable to check if it will here so I am playing it safe and only allowing custom fonts to work on Windows.
                if ((int)Environment.OSVersion.Platform == 2)
                {
                    FortniteFont = ModContent.Request<DynamicSpriteFont>("Killfeed/Fonts/KillfeedFontF", (AssetRequestMode)1).Value;
                    TF2Font = ModContent.Request<DynamicSpriteFont>("Killfeed/Fonts/KillfeedFontV", (AssetRequestMode)1).Value;
                    CSGOFont = ModContent.Request<DynamicSpriteFont>("Killfeed/Fonts/KillfeedFontR", (AssetRequestMode)1).Value;
                    XFont = ModContent.Request<DynamicSpriteFont>("Killfeed/Fonts/KillfeedFontX", (AssetRequestMode)1).Value;
                }
                else
                {
                    // If not the correct OS, we need to make it the default Terraria Font, Andy.
                    FortniteFont = FontAssets.MouseText.Value;
                    TF2Font = FontAssets.MouseText.Value;
                    CSGOFont = FontAssets.MouseText.Value;
                    XFont = FontAssets.MouseText.Value;
                }
                KillfeedConfig config = ModContent.GetInstance<KillfeedConfig>();
                switch (config.FontUsed)
                {
                    case "Fortnite Font":
                        CurrentFont = FortniteFont;
                        break;
                    case "TF2 Font":
                        CurrentFont = TF2Font;
                        break;
                    case "CSGO Font":
                        CurrentFont = CSGOFont;
                        break;
                    case "Futuristic Font":
                        CurrentFont = XFont;
                        break;
                    case "Default Font":
                        CurrentFont = FontAssets.MouseText.Value;
                        break;
                }
                useWhiteBackgroundForTF2 = config.TF2WhiteBackground;
            }
        }
        public static void LoadColors()
        {
            // Dont do this on a dedicated server.
            if (!Main.dedServ)
            {
                KillfeedConfig config = ModContent.GetInstance<KillfeedConfig>();
                switch (config.ColorUsed)
                { 
                    case "Fortnite Colors":
                        CurrentColors = FortniteColors;
                        break;
                    case "TF2 Colors":
                        CurrentColors = TF2Colors;
                        break;
                    case "CSGO Colors":
                        CurrentColors = CSGOColors;
                        break;
                    case "Futuristic Colors":
                        CurrentColors = XColors;
                        break;
                    case "Default Colors":
                        CurrentColors = BaseColors;
                        break;
                }
                if (config.TF2WhiteBackground)
                    TF2Colors[1] = tf2ColorWhite;
                else
                    TF2Colors[1] = tf2ColorNonWhite;
            }
        }

    }
}

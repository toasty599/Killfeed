using Killfeed.UI;
using System.ComponentModel;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Killfeed
{
    public class KillfeedConfig : ModConfig
    {

        [OptionStrings(new string[] { "Fortnite Font", "TF2 Font", "CSGO Font", "Futuristic Font", "Default Font" })]
        [DefaultValue("Fortnite Font")]
        [Tooltip("The font used")]
        [Label("Font")]
        [DrawTicks]
        public string FontUsed;

        [OptionStrings(new string[] { "Fortnite Colors", "TF2 Colors", "CSGO Colors", "Futuristic Colors", "Default Colors"})]
        [DefaultValue("Fortnite Colors")]
        [Tooltip("The color scheme used")]
        [Label("Color")]
        [DrawTicks]
        public string ColorUsed;

        
        [DefaultValue(false)]
        [Tooltip("Show the white background for the TF2 color scheme")]
        [Label("TF2 White Background")]
        public bool TF2WhiteBackground;

        // This is for client side only.
        public override ConfigScope Mode => ConfigScope.ClientSide;

        // Constructor
        public KillfeedConfig()
        {
            FontUsed = "Fortnite Font";
            ColorUsed = "Fortnite Colors";
            TF2WhiteBackground = false;
        }
        public override void OnChanged()
        {
            KillfeedConfig killfeedConfig = ModContent.GetInstance<KillfeedConfig>();
            if (killfeedConfig != null)
            {
                switch (FontUsed)
                {
                    case "Fortnite Font":
                        FontAndColorManager.CurrentFont = FontAndColorManager.FortniteFont;
                        break;
                    case "TF2 Font":
                        FontAndColorManager.CurrentFont = FontAndColorManager.TF2Font;
                        break;
                    case "CSGO Font":
                        FontAndColorManager.CurrentFont = FontAndColorManager.CSGOFont;
                        break;
                    case "Futuristic Font":
                        FontAndColorManager.CurrentFont = FontAndColorManager.XFont;
                        break;
                    case "Default Font":
                        FontAndColorManager.CurrentFont = FontAssets.MouseText.Value;
                        break;
                }
                switch (ColorUsed)
                {
                    case "Fortnite Colors":
                        FontAndColorManager.CurrentColors = FontAndColorManager.FortniteColors;
                        break;
                    case "TF2 Colors":
                        FontAndColorManager.CurrentColors = FontAndColorManager.TF2Colors;
                        break;
                    case "CSGO Colors":
                        FontAndColorManager.CurrentColors = FontAndColorManager.CSGOColors;
                        break;
                    case "Futuristic Colors":
                        FontAndColorManager.CurrentColors = FontAndColorManager.XColors;
                        break;
                    case "Default Colors":
                        FontAndColorManager.CurrentColors = FontAndColorManager.BaseColors;
                        break;
                }
                FontAndColorManager.useWhiteBackgroundForTF2 = TF2WhiteBackground;
                if (FontAndColorManager.useWhiteBackgroundForTF2)
                    FontAndColorManager.TF2Colors[1] = FontAndColorManager.tf2ColorNonWhite;
                else
                    FontAndColorManager.TF2Colors[1] = FontAndColorManager.tf2ColorWhite;
            }
        }
    }
}

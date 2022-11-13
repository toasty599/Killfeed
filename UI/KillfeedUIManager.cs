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
    public class KillfeedUIManager
    {
        // Could potentially remake this using structs, mass lists isnt very pretty or practical.

        // These store the latest NPC to add, adding them directly to the list would be ideal but due to global instancing you cannot do that,
        // and this is the next best way i could find.
        internal static bool ListsNeedUpdating = false;
        internal static string NpcName;
        internal static string PlayerName;
        internal static string DamageType;
        internal static int UITimer;
        internal static bool TrueMelee;
        internal static bool Critter;

        private static List<string> NPCNameList = new() { };

        private static List<string> PlayerNameList = new() { };

        private static List<string> DamageTypeList = new() { };

        private static List<int> UITimerList = new() { };

        private static List<bool> TrueMeleeList = new() { };

        private static List<bool> CritterList = new() { };

        #region RandomLists
        private static List<string> wordToUse = new() { };

        private static List<string> textToDraw = new() { };

        private static readonly List<string> MeleeQuotesList = new()
        {
            "stabbed","impaled","killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> RangedQuotesList = new()
        {
            "shot","lit up","killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> MagicQuotesList = new()
        {
            "vaporized","disintegrated", "killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> SummonerQuotesList = new()
        {
            "ganged up on","bullied", "killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> TrueMeleeQuotesList = new()
        {
            "bludgeoned","sliced", "killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> RogueQuotesList = new()
        {
            "severed","eviscerated","killed","defeated","eliminated","obliterated",
        };
        private static readonly List<string> MiscQuotesList = new()
        {
            "killed","defeated","eliminated","obliterated",
        };
        #endregion

        // Timer stuff
        public const int lengthToDisplay = 240;

        // Draw Pos, don't really need to be here, byeah.
        private static float baseDrawPosX = 0;
        private static float baseDrawPosY = 0;

        private static void UpdateLists()
        {
            // I hate this...

            // If we need to update the lists.
            if (ListsNeedUpdating)
            {
                // Set this to false so things are only added one time.
                ListsNeedUpdating = false;
                // If we have less than 5 things in the list (they all get things added and removed together so should never be out of sync.
                if (NPCNameList.Count < 5)
                {
                    NPCNameList.Add(NpcName);
                    PlayerNameList.Add(PlayerName);
                    DamageTypeList.Add(DamageType);
                    UITimerList.Add(lengthToDisplay);
                    TrueMeleeList.Add(TrueMelee);
                    CritterList.Add(Critter);
                    List<string> list = DamageType switch
                    {
                        "Melee" => MeleeQuotesList,
                        "Ranged" => RangedQuotesList,
                        "Magic" => MagicQuotesList,
                        "Summoner" => SummonerQuotesList,
                        "Rogue" => RogueQuotesList,
                        _ => MiscQuotesList,
                    };

                    // If it was a true melee kill, use this list.
                    if (TrueMelee)
                    {
                        list = TrueMeleeQuotesList;
                    }

                    // Pick a random string from the list, and add spaces before and after for formatting.
                    int j = Main.rand.Next(0, list.Count);
                    wordToUse.Add(" " + list[j] + " ");

                    // The final, full text to show. This is not used when drawing, that is created later on to
                    // allow for proper coloring.
                    textToDraw.Add($"{PlayerName}{" " + list[j] + " "}{NpcName}");

                }
                else // Else if we do, clear the first one and add another to the end.
                {
                    NPCNameList.RemoveAt(0);
                    PlayerNameList.RemoveAt(0);
                    DamageTypeList.RemoveAt(0);
                    UITimerList.RemoveAt(0);
                    TrueMeleeList.RemoveAt(0);
                    CritterList.RemoveAt(0);
                    wordToUse.RemoveAt(0);
                    textToDraw.RemoveAt(0);

                    NPCNameList.Add(NpcName);
                    PlayerNameList.Add(PlayerName);
                    DamageTypeList.Add(DamageType);
                    UITimerList.Add(lengthToDisplay);
                    TrueMeleeList.Add(TrueMelee);
                    CritterList.Add(Critter);
                    List<string> list = DamageType switch
                    {
                        "Melee" => MeleeQuotesList,
                        "Ranged" => RangedQuotesList,
                        "Magic" => MagicQuotesList,
                        "Summoner" => SummonerQuotesList,
                        "Rogue" => RogueQuotesList,
                        _ => MiscQuotesList,
                    };

                    // If it was a true melee kill, use this list.
                    if (TrueMelee)
                    {
                        list = TrueMeleeQuotesList;
                    }

                    // Pick a random string from the list, and add spaces before and after for formatting.
                    int j = Main.rand.Next(0, list.Count);
                    wordToUse.Add(" " + list[j] + " ");

                    // The final, full text to show. This is not used when drawing, that is created later on to
                    // allow for proper coloring.
                    textToDraw.Add($"{PlayerName}{" " + list[j] + " "}{NpcName}");
                }
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            
            // If we are a server, fuck off.
            if (Main.netMode == NetmodeID.Server)
                return;
            // Update our lists.
            UpdateLists();
            // Loop through all of the amounts in the list.
            for (int i = 0; i < NPCNameList.Count; i++)
            {
                // Get the data from the list for the current one, i.
                string nPCName = NPCNameList[i];
                string playerName = PlayerNameList[i];
                int displayTimer = UITimerList[i];
                bool critter = CritterList[i];

                // Decrease the timer.
                if (displayTimer > 0)
                    UITimerList[i]--;

                // Reset stuff if the timer is 0
                if (displayTimer == 0)
                {
                    NPCNameList.RemoveAt(i);
                    PlayerNameList.RemoveAt(i);
                    DamageTypeList.RemoveAt(i);
                    UITimerList.RemoveAt(i);
                    TrueMeleeList.RemoveAt(i);
                    CritterList.RemoveAt(i);
                    wordToUse.RemoveAt(i);
                    textToDraw.RemoveAt(i);
                }

                // If in the Inventory or the timer is 0, leave.
                if (Main.playerInventory || displayTimer == 0)
                    return;                          

                // The scale of the text, and background.
                float scale = 0.4f;
                // If we aren't using a custom font, leave the scale as 1.
                if (FontAndColorManager.CurrentFont == FontAssets.MouseText.Value)
                    scale = 1;

                // Get the size of the text.
                Vector2 size = FontAndColorManager.CurrentFont.MeasureString(textToDraw[i]);

                // Get the background texture.
                Texture2D BackgroundTexture;
                // Check if we should change to the white tf2 background.
                bool tf2Background = FontAndColorManager.CurrentColors == FontAndColorManager.TF2Colors && playerName == Main.LocalPlayer.name && FontAndColorManager.useWhiteBackgroundForTF2;
                if (tf2Background)
                    BackgroundTexture = ModContent.Request<Texture2D>("Killfeed/UI/Textures/backgroundtf2t", (AssetRequestMode)2).Value;
                else
                    BackgroundTexture = ModContent.Request<Texture2D>("Killfeed/UI/Textures/background", (AssetRequestMode)2).Value;

                // Get the size of the background texture.
                Vector2 backgroundSize = BackgroundTexture.Size();

                // Modify the height based on the font in use.
                int yOffset = 275;
                if (FontAndColorManager.CurrentFont == FontAndColorManager.FortniteFont)
                    backgroundSize.Y += 1f; // 50, 51
                else if (FontAndColorManager.CurrentFont == FontAndColorManager.TF2Font)
                    backgroundSize.Y += 1.1f;
                else if(FontAndColorManager.CurrentFont == FontAndColorManager.CSGOFont)
                    backgroundSize.Y += 10f;
                else if(FontAndColorManager.CurrentFont == FontAndColorManager.XFont)
                    backgroundSize.Y += 1f; // 50, 51
                else if(FontAndColorManager.CurrentFont == FontAssets.MouseText.Value)
                    backgroundSize.Y += 13f; // 50, 51

                // Set the correct draw positions. This perfectly places it above the chat, unfortunately you are unable
                // to check if anything is being drawn onscreen from the chat, only if the chat is open.
                baseDrawPosX = 90;
                baseDrawPosY = Main.screenHeight - (size.Y * scale) * (NPCNameList.Count-(i + 1)) - yOffset;

                // Fade out for the last 0.5 seconds.
                float opacity = 1;
                if (displayTimer <= 30)
                    opacity = (float)displayTimer / 30;
                
                // Set the base position to draw. By using Main.screenHeight for it, it will scale properly.
                Vector2 baseDrawPos = new(baseDrawPosX, baseDrawPosY);

                // Give the background the same position, but change the X slightly so it is larger.
                Vector2 baseBackgroundDrawPos = baseDrawPos;
                baseBackgroundDrawPos.X -= 3.5f;
                
                // Draw the background texture. Using the size in the scaling of it ensures it properly streches to the desired length based on the string's length.
                spriteBatch.Draw(BackgroundTexture, baseBackgroundDrawPos, null, (tf2Background ? Color.White*opacity : Color.Black *(0.45f* opacity)), 0, Vector2.Zero, size / backgroundSize*(scale+0.01f+(FontAndColorManager.CurrentFont == FontAssets.MouseText.Value ? 0.02f:0)), 0, 0);

                // This is for the color formatting.
                // Create two arrays, one for the text and one for the color. This is why we added the spaces to the text earlier. These are synced similar to the lists.
                string[] stringPieces = { playerName, wordToUse[i], nPCName };
                // Load the currently selected colors.
                Color baseColor = FontAndColorManager.CurrentColors[1];
                Color meColor = FontAndColorManager.CurrentColors[0];
                Color enemyColor = FontAndColorManager.CurrentColors[2];
                Color notMeColor = FontAndColorManager.CurrentColors[3];

                // We also change the color of the npc name to indicate whether it was harmless or not, along with checking if the player name matches ours.
                Color[] colors = { playerName == Main.LocalPlayer.name ? meColor : notMeColor, baseColor, critter ? notMeColor : enemyColor };

                // The offset starts at 0.
                Vector2 offset = Vector2.Zero;
                
                // Loop through each piece of the string, 
                // drawing each with a different color,
                // and moving the offset forward by its measured length multiplied by the scale.
                // Yes, I found this on stack overflow.
                for (int x = 0; x < stringPieces.Length; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAndColorManager.CurrentFont, stringPieces[x], baseDrawPos + offset, colors[x]*opacity,0,Vector2.Zero,scale,0,0);
                    offset.X += FontAndColorManager.CurrentFont.MeasureString(stringPieces[x]).X*scale;
                }
            }
        }
    }
    // This would ideally be in its own file, however due to the small size of this mod, and the fact im lazy, it is here.
    public class KillfeedUIWorld : ModSystem
    {
        // Responsible for making our custom UI draw.
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            // Always put this. Doesn't work else.
            int mouseIndex = layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex == -1)
                return;

            // You dont need to understand delegates to do this, although it helps.
            GameInterfaceDrawMethod val = delegate
            {
                // Put any checks+draw calls in here.
                // 'if(Main.playerInventory)' would run only if you are in the inventory.

                // I check whether to draw it in the Draw method itself, so i can still change timers etc while it isnt drawing.
                KillfeedUIManager.Draw(Main.spriteBatch);
                // Always return true.
                return true;
            };
            // This gives your layer a name.
            object obj = val;
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Killfeed UI", (GameInterfaceDrawMethod)obj, (InterfaceScaleType)1));
        }

        // This is needed to make the UI actually appear. Why? Good question.
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
        }

    }
}

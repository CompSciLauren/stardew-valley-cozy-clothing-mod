using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CozyClothing
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;

        private int previousShirt;
        private int previousPantStyle;
        private Color previousPantsColor;
        private int previousShoeColor;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        /// <summary>Raised after the save file is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ChangeIntoPajamas();

            Helper.Events.Player.Warped += OnWarped;
        }

        /// <summary>Raised after the player enters a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation is Farm && e.OldLocation is StardewValley.Locations.FarmHouse)
            {
                // Change out of pajamas and back into previous clothes
                Game1.player.changeShirt(previousShirt);
                Game1.player.changePantStyle(previousPantStyle);
                Game1.player.changePants(previousPantsColor);
                Game1.player.changeShoeColor(previousShoeColor);
            }
            else if (e.NewLocation is StardewValley.Locations.FarmHouse)
            {
                ChangeIntoPajamas();
            }
        }

        private void ChangeIntoPajamas()
        {
            // save current clothes to change back into later
            previousShirt = Game1.player.shirt.Value;
            previousPantStyle = Game1.player.pants.Value;
            previousPantsColor = Game1.player.pantsColor;
            previousShoeColor = Game1.player.shoes.Value;

            // change current clothes to be pajamas
            Game1.player.changePantStyle(0);

            switch (Config.PajamaColor)
            {
                case "Pink":
                    Game1.player.changeShirt(36);
                    Game1.player.changePants(Color.PaleVioletRed);
                    Game1.player.changeShoeColor(4);
                    break;
                case "Purple":
                    Game1.player.changeShirt(40);
                    Game1.player.changePants(Color.MediumPurple);
                    Game1.player.changeShoeColor(4);
                    break;
                case "Green":
                    Game1.player.changeShirt(96);
                    Game1.player.changePants(Color.LimeGreen);
                    Game1.player.changeShoeColor(4);
                    break;
                case "Water-Blue":
                    Game1.player.changeShirt(105);
                    Game1.player.changePants(Color.RoyalBlue);
                    Game1.player.changeShoeColor(4);
                    break;
                case "Blue":
                default:
                    Game1.player.changeShirt(9);
                    Game1.player.changePants(Color.DarkTurquoise);
                    Game1.player.changeShoeColor(6);
                    break;
            }
        }
    }
}

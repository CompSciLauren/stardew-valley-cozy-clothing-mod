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

        private bool currentlyInPajamas = false;

        // previous clothes
        private string previousShirt;
        private string previousPantStyle;
        private Color previousPantsColor;
        private string previousShoeColor;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
        }

        /// <summary>Raised after the save file is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            Helper.Events.Player.Warped += OnWarped;
            Helper.Events.GameLoop.DayEnding += OnDayEnding;
        }

        /// <summary>Raised after the player returns to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            Helper.Events.GameLoop.DayStarted -= OnDayStarted;
            Helper.Events.Player.Warped -= OnWarped;
            Helper.Events.GameLoop.DayEnding -= OnDayEnding;

            if (currentlyInPajamas)
            {
                ChangeIntoRegularClothes();
            }
        }

        /// <summary>Raised after the day has started.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            if (Game1.currentLocation is StardewValley.Locations.FarmHouse)
            {
                if (IsWeddingScheduledForToday() && currentlyInPajamas)
                {
                    ChangeIntoRegularClothes();
                } else if (!IsWeddingScheduledForToday() && !currentlyInPajamas)
                {
                    ChangeIntoPajamas();
                }
            }
        }

        /// <summary>Raised after the day is ending.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            if (currentlyInPajamas && Game1.currentLocation is StardewValley.Locations.FarmHouse)
            {
                ChangeIntoRegularClothes();
            }
        }

        /// <summary>Raised after the player enters a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation is Farm && e.OldLocation is StardewValley.Locations.FarmHouse && currentlyInPajamas)
            {
                ChangeIntoRegularClothes();
            }
            else if (e.NewLocation is StardewValley.Locations.FarmHouse && e.OldLocation is Farm && !currentlyInPajamas)
            {
                ChangeIntoPajamas();
            }
        }

        /// <summary>Removes pajamas and replaces them with previously worn clothes.</summary>
        private void ChangeIntoRegularClothes()
        {
            // Change out of pajamas and back into previous clothes
            Game1.player.changeShirt(previousShirt);
            Game1.player.changePantStyle(previousPantStyle);
            Game1.player.changePantsColor(previousPantsColor);
            Game1.player.changeShoeColor(previousShoeColor);

            currentlyInPajamas = false;
        }

        /// <summary>Removes current clothes and replaces them with pajamas.</summary>
        private void ChangeIntoPajamas()
        {
            // save current clothes to change back into later
            previousShirt = Game1.player.shirt.Value;
            previousPantStyle = Game1.player.pants.Value;
            previousPantsColor = Game1.player.pantsColor.Value;
            previousShoeColor = Game1.player.shoes.Value;

            // change current clothes to be pajamas
            Game1.player.changePantStyle("0");
            Game1.player.changeShoeColor("4");

            switch (Config.PajamaColor)
            {
                case "Pink":
                    Game1.player.changeShirt("1036");
                    Game1.player.changePantsColor(Color.PaleVioletRed);
                    break;
                case "Purple":
                    Game1.player.changeShirt("1040");
                    Game1.player.changePantsColor(Color.MediumPurple);
                    break;
                case "Green":
                    Game1.player.changeShirt("1096");
                    Game1.player.changePantsColor(Color.LimeGreen);
                    break;
                case "Water-Blue":
                    Game1.player.changeShirt("1105");
                    Game1.player.changePantsColor(Color.RoyalBlue);
                    break;
                case "Blue":
                default:
                    Game1.player.changeShirt("1009");
                    Game1.player.changePantsColor(Color.DarkTurquoise);
                    Game1.player.changeShoeColor("6");
                    break;
            }

            currentlyInPajamas = true;
        }

        /// <summary>Checks if a wedding is occurring.</summary>
        /// <returns>True if a wedding is occurring, false otherwise.</returns>
        private bool IsWeddingScheduledForToday()
        {
            if (Game1.CurrentEvent is not null && Game1.CurrentEvent.isWedding)
            {
                return true;
            }
            
            return false;
        }
    }
}

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CozyClothing
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private int previousShirt;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        /// <summary>Raised after the save file is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            ChangeShirtToPajamaShirt();

            Helper.Events.Player.Warped += OnWarped;
        }

        /// <summary>Raised after the player enters a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (e.NewLocation is Farm && e.OldLocation is StardewValley.Locations.FarmHouse)
            {
                // Change out of pajama shirt and back into previous shirt
                Game1.player.changeShirt(previousShirt);
            }
            else if (e.NewLocation is StardewValley.Locations.FarmHouse)
            {
                ChangeShirtToPajamaShirt();
            }
        }

        private void ChangeShirtToPajamaShirt()
        {
            // save current shirt to change back into later
            previousShirt = Game1.player.shirt.Value;
            // change current shirt to be pajama shirt
            Game1.player.changeShirt(10);
        }
    }
}

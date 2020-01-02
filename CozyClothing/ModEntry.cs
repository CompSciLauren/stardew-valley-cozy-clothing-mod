using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace CozyClothing
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private int previousShirt;
        private int previousPantStyle;
        private Color previousPantsColor;
        private int previousShoeColor;

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
                // Change out of pajamas and back into previous clothes
                Game1.player.changeShirt(previousShirt);
                Game1.player.changePantStyle(previousPantStyle);
                Game1.player.changePants(previousPantsColor);
                Game1.player.changeShoeColor(previousShoeColor);
            }
            else if (e.NewLocation is StardewValley.Locations.FarmHouse)
            {
                ChangeShirtToPajamaShirt();
            }
        }

        private void ChangeShirtToPajamaShirt()
        {
            // save current clothes to change back into later
            previousShirt = Game1.player.shirt.Value;
            previousPantStyle = Game1.player.pants.Value;
            previousPantsColor = Game1.player.pantsColor;
            previousShoeColor = Game1.player.shoes.Value;

            // change current clothes to be pajamas
            Game1.player.changeShirt(9);
            Game1.player.changePantStyle(0);
            Game1.player.changePants(Color.DarkTurquoise);
            Game1.player.changeShoeColor(6);
        }
    }
}

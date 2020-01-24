using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;

namespace CozyClothing
{
    public interface IJsonAssetsApi
    {
        void LoadAssets(string path);
        int GetHatId(string name);
        int GetClothingId(string name);
        IDictionary<string, int> GetAllHatIds();
        IDictionary<string, int> GetAllClothingIds();
    }

    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        internal static IJsonAssetsApi JsonAssets;

        /// <summary>The mod configuration from the player.</summary>
        private ModConfig Config;

        private bool currentlyInPajamas = false;

        // previous clothes
        private StardewValley.Objects.Hat previousHat;
        private StardewValley.Objects.Clothing previousShirt;
        private StardewValley.Objects.Clothing previousPants;
        private int previousShoeColor;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            JsonAssets = Helper.ModRegistry.GetApi<IJsonAssetsApi>("spacechase0.JsonAssets");
            if (JsonAssets == null)
            {
                Monitor.Log("Can't access the Json Assets API. Is the mod installed correctly?");
                return;
            }
        }

        /// <summary>Raised after the save file is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            //Monitor.Log("CLOTHING IDS: ", LogLevel.Debug);
            //foreach (var id in JsonAssets.GetAllClothingIds())
            //    Monitor.Log($"{id.Key}: {id.Value}",
            //        LogLevel.Debug);

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
            if (!currentlyInPajamas)
            {
                ChangeIntoPajamas();
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
            if (!(e.NewLocation is StardewValley.Locations.Cellar) && !(e.NewLocation is StardewValley.Locations.FarmHouse) && (e.OldLocation is StardewValley.Locations.FarmHouse || e.OldLocation is StardewValley.Locations.Cellar) && currentlyInPajamas)
            {
                ChangeIntoRegularClothes();
            }
            else if (e.NewLocation is StardewValley.Locations.FarmHouse && !currentlyInPajamas)
            {
                ChangeIntoPajamas();
            }
        }

        /// <summary>Removes pajamas and replaces them with previously worn clothes.</summary>
        private void ChangeIntoRegularClothes()
        {
            // Change out of pajamas and back into previous clothes
            Game1.player.hat.Set(previousHat);
            Game1.player.shirtItem.Set(previousShirt);
            Game1.player.pantsItem.Set(previousPants);
            Game1.player.changeShoeColor(previousShoeColor);

            currentlyInPajamas = false;
        }

        /// <summary>Removes current clothes and replaces them with pajamas.</summary>
        private void ChangeIntoPajamas()
        {
            // save current clothes to change back into later
            previousHat = Game1.player.hat.Value;
            previousShirt = Game1.player.shirtItem.Value;
            previousPants = Game1.player.pantsItem.Value;
            previousShoeColor = Game1.player.shoes.Value;

            // change current clothes to be pajamas
            switch (Config.PajamaColor)
            {
                case "Bear Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Bear Onesie Hat"), JsonAssets.GetClothingId("Bear Onesie Shirt"), JsonAssets.GetClothingId("Bear Onesie Pants"));
                    break;
                case "Blue Chicken Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Blue Chicken Onesie Hat"), JsonAssets.GetClothingId("Blue Chicken Onesie Shirt"), JsonAssets.GetClothingId("Blue Chicken Onesie Pants"));
                    break;
                case "Dog Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Dog Onesie Hat"), JsonAssets.GetClothingId("Dog Onesie Shirt"), JsonAssets.GetClothingId("Dog Onesie Pants"));
                    break;
                case "Elephant Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Elephant Onesie Hat"), JsonAssets.GetClothingId("Elephant Onesie Shirt"), JsonAssets.GetClothingId("Elephant Onesie Pants"));
                    break;
                case "Kitty Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Kitty Onesie Hat"), JsonAssets.GetClothingId("Kitty Onesie Shirt"), JsonAssets.GetClothingId("Kitty Onesie Pants"));
                    break;
                case "Panda Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Panda Onesie Hat"), JsonAssets.GetClothingId("Panda Bear Onesie Shirt"), JsonAssets.GetClothingId("Panda Bear Onesie Pants"));
                    break;
                case "Unicorn Onesie":
                    SetOnesiePajamas(JsonAssets.GetHatId("Unicorn Onesie Hat"), JsonAssets.GetClothingId("Unicorn Onesie Shirt"), JsonAssets.GetClothingId("Unicorn Onesie Pants"));
                    break;
                case "Pink":
                    Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(1036));
                    Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(0));
                    Game1.player.pantsItem.Value.clothesColor.Value = Color.PaleVioletRed;
                    Game1.player.changeShoeColor(4);
                    break;
                case "Purple":
                    Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(1040));
                    Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(0));
                    Game1.player.pantsItem.Value.clothesColor.Value = Color.MediumPurple;
                    Game1.player.changeShoeColor(4);
                    break;
                case "Green":
                    Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(1096));
                    Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(0));
                    Game1.player.pantsItem.Value.clothesColor.Value = Color.LimeGreen;
                    Game1.player.changeShoeColor(4);
                    break;
                case "Water-Blue":
                    Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(1105));
                    Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(0));
                    Game1.player.pantsItem.Value.clothesColor.Value = Color.RoyalBlue;
                    Game1.player.changeShoeColor(4);
                    break;
                case "Blue":
                default:
                    Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(1009));
                    Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(0));
                    Game1.player.pantsItem.Value.clothesColor.Value = Color.DarkTurquoise;
                    Game1.player.changeShoeColor(6);
                    break;
            }
            currentlyInPajamas = true;
        }

        /// <summary>Sets the pajama clothes.</summary>
        private void SetOnesiePajamas(int hatID, int shirtID, int pantsID)
        {
            Game1.player.hat.Set(new StardewValley.Objects.Hat(hatID));
            Game1.player.shirtItem.Set(new StardewValley.Objects.Clothing(shirtID));
            Game1.player.pantsItem.Set(new StardewValley.Objects.Clothing(pantsID));
        }
    }
}

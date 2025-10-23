using StardewModdingAPI;

namespace CozyClothing
{
    class ModConfig
    {
        public string PajamaColor { get; set; }

        public ModConfig()
        {
            PajamaColor = "Blue";
        }

        public void Reset()
        {
            PajamaColor = "Blue";
        }
    }
}
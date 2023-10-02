using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SCPSlayer
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Debug mode (methinks)")]
        public bool Debug { get; set; } = true;
        [Description("Number of lives per player")]
        public int Lives { get; set; } = 5;
        [Description("Radar distance")]
        public int radarDistance { get; set; } = 20;
        [Description("Minimum distance the game will try to spawn you in")]
        public int spawnDistance { get; set; } = 30;
    }
}

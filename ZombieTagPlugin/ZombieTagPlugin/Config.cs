using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ZombieTagPlugin
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Debug mode (methinks)")]
        public bool Debug { get; set; }

        [Description("Minimum distance limit")]
        public int minDistance { get; set; } = 20;
        [Description("Maximum distance limit")]
        public int maxDistance { get; set; } = 30;

        [Description("Whether or not players have a chance of spawning with previously spawned player (% chance 1-100)")]
        public int prevSpawn { get; set; } = 75;

        [Description("Whether or not to restrict the game to one zone.")]
        public bool zoneLockdown { get; set; } = true;
    }
}
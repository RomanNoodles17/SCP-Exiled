
using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Oddball
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Debug mode (methinks)")]
        public bool Debug { get; set; } = true;

        [Description("Grace period before round starts")]
        public int gracePeriod { get; set; } = 30;
        [Description("Radar distance")]
        public int radarDistance { get; set; } = 50;
        [Description("Float respawn time in seconds")]
        public float respawnTime { get; set; } = 5f;
        [Description("Score per players (capped at 3)")]
        public int scoreNeededPerPlayer { get; set; } = 300;
    }
}



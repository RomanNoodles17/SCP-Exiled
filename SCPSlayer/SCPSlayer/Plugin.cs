using Exiled.API.Enums;
using Exiled.API.Features;
using System;

namespace SCPSlayer
{
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin Singleton = new Plugin();
        public static Plugin Instance => Singleton;

        public override string Name => "Slayer";
        public override string Author => "Roman_Noodles";
        public override Version Version => new Version(1, 0, 0);

        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private MyServer server;

        public override void OnEnabled()
        {
            RegisterEvents();
        }
        public override void OnDisabled()
        {
            UnregisterEvents();
        }

        public void RegisterEvents()
        {
            server = new MyServer();

            Exiled.Events.Handlers.Server.RoundStarted += server.OnRoundStarted;
            //Server/Player.event += server/player.onevent;
        }
        public void UnregisterEvents()
        {
            //Server/Player.event += server/player.onevent;
            Exiled.Events.Handlers.Server.RoundStarted -= server.OnRoundStarted;

            server = null;
        }

    }
}

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.Linq;

namespace Oddball
{
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin Singleton = new Plugin();
        public static Plugin Instance => Singleton;

        public override string Name => "Oddball";
        public override string Author => "Roman_Noodles";
        public override Version Version => new Version(1, 0, 0);

        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private MyPlayer player;
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
            player = new MyPlayer();
            server = new MyServer();

            Exiled.Events.Handlers.Server.RoundStarted += server.OnRoundStarted;
            Exiled.Events.Handlers.Player.Died += player.OnDeath;
            Exiled.Events.Handlers.Player.InteractingDoor += player.OnInteractingDoor;
            Exiled.Events.Handlers.Player.Dying += player.OnDying;
            //Server/Player.event += server/player.onevent;
        }
        public void UnregisterEvents()
        {
            //Server/Player.event += server/player.onevent;
            Exiled.Events.Handlers.Server.RoundStarted -= server.OnRoundStarted;
            Exiled.Events.Handlers.Player.Died -= player.OnDeath;
            Exiled.Events.Handlers.Player.InteractingDoor -= player.OnInteractingDoor;
            Exiled.Events.Handlers.Player.Dying -= player.OnDying;

            player = null;
            server = null;
        }

    }
}

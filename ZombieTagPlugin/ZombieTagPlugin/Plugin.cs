using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Exiled.Events;

namespace ZombieTagPlugin
{
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin Singleton = new Plugin();
        public static Plugin Instance => Singleton;

        public override string Name => "Zombie Tag";
        public override string Author => "Roman_Noodles";
        public override Version Version => new Version(1, 0, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private Handlers.ZPlayer player;
        private Handlers.ZServer server;

        public static bool scp173Used = false;
        public static bool scp106Used = false;
        public static bool scp939Used = false;
        public static bool scp049Used = false;
        public static int deathCounter = 0;

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
            player = new Handlers.ZPlayer();
            server = new Handlers.ZServer();

            Server.WaitingForPlayers += server.OnWaitingForPlayers;
            Server.RoundStarted += server.OnRoundStarted;

            Player.InteractingDoor += player.OnInteractingDoor;
            Player.Died += player.onDied;
        }

        public void UnregisterEvents()
        {
            Server.WaitingForPlayers -= server.OnWaitingForPlayers;
            Server.RoundStarted -= server.OnRoundStarted;

            Player.InteractingDoor -= player.OnInteractingDoor;
            Player.Died -= player.onDied;

            player = null;
            server = null;
        }
    }
}

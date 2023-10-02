using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using PlayerRoles;
using UnityEngine;
using System.Linq;

namespace Oddball
{
    class MyServer
    {
        public static List<MyPlayer> players;
        public static ZoneType zone;
        public static bool has173;
        public static bool has939;
        public static bool has106;
        public static bool has049;
        public static bool has096;


        public void OnRoundStarted()
        {
            Round.IsLocked = true;
            Timing.KillCoroutines();
            has173 = false;
            has939 = false;
            has106 = false;
            has049 = false;
            has096 = false;
            foreach (Door d in Door.List)
                if (d.IsCheckpoint)
                    d.ChangeLock(Exiled.API.Enums.DoorLockType.NoPower);

            System.Random rand = new System.Random();
            if (rand.Next(3) == 0)
                zone = ZoneType.Entrance;
            else if (rand.Next(2) == 0)
                zone = ZoneType.HeavyContainment;
            else
                zone = ZoneType.LightContainment;

            players = new List<MyPlayer>();
            foreach (Player p in Player.List)
                players.Add(new MyPlayer(p));

            // Spawns
            Player firstHumanPlayer = Player.List.ElementAt(rand.Next(Server.PlayerCount));
            foreach (Player p in Player.List)
                if (!p.Equals(firstHumanPlayer))
                    MyPlayer.respawnSCP(p);

            Door scpDoor = Door.Random(zone);
            foreach (Player p in Player.List)
                p.Teleport(scpDoor);
            MyPlayer.spawnHuman(firstHumanPlayer);

            Timing.RunCoroutine(ManagePlayers());

        }

        public IEnumerator<float> ManagePlayers()
        {
            for (int i = Plugin.Instance.Config.gracePeriod; i > 0; i--) {
                foreach (MyPlayer mp in players)
                    mp.player.Broadcast(2, "Timestart in: " + i, Broadcast.BroadcastFlags.Normal, true);
                yield return Timing.WaitForSeconds(1);
            }

            Timing.RunCoroutine(MyPlayer.FullLocationInfo());

            bool done = false;
            int scoreNeeded = Plugin.Instance.Config.scoreNeededPerPlayer;
            if (Server.PlayerCount > 4)
                scoreNeeded /= 3;
            else
                scoreNeeded /= (Server.PlayerCount - 1);
            while (!done)
            {
                int topScore = 0;
                foreach(MyPlayer mp in players)
                {
                    if (mp.player.IsHuman)
                        mp.score++;
                    if (mp.score > topScore)
                        topScore = mp.score;
                }
                if (topScore > scoreNeeded)
                    done = true;
                yield return Timing.WaitForSeconds(1);
            }
            Round.IsLocked = false;
            Round.EndRound(true);
        }
    }
}

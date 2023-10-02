using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace SCPSlayer
{
    class MyServer
    {
        public static List<MyPlayer> players;
        ZoneType zone;
        RoleTypeId role;
        public void OnRoundStarted()
        {
            System.Random rand = new System.Random();
            Server.FriendlyFire = true;
            Round.IsLocked = true;
            players = new List<MyPlayer>();
            foreach (Player p in Player.List)
                players.Add(new MyPlayer(p));

            zone = ZoneType.Other;
            while (zone == ZoneType.Other)
            {
                int x = rand.Next(3);
                if (x == 0)
                    zone = ZoneType.Entrance;
                else if (x == 1)
                    zone = ZoneType.HeavyContainment;
                else if (x == 2)
                    zone = ZoneType.LightContainment;
            }
            role = RoleTypeId.None;
            while(role == RoleTypeId.None)
            {
                int x = rand.Next(9);
                if (x == 0)
                    role = RoleTypeId.ChaosConscript;
                else if (x == 1)
                    role = RoleTypeId.ChaosMarauder;
                else if (x == 2)
                    role = RoleTypeId.ChaosRepressor;
                else if (x == 3)
                    role = RoleTypeId.ChaosRifleman;
                else if (x == 4)
                    role = RoleTypeId.FacilityGuard;
                else if (x == 5)
                    role = RoleTypeId.NtfCaptain;
                else if (x == 6)
                    role = RoleTypeId.NtfPrivate;
                else if (x == 7)
                    role = RoleTypeId.NtfSergeant;
                else if (x == 8)
                    role = RoleTypeId.NtfSpecialist;
            }
            foreach (Player p in Player.List)
                p.Role.Set(RoleTypeId.Spectator);

            foreach (Door d in Door.List)
                if (d.IsCheckpoint)
                    d.ChangeLock(Exiled.API.Enums.DoorLockType.NoPower);

            Timing.RunCoroutine(MyPlayer.FullLocationInfo());
            Timing.RunCoroutine(RoundManage());
        }
        public IEnumerator<float> RoundManage()
        {
            bool gameOver = false;
            while (!gameOver)
            {
                foreach(MyPlayer mp in players)
                {
                    if (!mp.player.IsAlive && mp.lives > 0 && !mp.respawning)
                    {
                        Log.Info(mp.player.Nickname + " is respawning...");
                        mp.respawning = true;
                        Timing.CallDelayed(5f, () =>
                        {
                            mp.lives--;
                            mp.player.Role.Set(role);
                            int tries = 10;
                            while (mp.player.Zone != zone)
                            {
                                tries--;
                                Door d = Door.Random(zone);
                                if (!d.IsElevator && !d.IsGate && !d.IsCheckpoint)
                                    mp.player.Teleport(d);

                                float minDistance = 99999;
                                foreach (MyPlayer p in players)
                                {
                                    float dist = Vector3.Distance(p.player.Position, mp.player.Position);
                                    if (dist < minDistance && !p.Equals(mp))
                                        minDistance = dist;
                                }

                                if (tries > 0 && minDistance < Plugin.Instance.Config.spawnDistance)
                                    mp.player.Teleport(DoorType.SurfaceGate);
                            }
                            MyPlayer.fillInventory(mp.player);
                            mp.respawning = false;
                        });
                    }
                }

                int x = 0;
                foreach (MyPlayer mp in players)
                    if ((mp.player.IsAlive || mp.lives > 0))
                        x++;
                gameOver = x <= 1;

                yield return Timing.WaitForSeconds(1f);
            }
            Round.IsLocked = false;
        }
    }
}

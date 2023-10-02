using Exiled.Events.EventArgs.Player;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using System;
using Exiled.API.Features.Items;

namespace Oddball
{
    class MyPlayer
    {
        public Player player;
        public int score;

        public MyPlayer(Player p)
        {
            player = p;
        }
        
        public MyPlayer()
        {

        }

        public static void spawnHuman(Player p)
        {
            //If removing an scp, let know
            RoleTypeId tempRole = p.Role.Type;
            if (tempRole == RoleTypeId.Scp173)
                MyServer.has173 = false;
            if (tempRole == (RoleTypeId.Scp096))
                MyServer.has096 = false;
            if (tempRole == (RoleTypeId.Scp939))
                MyServer.has939 = false;
            if (tempRole == (RoleTypeId.Scp049))
                MyServer.has049 = false;
            if (tempRole == (RoleTypeId.Scp106))
                MyServer.has106 = false;

            // Choose class
            System.Random rand = new System.Random();
            if (rand.Next(2) == 0)
                p.Role.Set(RoleTypeId.ClassD);
            else if (rand.Next(2) == 0)
                p.Role.Set(RoleTypeId.Scientist);
            else if (rand.Next(3) == 0)
                p.Role.Set(RoleTypeId.FacilityGuard);
            else if (rand.Next(3) == 0)
                p.Role.Set(RoleTypeId.NtfPrivate);
            else if (rand.Next(3) == 0)
                p.Role.Set(RoleTypeId.NtfCaptain);
            else
                p.Role.Set(RoleTypeId.NtfSpecialist);

            // Choose location
            Door bestDoor = Door.Random(MyServer.zone);
            float bestDistance = 0;
            for (int i = 0; i < 10; i++)
            {
                Door d = Door.Random(MyServer.zone);
                p.Teleport(d);
                if (p.Zone != MyServer.zone)
                    continue;
                float closest = 99999;
                foreach (Player playa in Player.List)
                {
                    float dist = Vector3.Distance(d.Position, playa.Position);
                    if (dist < closest && !p.Equals(playa))
                        closest = dist;
                }
                if (closest > bestDistance)
                {
                    bestDistance = closest;
                    bestDoor = d;
                }
                if (bestDistance > Plugin.Instance.Config.radarDistance)
                    break;
            }
            p.Teleport(bestDoor);

            // Give items
            p.ClearInventory();

            p.AddItem(ItemType.KeycardO5);

            for (int i = 0; i < 3; i++)
            {
                int x = rand.Next(100);
                if (x < 10)
                    p.AddItem(ItemType.SCP500);
                else if (x < 20)
                    p.AddItem(ItemType.Painkillers);
                else if (x < 40)
                    p.AddItem(ItemType.Adrenaline);
                else if (x < 90)
                    p.AddItem(ItemType.Medkit);
            }

            for (int i = 0; i < 3; i++)
            {
                int x = rand.Next(100);
                if (x < 20)
                    p.AddItem(ItemType.SCP207);
                else if (x < 30)
                    p.AddItem(ItemType.SCP1853);
                else if (x < 50)
                    p.AddItem(ItemType.SCP2176);
                else if (x < 70)
                    p.AddItem(ItemType.GrenadeFlash);
                else if (x < 90)
                    p.AddItem(ItemType.GrenadeHE);
            }
            for(int i = 0; i < 1; i++)
            {
                int x = rand.Next(100);
                if (x < 30)
                    p.AddItem(ItemType.SCP268);
                else if (x < 60)
                    p.AddItem(ItemType.MicroHID);
                else
                    p.AddItem(ItemType.Flashlight);
            }            
        }

        public static void respawnSCP(Player p)
        {
            System.Random rand = new System.Random();
            RoleTypeId role = RoleTypeId.None;
            if (!MyServer.has173)
            {
                role = RoleTypeId.Scp173;
                MyServer.has173 = true;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int x = rand.Next(3);
                    RoleTypeId tempRole = RoleTypeId.None;
                    if (x == 0 && !MyServer.has939)
                    {
                        tempRole = RoleTypeId.Scp939;
                        MyServer.has939 = true;
                    }
                    else if (x == 1 && !MyServer.has049)
                    {
                        tempRole = RoleTypeId.Scp049;
                        MyServer.has049 = true;
                    }
                    else if (x == 2 && !MyServer.has106)
                    {
                        tempRole = RoleTypeId.Scp106;
                        MyServer.has106 = true;
                    }
                    else if (x == 3 && !MyServer.has096)
                    {
                        tempRole = RoleTypeId.Scp096;
                        MyServer.has096 = true;
                    }
                    role = tempRole;
                    break;
                }
            }
            if (role.Equals(RoleTypeId.None))
                role = RoleTypeId.Scp0492;


            p.Role.Set(RoleTypeId.Scp0492);
            Vector3 v = p.Position;
            p.Role.Set(role);

            p.Teleport(v);
            if (p.Zone == Exiled.API.Enums.ZoneType.HeavyContainment && p.Position.y < -1000)
                p.Teleport(p.CurrentRoom);
            if ((p.CurrentRoom.Identifier + "").Substring(0, 7).Equals("HCZ_Tes"))
                p.Teleport(Exiled.API.Enums.DoorType.HIDLeft);
            if ((p.CurrentRoom.Identifier + "").Substring(0, 4).Equals("Pock"))
                p.Teleport(Exiled.API.Enums.RoomType.Hcz106);
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.Player.IsScp || ev.IsAllowed == true)
                return;
            Room r = ev.Player.CurrentRoom;
            Timing.CallDelayed(20f, () =>
            {
                if (ev.Player.CurrentRoom.Equals(r))
                    ev.Door.IsOpen = true;
            });
        }

        public void OnDeath(DiedEventArgs ev)
        {
            Timing.CallDelayed(Plugin.Instance.Config.respawnTime, () =>
            {
            bool random = false;
            if (!ev.TargetOldRole.IsHuman())
            {
                Log.Info("SCP");
                RoleTypeId tempRole = ev.TargetOldRole;
                if (tempRole == RoleTypeId.Scp173)
                    MyServer.has173 = false;
                if (tempRole==(RoleTypeId.Scp096))
                    MyServer.has096 = false;
                if (tempRole==(RoleTypeId.Scp939))
                    MyServer.has939 = false;
                if (tempRole==(RoleTypeId.Scp049))
                    MyServer.has049 = false;
                if (tempRole==(RoleTypeId.Scp106))
                    MyServer.has106 = false;

                respawnSCP(ev.Player);
                return;
            }
            try
            {
                Log.Info(ev.Attacker.ToString());
            }   catch(Exception e)
            {
                Log.Info("slipup");
                random = true;
            }
            if (!random && ev.Attacker.Equals(ev.Player))
                random = true;



            if (!random)
            {
                spawnHuman(ev.Attacker);
                respawnSCP(ev.Player);
                return;
            }
            int x = 0;
            System.Random rand = new System.Random();
            Player nextHuman = ev.Player;
            foreach (Player p in Player.List)
                if (rand.Next(++x) == 0 && !p.Equals(ev.Player))
                    nextHuman = p;
            spawnHuman(nextHuman);
                respawnSCP(ev.Player);
            });
        }

        public void OnDying(DyingEventArgs ev)
        {
            ev.Player.ClearInventory();
        }

        public static IEnumerator<float> FullLocationInfo()
        {
            for (; ; )
            {
                foreach (MyPlayer mp in MyServer.players)
                {
                    Player p = mp.player;
                    if (p.IsScp)
                    {
                        float miniminDistance = 99999;
                        Player minPlayer = p;
                        foreach (Player player in Player.List)
                            if (!player.IsScp && Vector3.Distance(p.Position, player.Position) < miniminDistance && Vector3.Distance(p.Position, player.Position) > 0.01)
                            {
                                miniminDistance = Vector3.Distance(p.Position, player.Position);
                                minPlayer = player;
                            }

                        string s = ((int)miniminDistance) + " m";
                        if (miniminDistance > 150)
                        {
                            if (minPlayer.Zone == Exiled.API.Enums.ZoneType.HeavyContainment && p.Zone == Exiled.API.Enums.ZoneType.HeavyContainment)
                                s = (minPlayer.CurrentRoom.Identifier + "").Substring(0, 8);
                            else
                                s = minPlayer.Zone.ToString();
                        }
                        int limitingDistance = Plugin.Instance.Config.radarDistance;

                        if (miniminDistance < limitingDistance)
                            s = "<" + limitingDistance + " m";
                        if (miniminDistance >= 99998)
                            s = "";
                        p.Broadcast(1, "" + s, Broadcast.BroadcastFlags.Normal, true);
                    }
                    else
                        p.Broadcast(1, "Score: " + mp.score, Broadcast.BroadcastFlags.Normal, true);
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}

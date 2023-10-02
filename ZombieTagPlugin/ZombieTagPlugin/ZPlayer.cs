using Exiled.Events.EventArgs.Player;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Features.Doors;

namespace ZombieTagPlugin.Handlers
{
    class ZPlayer
    {
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            //Log.Info(message: ev.Door.Name);
            //Log.Info(message: ev.Door.Zone);
            //Log.Info(message: ev.Door.Room.Name);
            if (!ev.Player.IsScp || ev.IsAllowed == true || ev.Door.IsCheckpoint)
                return;
            Room r = ev.Player.CurrentRoom;
            Timing.CallDelayed(20f, () =>
            {
                if (ev.Player.CurrentRoom.Equals(r))
                    ev.Door.IsOpen = true;
            });
        }
        public void onDied(DiedEventArgs ev)
        {
            Timing.CallDelayed(5f, () =>
            {
                Exiled.API.Features.Respawn.NtfTickets = 0.1f;
                if (!ev.TargetOldRole.IsHuman())
                    return;
                Plugin.deathCounter++;
                ev.Player.Role.Set(RoleTypeId.Scp0492);
                Room r = ev.Player.CurrentRoom;
                Vector3 v = ev.Player.Position;


                int c2 = 0;
                System.Random rand = new System.Random();
                if (!Plugin.scp049Used)
                {
                    if (c2 > -1 && 4 - Plugin.deathCounter - c2 > 0 && rand.Next(4 - Plugin.deathCounter - c2) == 0)
                    {
                        ev.Player.Role.Set(RoleTypeId.Scp049);
                        c2 = -999;
                        Plugin.scp049Used = true;
                    }
                    c2++;
                }
                if (!Plugin.scp106Used)
                {
                    if (c2 > -1 && 4 - Plugin.deathCounter - c2 > 0 && rand.Next(4 - Plugin.deathCounter - c2) == 0)
                    {
                        ev.Player.Role.Set(RoleTypeId.Scp106);
                        c2 = -999;
                        Plugin.scp106Used = true;
                    }
                    c2++;
                }
                if (!Plugin.scp173Used)
                {
                    if (c2 > -1 && 4 - Plugin.deathCounter - c2 > 0 && rand.Next(4 - Plugin.deathCounter - c2) == 0)
                    {
                        ev.Player.Role.Set(RoleTypeId.Scp173);
                        c2 = -999;
                        Plugin.scp173Used = true;
                    }
                    c2++;
                }
                if (!Plugin.scp939Used)
                {
                    if (c2 > -1 && 4 - Plugin.deathCounter - c2 > 0 && rand.Next(4 - Plugin.deathCounter - c2) == 0)
                    {
                        ev.Player.Role.Set(RoleTypeId.Scp939);
                        c2 = -999;
                        Plugin.scp939Used = true;
                    }
                    c2++;
                }

                ev.Player.Teleport(v);
                if (ev.Player.Zone == Exiled.API.Enums.ZoneType.HeavyContainment && ev.Player.Position.y < -1000)
                    ev.Player.Teleport(ev.Player.CurrentRoom);
                if ((ev.Player.CurrentRoom.Identifier + "").Substring(0, 7).Equals("HCZ_Tes"))
                    ev.Player.Teleport(Exiled.API.Enums.DoorType.HIDLeft);
                if ((ev.Player.CurrentRoom.Identifier + "").Substring(0, 4).Equals("Pock"))
                    ev.Player.Teleport(Exiled.API.Enums.RoomType.Hcz106);

            });
        }
        public static IEnumerator<float> FullLocationInfo()
        {
            for (; ; )
            {

                foreach (Door d in Door.List)
                    if (d.Type == Exiled.API.Enums.DoorType.ElevatorGateA)
                        d.IsOpen = false;
                    else if (d.Type == Exiled.API.Enums.DoorType.ElevatorGateB)
                        d.IsOpen = false;
                    else if (d.Type == Exiled.API.Enums.DoorType.ElevatorLczA)
                        d.IsOpen = false;
                    else if (d.Type == Exiled.API.Enums.DoorType.ElevatorLczB)
                        d.IsOpen = false;


                foreach (Player p in Player.List)
                {
                    if (p.IsNTF && !(p.Role.Type == RoleTypeId.FacilityGuard) || p.IsCHI)
                        p.Role.Set(RoleTypeId.Spectator);
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
                        int limitingDistance = Plugin.Instance.Config.maxDistance - Plugin.Instance.Config.minDistance;
                        limitingDistance = Plugin.Instance.Config.minDistance + (int)((float)limitingDistance * Plugin.deathCounter / 3);

                        limitingDistance += limitingDistance % 5;
                        if (limitingDistance < Plugin.Instance.Config.minDistance)
                            limitingDistance = Plugin.Instance.Config.minDistance;
                        else if (limitingDistance > Plugin.Instance.Config.maxDistance)
                            limitingDistance = Plugin.Instance.Config.maxDistance;
                        if (miniminDistance < limitingDistance)
                            s = "<" + limitingDistance + " m";
                        if (miniminDistance >= 99998)
                            s = "";
                        p.Broadcast(1, "" + s, Broadcast.BroadcastFlags.Normal, true);
                    }
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }


}

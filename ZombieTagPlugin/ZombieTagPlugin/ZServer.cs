using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZombieTagPlugin.Handlers
{
    class ZServer
    {
        public void OnWaitingForPlayers()
        {
            //Log.Info(message: "Waiting for players...");
        }

        public void OnRoundStarted()
        {

            System.Random rand = new System.Random();

            // choose spawning conditions for match
            int zoneNumber = rand.Next(3); // 0,1,2=L,H,E
            int roleNumber = rand.Next(100); 

            // Spawn all as Human Players
            foreach(Player player in Player.List)
            {
                // Choose starting position
                if (zoneNumber == 0)
                    player.Role.Set(RoleTypeId.Scientist);
                else if(zoneNumber == 1)
                {
                    player.Role.Set(RoleTypeId.ClassD);
                    Door d = Door.Random(Exiled.API.Enums.ZoneType.HeavyContainment);
                    while(d.Room.Equals(Exiled.API.Enums.RoomType.HczEzCheckpointA) || d.Room.Equals(Exiled.API.Enums.RoomType.HczEzCheckpointB) || d.Room.Equals(Exiled.API.Enums.RoomType.Hcz096))
                        d = Door.Random(Exiled.API.Enums.ZoneType.HeavyContainment);
                    player.Teleport(d.Room);
                }
                else
                {
                    player.Role.Set(RoleTypeId.FacilityGuard);
                }
                Vector3 spawnPos = player.Position;

                // Assign role and tp to spawnPos
                if (roleNumber < 50)
                    player.Role.Set(RoleTypeId.ClassD);
                else if (roleNumber < 75)
                    player.Role.Set(RoleTypeId.Scientist);
                else
                    player.Role.Set(RoleTypeId.FacilityGuard);
                player.Teleport(spawnPos);

                // Give Items
                player.ClearInventory();
                switch (rand.Next(12)) 
                {
                    case 0:
                        player.AddItem(ItemType.KeycardChaosInsurgency);
                        break;
                    case 1:
                        player.AddItem(ItemType.KeycardContainmentEngineer);
                        break;
                    case 2:
                        player.AddItem(ItemType.KeycardFacilityManager);
                        break;
                    case 3:
                        player.AddItem(ItemType.KeycardGuard);
                        break;
                    case 4:
                        player.AddItem(ItemType.KeycardJanitor);
                        break;
                    case 5:
                        player.AddItem(ItemType.KeycardNTFCommander);
                        break;
                    case 6:
                        player.AddItem(ItemType.KeycardNTFLieutenant);
                        break;
                    case 7:
                        player.AddItem(ItemType.KeycardNTFOfficer);
                        break;
                    case 8:
                        player.AddItem(ItemType.KeycardO5);
                        break;
                    case 9:
                        player.AddItem(ItemType.KeycardResearchCoordinator);
                        break;
                    case 10:
                        player.AddItem(ItemType.KeycardScientist);
                        break;
                    default:
                        player.AddItem(ItemType.KeycardZoneManager);
                        break;
                }

            }

            Plugin.scp049Used = false;
            Plugin.scp106Used = false;
            Plugin.scp173Used = false;
            Plugin.scp939Used = false;
            Plugin.deathCounter = 0;

            int firstSCP = rand.Next(4);
            firstSCP = 2;
            int firstPlayer = rand.Next(Server.PlayerCount);
            Player p = Player.List.ElementAt(firstPlayer);

            if (firstSCP == 0)
            {
                p.Role.Set(RoleTypeId.Scp049);
                Plugin.scp049Used = true;
            }
            if (firstSCP == 1)
            {
                p.Role.Set(RoleTypeId.Scp106);
                Plugin.scp106Used = true;
            }
            if (firstSCP == 2)
            {
                p.Role.Set(RoleTypeId.Scp173);
                Plugin.scp173Used = true;
            }
            if (firstSCP == 3)
            {
                p.Role.Set(RoleTypeId.Scp939);
                Plugin.scp939Used = true;
            }
            //p.Teleport(p.CurrentRoom);
            if (zoneNumber == 0)
                p.Teleport(Exiled.API.Enums.DoorType.Scp173Gate);
            else if (zoneNumber == 2)
                if(rand.Next(2)==0)
                p.Teleport(Exiled.API.Enums.RoomType.HczEzCheckpointA);
                else
                    p.Teleport(Exiled.API.Enums.RoomType.HczEzCheckpointB);
            Timing.RunCoroutine(ZPlayer.FullLocationInfo());

            // Lock Doors
            foreach (Door d in Door.List)
                if (d.IsCheckpoint)
                    d.ChangeLock(Exiled.API.Enums.DoorLockType.NoPower);

        }
    }
}

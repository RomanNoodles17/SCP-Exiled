using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace SCPSlayer
{
    class MyPlayer
    {
        public Player player;
        public int lives;
        public bool respawning = false;

        public MyPlayer(Player p)
        {
            player = p;
            lives = Plugin.Instance.Config.Lives;
        }

        public static void fillInventory(Player player)
        {
            player.ClearInventory();
            System.Random rand = new System.Random();
            for(int i = 0; i < 2; i++)
            {
                int gun = rand.Next(10);
                if (gun == 0)
                    player.AddItem(ItemType.GunAK);
                if (gun == 1)
                    player.AddItem(ItemType.GunCOM15);
                if (gun == 2)
                    player.AddItem(ItemType.GunCOM18);
                if (gun == 3)
                    player.AddItem(ItemType.GunCom45);
                if (gun == 4)
                    player.AddItem(ItemType.GunCrossvec);
                if (gun == 5)
                    player.AddItem(ItemType.GunE11SR);
                if (gun == 6)
                    player.AddItem(ItemType.GunFSP9);
                if (gun == 7)
                    player.AddItem(ItemType.GunLogicer);
                if (gun == 8)
                    player.AddItem(ItemType.GunRevolver);
                if (gun == 9)
                    player.AddItem(ItemType.GunRevolver);
            }
            player.AddItem(ItemType.KeycardO5);
            player.AddItem(ItemType.ArmorCombat);

            for(int i = 0; i < 2; i++)
            {
                int x = rand.Next(100);
                if (x < 10)
                    player.AddItem(ItemType.SCP500);
                else if (x < 20)
                    player.AddItem(ItemType.Painkillers);
                else if (x < 40)
                    player.AddItem(ItemType.Adrenaline);
                else if (x < 90)
                    player.AddItem(ItemType.Medkit);
            }
            for(int i = 0; i < 2; i++)
            {
                int x = rand.Next(100);
                if (x < 2)
                    player.AddItem(ItemType.SCP268);
                else if (x < 10)
                    player.AddItem(ItemType.SCP207);
                else if (x < 20)
                    player.AddItem(ItemType.SCP1853);
                else if (x < 30)
                    player.AddItem(ItemType.SCP2176);
                else if (x < 55)
                    player.AddItem(ItemType.GrenadeFlash);
                else if (x < 80)
                    player.AddItem(ItemType.GrenadeHE);
            }

            player.AddAmmo(Exiled.API.Enums.AmmoType.Ammo12Gauge, 120);
            player.AddAmmo(Exiled.API.Enums.AmmoType.Ammo44Cal, 120);
            player.AddAmmo(Exiled.API.Enums.AmmoType.Nato556, 48);
            player.AddAmmo(Exiled.API.Enums.AmmoType.Nato762, 48);
            player.AddAmmo(Exiled.API.Enums.AmmoType.Nato9, 48);

        }

        public static IEnumerator<float> FullLocationInfo()
        {
            for (; ; )
            {
                foreach (MyPlayer mp in MyServer.players)
                {
                    Player p = mp.player;
                    if (p.IsAlive)
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
                        p.Broadcast(10, "Lives Remaining: " + mp.lives, Broadcast.BroadcastFlags.Normal, true);
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }
    }
}

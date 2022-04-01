﻿using BepInEx;
using BepInEx.Configuration;
using LegendAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using UnityEngine;

namespace Mythical {

    #region BepInPlugin Notes
    // The BepInPlugin attribute in [brackets] is setting parameters for bepin to load your mod

    // GUID: "TheTimeSweeper.CameraModExample"
    // The the identifier for your mod. This one simply follows the convention of "AuthorName.YourModName"

    // Name: "CameraModExample"
    // The the full human-readable name of your mod.

    // Version: "0.1.0"
    // The Version, as you'd expect, useful for differing between updates.
    //     Customary to follow Semantic Versioning (major.minor.patch). 
    //         You don't have to, but you'll just look silly in front of everyone. It's ok. I won't make fun of you.
    #endregion
    [BepInPlugin("Amber.Mythical", "Mythical", "0.1.0")]
    public class ContentLoader : BaseUnityPlugin {
        #region BaseUnityPlugin Notes
        // BaseUnityPlugin is the main class that gets loaded by bepin.
        // It inherits from MonoBehaviour, so it gains all the familiar Unity callback functions you can use: 
        //     Awake, Start, Update, FixedUpdate, etc.

        //     Awake is most important. it's basically where we initialize everything we do

        //     For further reading, you can check out https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

        // now, close these two Notes regions so the script looks little nicer to work with 
        #endregion

        // This Awake() function will run at the very start when the mod is initialized
        void Awake() {

            Skills.Awake();

            // This is the just a first little tester code to see if our mod is running on WoL. You'll see it in the BepInEx console
            Debug.Log("Loading Outfits");
            OutfitInfo outfitInfo = new OutfitInfo();
            outfitInfo.name = "Showman";
            outfitInfo.outfit = new global::Outfit("Mythical::Showman", 30, new List<global::OutfitModStat>
            {
                new global::OutfitModStat(global::OutfitModStat.OutfitModType.Damage, 0f, 0.1f, 0f, false),
                new global::OutfitModStat(Outfits.CustomModType, 0f, 0.1f, 0f, false)
            }, false, false);
            outfitInfo.customDesc = ((bool b) => "Start with more money, rich boy ;)");
            outfitInfo.customMod = ((player, b, b2) => {
                if (b)
                {
                    Player.goldWallet.balance += 100;
                } else
                {
                    Player.goldWallet.balance -= 100;
                }
            });

            Outfits.Register(outfitInfo);

            outfitInfo = new OutfitInfo();
            outfitInfo.name = "Shade";
            outfitInfo.outfit = new global::Outfit("Mythical::Shade", 24, new List<global::OutfitModStat>
            {
                new global::OutfitModStat(global::OutfitModStat.OutfitModType.Speed, 0f, 0.1f, 0f, false),
                new global::OutfitModStat(Outfits.CustomModType, 0f, 0.1f, 0f, false)
            }, false, false);
            outfitInfo.customDesc = ((bool b) => "Pits will no longer spawn in rooms!");
            outfitInfo.customMod = ((player, b, b2) => {
                Level.removeAllPits = b;
            });

            Outfits.Register(outfitInfo);

            outfitInfo = new OutfitInfo();
            outfitInfo.name = "Hoarder";
            outfitInfo.outfit = new global::Outfit("Mythical::Hoarder", 22, new List<global::OutfitModStat>
            {
                new global::OutfitModStat(global::OutfitModStat.OutfitModType.Speed, 0f, -0.1f, 0f, false),
                new global::OutfitModStat(Outfits.CustomModType, 0f, 0.1f, 0f, false)
            }, false, false);
            outfitInfo.customDesc = ((bool b) => "You can carry six more items!");
            outfitInfo.customMod = ((player, b, b2) => {
                if (b)
                {
                    On.Inventory.AddItem_Item_bool_bool += Inventory_AddItem;
                } else
                {
                    On.Inventory.AddItem_Item_bool_bool -= Inventory_AddItem;
                }
            });

            Outfits.Register(outfitInfo);


            //SampleSkillLoader.Awake();


            ItemInfo monsterTooth = new ItemInfo();
            monsterTooth.name = "MonsterTooth";
            monsterTooth.item = new MonsterTooth();
            monsterTooth.tier = 1;

            TextManager.ItemInfo itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Monster Tooth";
            itemInfo.description = "Gain health when killing an enemy!";
            itemInfo.itemID = MonsterTooth.staticID;

            Sprite spr = ImgHandler.LoadSprite("tooth");

            monsterTooth.text = itemInfo;
            monsterTooth.icon = (spr != null ? spr : null);

            Items.Register(monsterTooth);

            ItemInfo behemoth = new ItemInfo();
            behemoth.name = "BrilliantBehemoth";
            behemoth.item = new Behemoth();
            behemoth.tier = 1;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Brilliant Behemoth";
            itemInfo.description = "Chance to create explosions on hit, BUT deal less damage!";
            itemInfo.itemID = Behemoth.staticID;

            spr = ImgHandler.LoadSprite("cannon");

            behemoth.text = itemInfo;
            behemoth.icon = (spr != null ? spr : null);

            Items.Register(behemoth);

            ItemInfo sage = new ItemInfo();
            sage.name = "InvisibleOnLowHealth";
            sage.item = new InvisibleOnLowHealth();
            sage.tier = 2;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sage's Armor";
            itemInfo.description = "Become invulnerable at low health!";
            itemInfo.itemID = InvisibleOnLowHealth.staticID;

            spr = ImgHandler.LoadSprite("sage");

            sage.text = itemInfo;
            sage.icon = (spr != null ? spr : null);

            Items.Register(sage);

            ItemInfo midas = new ItemInfo();
            midas.name = "MidasRage";
            midas.item = new MidasRage();
            midas.tier = 2;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Rage of Midas";
            itemInfo.description = "Destructibles drop more gold more frequently!";
            itemInfo.itemID = MidasRage.staticID;

            spr = ImgHandler.LoadSprite("midas");

            midas.text = itemInfo;
            midas.icon = (spr != null ? spr : null);

            Items.Register(midas); //Here is where all the funny anti element relics come in ------------

            ItemInfo frost = new ItemInfo();
            frost.name = "frostCrit";
            frost.item = new FrostCrit();
            frost.tier = 3;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sanctum of Antifrost";
            itemInfo.description = "Attacks against Frost enemies are guaranteed to be critical!";
            itemInfo.itemID = FrostCrit.staticID;

            spr = ImgHandler.LoadSprite("antifrost");

            frost.text = itemInfo;
            frost.icon = (spr != null ? spr : null);

            Items.Register(frost);

            ItemInfo flame = new ItemInfo();
            flame.name = "flameCrit";
            flame.item = new FrostCrit();
            flame.tier = 3;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sanctum of Anti-Flame";
            itemInfo.description = "Attacks against flame enemies are guaranteed to be critical!";
            itemInfo.itemID = "flameCrit";

            spr = ImgHandler.LoadSprite("antiflame");

            flame.text = itemInfo;
            flame.icon = (spr != null ? spr : null);

            Items.Register(flame);

            ItemInfo earth = new ItemInfo();
            earth.name = "earthCrit";
            earth.item = new FrostCrit();
            earth.tier = 2;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sanctum of Anti-Earth";
            itemInfo.description = "Attacks against earth enemies are guaranteed to be critical!";
            itemInfo.itemID = "earthCrit";

            spr = ImgHandler.LoadSprite("antiearth");

            earth.text = itemInfo;
            earth.icon = (spr != null ? spr : null);

            Items.Register(earth);

            ItemInfo wind = new ItemInfo();
            wind.name = "windCrit";
            wind.item = new FrostCrit();
            wind.tier = 2;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sanctum of Anti-Wind";
            itemInfo.description = "Attacks against wind enemies are guaranteed to be critical!";
            itemInfo.itemID = "windCrit";

            spr = ImgHandler.LoadSprite("antiwind");

            wind.text = itemInfo;
            wind.icon = (spr != null ? spr : null);

            Items.Register(wind);
            
            ItemInfo thunder = new ItemInfo();
            thunder.name = "thunderCrit";
            thunder.item = new FrostCrit();
            thunder.tier = 4;

            itemInfo = new TextManager.ItemInfo();
            itemInfo.displayName = "Sanctum of Anti-thunder";
            itemInfo.description = "Attacks against thunder enemies are guaranteed to be critical!";
            itemInfo.itemID = "thunderCrit";

            spr = ImgHandler.LoadSprite("antithunder");

            thunder.text = itemInfo;
            thunder.icon = (spr != null ? spr : null);

            Items.Register(thunder);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                GameController.players[0].GetComponent<Player>().GiveDesignatedItem("DamageUp");
            }
        }

        public bool Inventory_AddItem(On.Inventory.orig_AddItem_Item_bool_bool orig, Inventory self, Item givenItem, bool showNotice, bool ignoreMax)
        {
            self.currentItemID = givenItem.ID;
            Player.newItems[givenItem.category].Remove(self.currentItemID);
            if (self.ContainsItem(self.currentItemID))
            {
                return false;
            }
            if (!self.CheckForMutallyExclusiveItems(self.currentItemID))
            {
                return false;
            }
            if (self.itemDict.Count >= 18 &&!self.CheckItemCombine(givenItem) && !ignoreMax && !self.DropItem(string.Empty))
            {
                return false;
            }
            givenItem.parentEntity = self.parentEntity;
            givenItem.SetParentSkillCategory();
            self.itemDict[self.currentItemID] = givenItem;
            givenItem.Activate();
            if (showNotice && self.parentEntity is Player)
            {
                ((Player)self.parentEntity).newItemNoticeUI.Display(TextManager.GetItemName(self.currentItemID), IconManager.GetItemIcon(self.currentItemID), null, false, false, false, true);
                if (givenItem.isCursed)
                {
                    SoundManager.PlayAudio("BuyCursedRelic", 1f, false, -1f, -1f);
                }
                else
                {
                    SoundManager.PlayAudio("Powerup", 1f, false, -1f, -1f);
                }
                if (givenItem.isCursed)
                {
                    PoolManager.GetPoolItem<ParticleEffect>("CursedAuraBurstEffect").Play(null, new Vector3?(self.parentEntity.transform.position), null, null, null, null, 0f, false);
                }
            }
            self.count++;
            self.AnnounceItemEvent(givenItem, true);
            if (givenItem.isCursed)
            {
                AchievementManager.UnlockAchievement(Achievement.HoldFourCursed, false);
            }
            return true;

            //return orig(self, item, show, true);
        }

        // This Update() function will run every frame

        // Here, we'll hook on to the CameraController's Awake function, to mess with things when it initializes.


        // Here, we're hooking on the Update function. This code runs every frame on that CameraController

    }
}

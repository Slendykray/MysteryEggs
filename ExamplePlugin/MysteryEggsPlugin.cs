using BepInEx;
using R2API;
using RoR2;
using RoR2.ExpansionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace MysteryEggs
{
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class MysteryEggsPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Slendykray";
        public const string PluginName = "MysteryEggs";
        public const string PluginVersion = "1.0.0";

        public static MysteryEggsPlugin instance;

        public static ItemDef lunarOnKill;
        public static ItemDef goldOnKill;
        public static ItemDef voidOnKill;

        public static ItemDef goldEgg;
        public static ItemDef lunarEgg;
        public static ItemDef voidEgg;


        public static DeployableSlot goldSlot;
        public static DeployableSlot lunarSlot;
        public static DeployableSlot voidSlot;

        public void Awake()
        {
            instance = this;

            Log.Init(Logger);

            Asset.Init();

            InitItems();

            InitHelpers();

            InitDeployableSlots();

            Tokens.Init();

            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;

            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;

        }

        private void InitItems()
        {
            var displayRules = new ItemDisplayRuleDict(null);


            goldEgg = ScriptableObject.CreateInstance<ItemDef>();
            // Language Tokens, explained there https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
            goldEgg.name = "GOLDEGG_NAME";
            goldEgg.nameToken = "GOLDEGG_NAME";
            goldEgg.pickupToken = "GOLDEGG_PICKUP";
            goldEgg.descriptionToken = "GOLDEGG_DESC";
            goldEgg.loreToken = "GOLDEGG_LORE";

#pragma warning disable Publicizer001
            goldEgg._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier2Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001
            //goldEgg.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/FireBallDash/texEggIcon.png").WaitForCompletion();
            goldEgg.pickupIconSprite = Asset.mainBundle.LoadAsset<Sprite>("texGold");
            //goldEgg.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion();
            GameObject pref = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion(), "eggg");
            pref.transform.Find("mdlEgg").GetComponent<MeshRenderer>().material = Asset.mainBundle.LoadAsset<Material>("matGold");
            goldEgg.pickupModelPrefab = pref;

            goldEgg.canRemove = true;
            goldEgg.hidden = false;
            goldEgg.tags = [ItemTag.AIBlacklist, ItemTag.CannotCopy, ItemTag.FoodRelated, ItemTag.ExtractorUnitBlacklist, ItemTag.CanBeTemporary, ItemTag.Utility];

            ItemAPI.Add(new CustomItem(goldEgg, displayRules));


            lunarEgg = ScriptableObject.CreateInstance<ItemDef>();
            lunarEgg.name = "LUNAREGG_NAME";
            lunarEgg.nameToken = "LUNAREGG_NAME";
            lunarEgg.pickupToken = "LUNAREGG_PICKUP";
            lunarEgg.descriptionToken = "LUNAREGG_DESC";
            lunarEgg.loreToken = "LUNAREGG_LORE";

#pragma warning disable Publicizer001
            lunarEgg._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/LunarTierDef.asset").WaitForCompletion();
#pragma warning restore Publicizer001
            //lunarEgg.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/FireBallDash/texEggIcon.png").WaitForCompletion();
            lunarEgg.pickupIconSprite = Asset.mainBundle.LoadAsset<Sprite>("texLunar");
            //lunarEgg.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion();
            GameObject pref2 = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion(), "eggg");
            pref2.transform.Find("mdlEgg").GetComponent<MeshRenderer>().material = Asset.mainBundle.LoadAsset<Material>("matLunar");
            lunarEgg.pickupModelPrefab = pref2;

            lunarEgg.canRemove = true;
            lunarEgg.hidden = false;
            lunarEgg.tags = [ItemTag.AIBlacklist, ItemTag.CannotCopy, ItemTag.FoodRelated, ItemTag.ExtractorUnitBlacklist, ItemTag.CanBeTemporary, ItemTag.Utility];

            ItemAPI.Add(new CustomItem(lunarEgg, displayRules));



            voidEgg = ScriptableObject.CreateInstance<ItemDef>();
            voidEgg.name = "VOIDEGG_NAME";
            voidEgg.nameToken = "VOIDEGG_NAME";
            voidEgg.pickupToken = "VOIDEGG_PICKUP";
            voidEgg.descriptionToken = "VOIDEGG_DESC";
            voidEgg.loreToken = "VOIDEGG_LORE";

#pragma warning disable Publicizer001
            voidEgg._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/DLC1/Common/VoidTier2Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001
            //voidEgg.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/FireBallDash/texEggIcon.png").WaitForCompletion();
            voidEgg.pickupIconSprite = Asset.mainBundle.LoadAsset<Sprite>("texVoid");
            //voidEgg.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion();
            GameObject pref3 = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/FireBallDash/PickupEgg.prefab").WaitForCompletion(), "eggg");
            pref3.transform.Find("mdlEgg").GetComponent<MeshRenderer>().material = Asset.mainBundle.LoadAsset<Material>("matVoid");
            voidEgg.pickupModelPrefab = pref3;

            voidEgg.canRemove = true;
            voidEgg.hidden = false;
            voidEgg.tags = [ItemTag.AIBlacklist, ItemTag.CannotCopy, ItemTag.FoodRelated, ItemTag.ExtractorUnitBlacklist, ItemTag.CanBeTemporary, ItemTag.Utility];

            voidEgg.requiredExpansion = Addressables.LoadAssetAsync<ExpansionDef>("RoR2/DLC1/Common/DLC1.asset").WaitForCompletion();

            ItemAPI.Add(new CustomItem(voidEgg, displayRules));


            var provider = ScriptableObject.CreateInstance<ItemRelationshipProvider>();
            provider.name = "MysteryEggContagiousItemProvider";
            provider.relationshipType = Addressables.LoadAssetAsync<ItemRelationshipType>("RoR2/DLC1/Common/ContagiousItem.asset").WaitForCompletion();
            provider.relationships =
            [
                new ItemDef.Pair
                {
                    itemDef1 = goldEgg,
                    itemDef2 = voidEgg
                }

            ];
            ContentAddition.AddItemRelationshipProvider(provider);
        }

        private void InitDeployableSlots()
        {
            goldSlot = DeployableAPI.RegisterDeployableSlot((master, slot) =>
            {
                return master.inventory.GetItemCount(goldEgg);
            });

            lunarSlot = DeployableAPI.RegisterDeployableSlot((master, slot) =>
            {
                return master.inventory.GetItemCount(lunarEgg);
            });

            voidSlot = DeployableAPI.RegisterDeployableSlot((master, slot) =>
            {
                return master.inventory.GetItemCount(voidEgg);
            });
        }

        private void InitHelpers()
        {
            var displayRules = new ItemDisplayRuleDict(null);

            goldOnKill = ScriptableObject.CreateInstance<ItemDef>();

            goldOnKill.name = "GOLDONKILL_NAME";
            goldOnKill.nameToken = "GOLDONKILL_NAME";
            goldOnKill.pickupToken = "GOLDONKILL_PICKUP";
            goldOnKill.descriptionToken = "GOLDONKILL_DESC";
            goldOnKill.loreToken = "GOLDONKILL_LORE";

            goldOnKill.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Core/texNullIcon.png").WaitForCompletion();

            goldOnKill.canRemove = false;
            goldOnKill.hidden = true;
            goldOnKill.tags = [ItemTag.WorldUnique];
            goldOnKill.deprecatedTier = ItemTier.NoTier;

            ItemAPI.Add(new CustomItem(goldOnKill, displayRules));


            lunarOnKill = ScriptableObject.CreateInstance<ItemDef>();

            lunarOnKill.name = "LUNARONKILL_NAME";
            lunarOnKill.nameToken = "LUNARONKILL_NAME";
            lunarOnKill.pickupToken = "LUNARONKILL_PICKUP";
            lunarOnKill.descriptionToken = "LUNARONKILL_DESC";
            lunarOnKill.loreToken = "LUNARONKILL_LORE";

            lunarOnKill.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Core/texNullIcon.png").WaitForCompletion();

            lunarOnKill.canRemove = false;
            lunarOnKill.hidden = true;
            lunarOnKill.tags = [ItemTag.WorldUnique];
            lunarOnKill.deprecatedTier = ItemTier.NoTier;

            ItemAPI.Add(new CustomItem(lunarOnKill, displayRules));


            voidOnKill = ScriptableObject.CreateInstance<ItemDef>();

            voidOnKill.name = "VOIDONKILL_NAME";
            voidOnKill.nameToken = "VOIDONKILL_NAME";
            voidOnKill.pickupToken = "VOIDONKILL_PICKUP";
            voidOnKill.descriptionToken = "VOIDONKILL_DESC";
            voidOnKill.loreToken = "VOIDONKILL_LORE";
         
            voidOnKill.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Core/texNullIcon.png").WaitForCompletion();

            voidOnKill.canRemove = false;
            voidOnKill.hidden = true;
            voidOnKill.tags = [ItemTag.WorldUnique];
            voidOnKill.deprecatedTier = ItemTier.NoTier;
        
            ItemAPI.Add(new CustomItem(voidOnKill, displayRules));
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            if (NetworkServer.active)
            {
                // Setting the behavior stacks to 1 or 0 may be more appropriate
                // by checking if it exists in the inventory at all.
                self.AddItemBehavior<LunarEggBehavior>(self.inventory.GetItemCount(lunarEgg));
                self.AddItemBehavior<GoldEggBehavior>(self.inventory.GetItemCount(goldEgg));
                self.AddItemBehavior<VoidEggBehavior>(self.inventory.GetItemCount(voidEgg));
            }
            orig(self);
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            if (!report.attacker || !report.attackerBody)
            {
                return;
            }

            TeamIndex attackerTeamIndex = report.attackerTeamIndex;

            var attackerCharacterBody = report.attackerBody;

            if (attackerCharacterBody.inventory)
            {
                var goldCount = attackerCharacterBody.inventory.GetItemCount(goldOnKill.itemIndex);
                if (goldCount > 0)
                {
                    GameObject gameObject11 = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BonusMoneyPack"), report.victim.gameObject.transform.position, UnityEngine.Random.rotation);
                    TeamFilter component13 = gameObject11.GetComponent<TeamFilter>();
                    if (component13)
                    {
                        component13.teamIndex = attackerTeamIndex;
                    }
                    NetworkServer.Spawn(gameObject11);
                }

                var lunarCount = attackerCharacterBody.inventory.GetItemCount(lunarOnKill.itemIndex);
                if (lunarCount > 0)
                {
                    //if (Util.CheckRoll(80))
                    //{
                        PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.MiscPickups.LunarCoin.miscPickupIndex), report.victim.transform.position, Vector3.up * 10f);
                    //}                        
                }

                var voidCount = attackerCharacterBody.inventory.GetItemCount(voidOnKill.itemIndex);
                if (voidCount > 0)
                {
                    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("bubbet.bubbetsitems"))
                    {
                        PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(DLC1Content.MiscPickups.VoidCoin.miscPickupIndex), report.victim.transform.position, Vector3.up * 10f);
                    }
                    else
                    {
                        PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.MiscPickups.LunarCoin.miscPickupIndex), report.victim.transform.position, Vector3.up * 10f);
                    }
                }
            }
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            //if (Input.GetKeyDown(KeyCode.F2))
            //{
            //    // Get the player body to use a position:
            //    var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

            //    // And then drop our defined item in front of the player.

            //    Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");
            //    //PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(goldEgg.itemIndex), transform.position, transform.forward * 20f);
            //    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(goldEgg.itemIndex), transform.position, Vector3.zero);
            //}

            //if (Input.GetKeyDown(KeyCode.F4))
            //{
            //    // Get the player body to use a position:
            //    var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

            //    // And then drop our defined item in front of the player.

            //    Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");
            //    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(voidEgg.itemIndex), transform.position, Vector3.zero);
            //}

            //if (Input.GetKeyDown(KeyCode.F5))
            //{
            //    // Get the player body to use a position:
            //    var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

            //    // And then drop our defined item in front of the player.

            //    Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");
            //    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(lunarEgg.itemIndex), transform.position, Vector3.zero);
            //}
        }
    }
}

using R2API;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryEggs
{
    internal class GoldEggBehavior : CharacterBody.ItemBehavior
    {
        private float timeBetweenResummons = 30f;

        private float timeBetweenRetryResummons = 1f;

        private float resummonCooldown;

        private DeployableSlot slot = MysteryEggsPlugin.goldSlot;

        public static ItemDef GetItemDef()
        {
            return MysteryEggsPlugin.goldEgg;
        }

        private void Awake()
        {
            base.enabled = false;    
        }

        private void OnEnable()
        {
            this.resummonCooldown = timeBetweenRetryResummons;
        }

        private void FixedUpdate()
        {

            int stack = this.stack;

            CharacterMaster bodyMaster = base.body.master;

            if (!bodyMaster)
			{
                return;
            }

            int deployableCount = bodyMaster.GetDeployableCount(slot);
            if (deployableCount < stack)
            {
                this.resummonCooldown -= Time.fixedDeltaTime;
                if (this.resummonCooldown <= 0f)
                {
                    //GameObject newMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleMaster.prefab").WaitForCompletion(), "lunarboi");
                    GameObject newMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BeetleGuard/BeetleGuardMaster.prefab").WaitForCompletion(), "lunarboi");
                    //newMaster.AddComponent<Deployable>();

                    //AISkillDriver aiReturn = newMaster.AddComponent<AISkillDriver>();
                    //aiReturn.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                    //aiReturn.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    //aiReturn.aimType = AISkillDriver.AimType.AtMoveTarget;
                    //aiReturn.minDistance = 10f;
                    //aiReturn.skillSlot = SkillSlot.None;
                    //aiReturn.customName = "ReturnToLeaderDefault";


                    //AISkillDriver aiLeash = newMaster.AddComponent<AISkillDriver>();
                    //aiLeash.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                    //aiLeash.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    //aiLeash.aimType = AISkillDriver.AimType.AtCurrentLeader;
                    //aiLeash.minDistance = 50f;
                    //aiLeash.skillSlot = SkillSlot.None;
                    //aiLeash.driverUpdateTimerOverride = 3f;
                    //aiLeash.resetCurrentEnemyOnNextDriverSelection = true;
                    //aiLeash.customName = "ReturnToOwnerLeash";



                    //foreach (AISkillDriver ais in newMaster.transform)
                    //{
                    //    if (ais.customName.Contains("PathFromAfar"))
                    //    {
                    //        Destroy(ais);
                    //    }
                    //}

                    //GameObject newBody = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleBody.prefab").WaitForCompletion(), "lunarboibody");
                    GameObject newBody = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BeetleGuard/BeetleGuardBody.prefab").WaitForCompletion(), "lunarboibody");
                    var body = newBody.GetComponent<CharacterBody>();
                    body.baseRegen = 0.6f;
                    body.levelRegen = 0.12f;
                    //base 500
                    body.baseMaxHealth = 800f;
                    //base 12
                    body.baseDamage = 20f;
                    newMaster.GetComponent<CharacterMaster>().bodyPrefab = newBody;


                    CharacterSpawnCard newCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
                    newCard.prefab = newMaster;

                    newCard.itemsToGrant = new ItemCountPair[]
                    {
                        new ItemCountPair()
                        {
                            itemDef = MysteryEggsPlugin.goldOnKill,
                            count = 1
                        }
                    };



                    DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(newCard, new DirectorPlacementRule
                    {
                        placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                        minDistance = 3f,
                        maxDistance = 40f,
                        spawnOnTarget = base.transform
                    }, RoR2Application.rng);
                    directorSpawnRequest.summonerBodyObject = base.gameObject; 
                    
                    directorSpawnRequest.onSpawnedServer += OnSpawned;   
                    
                    DirectorCore.instance.TrySpawnObject(directorSpawnRequest);


                    void OnSpawned(SpawnCard.SpawnResult result)
                    {
                        if (result.success)
                        {
                            bodyMaster.AddDeployable(result.spawnedInstance.GetComponent<Deployable>(), slot);
                        }                     
                        else
                        {
                            this.resummonCooldown = timeBetweenRetryResummons;
                            return;
                        }                      
                    }

                    this.resummonCooldown = timeBetweenResummons / stack;
                }
            }


        }


    }
}

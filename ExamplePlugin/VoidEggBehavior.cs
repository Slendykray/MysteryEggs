using R2API;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MysteryEggs
{
    internal class VoidEggBehavior : CharacterBody.ItemBehavior
    {
        private float timeBetweenResummons = 60f;

        private float timeBetweenRetryResummons = 1f;

        private float resummonCooldown;

        private DeployableSlot slot = MysteryEggsPlugin.voidSlot;

        public static ItemDef GetItemDef()
        {
            return MysteryEggsPlugin.voidEgg;
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
                    int random = Random.Range(0, 3);

                    GameObject[] fullmasters =
                        [
                           Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierMaster.prefab").WaitForCompletion(),
                           Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerMaster.prefab").WaitForCompletion(),
                           Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabMaster.prefab").WaitForCompletion(),
                        ];


                    GameObject[] bodies =
                        [
                            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierBody.prefab").WaitForCompletion(),
                            Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerBody.prefab").WaitForCompletion(),
                            Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion(),
                        ];
                    

                    GameObject newMaster = PrefabAPI.InstantiateClone(fullmasters[random], "lunarboi");
                    newMaster.AddComponent<Deployable>();


                    AISkillDriver aiReturn = newMaster.AddComponent<AISkillDriver>();
                    aiReturn.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                    aiReturn.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    aiReturn.aimType = AISkillDriver.AimType.AtMoveTarget;
                    aiReturn.minDistance = 10f;
                    aiReturn.skillSlot = SkillSlot.None;
                    aiReturn.customName = "ReturnToLeaderDefault";


                    //GameObject newBody = PrefabAPI.InstantiateClone(bodies[random], "lunarboibody");
                    //var body = newBody.GetComponent<CharacterBody>();
                    //body.baseRegen = 0.6f;
                    //body.baseMaxHealth = 500f;
                    //newMaster.GetComponent<CharacterMaster>().bodyPrefab = newBody;




                    CharacterSpawnCard newCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
                    newCard.prefab = newMaster;

                    newCard.itemsToGrant = new ItemCountPair[]
                    {
                        new ItemCountPair()
                        {
                            itemDef = MysteryEggsPlugin.voidOnKill,
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
                    //directorSpawnRequest.teamIndexOverride = TeamIndex.Neutral;

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

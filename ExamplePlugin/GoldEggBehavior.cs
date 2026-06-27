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
                    CharacterSpawnCard newCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
                    newCard.prefab = Prefabs.beetleMaster;

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

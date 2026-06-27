using UnityEngine;
using System.IO;
using System.Reflection;
using RoR2;
using R2API;
using UnityEngine.AddressableAssets;
using RoR2.CharacterAI;

namespace MysteryEggs
{
    public static class Prefabs
    {
        public static GameObject beetleMaster;
        public static GameObject lemurianMaster;
        public static GameObject[] voidMasters;

        public static void Init()
        {
            //gold
            beetleMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BeetleGuard/BeetleGuardMaster.prefab").WaitForCompletion(), "lunarboi");
            if (!beetleMaster.GetComponent<Deployable>())
            {
                beetleMaster.AddComponent<Deployable>();
            }

            GameObject beetleBodyPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BeetleGuard/BeetleGuardBody.prefab").WaitForCompletion(), "beetleGoldBodyPrefab");
            var beetleBody = beetleBodyPrefab.GetComponent<CharacterBody>();
            beetleBody.baseRegen = 0.6f;
            beetleBody.levelRegen = 0.12f;         
            beetleBody.baseMaxHealth = 800f;  //base 500     
            beetleBody.baseDamage = 20f; //base 12
            beetleMaster.GetComponent<CharacterMaster>().bodyPrefab = beetleBodyPrefab;

            ContentAddition.AddBody(beetleBodyPrefab);

            //lunar
            lemurianMaster = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/LemurianMaster.prefab").WaitForCompletion(), "lunarboi");
            lemurianMaster.AddComponent<Deployable>();
            AISkillDriver aiReturnLunar = lemurianMaster.AddComponent<AISkillDriver>();
            aiReturnLunar.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
            aiReturnLunar.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            aiReturnLunar.aimType = AISkillDriver.AimType.AtMoveTarget;
            aiReturnLunar.minDistance = 10f;
            aiReturnLunar.skillSlot = SkillSlot.None;
            aiReturnLunar.customName = "ReturnToLeaderDefault";


            foreach (AISkillDriver ais in lemurianMaster.transform)
            {
                if (ais.customName.Contains("PathFromAfar"))
                {
                    Object.Destroy(ais);
                }
            }

            GameObject lemurianBodyPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lemurian/LemurianBody.prefab").WaitForCompletion(), "lemurianLunarBodyPrefab");
            var lemurianBody = lemurianBodyPrefab.GetComponent<CharacterBody>();
            lemurianBody.baseRegen = 0.6f;
            lemurianBody.baseMaxHealth = 500f;
            lemurianMaster.GetComponent<CharacterMaster>().bodyPrefab = lemurianBodyPrefab;

            ContentAddition.AddBody(lemurianBodyPrefab);

            //void
            voidMasters =
                [
                     PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Nullifier/NullifierMaster.prefab").WaitForCompletion(), "gg"),
                     PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerMaster.prefab").WaitForCompletion(), "gg"),
                     PrefabAPI.InstantiateClone( Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabMaster.prefab").WaitForCompletion(), "gg")
                ];

            foreach (GameObject master in voidMasters)
            {
                master.AddComponent<Deployable>();

                AISkillDriver aiReturnVoid = master.AddComponent<AISkillDriver>();
                aiReturnVoid.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                aiReturnVoid.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                aiReturnVoid.aimType = AISkillDriver.AimType.AtMoveTarget;
                aiReturnVoid.minDistance = 10f;
                aiReturnVoid.skillSlot = SkillSlot.None;
                aiReturnVoid.customName = "ReturnToLeaderDefault";
            }

        }

    }
}

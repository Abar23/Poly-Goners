using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSE5912
{
    public class DamageManager
    {
        public enum DamageType { MELEE, RANGED, FIRE, MAGIC, GAS};
        public float DamageUpdateFrequency = 0.1f; // seconds before damage is updated in the damage over time.
        public static DamageManager Instance { get; } = new DamageManager();
        private Dictionary<string,Dictionary<DamageType, float>> damageScaleMap = new Dictionary<string,Dictionary<DamageType, float>>();
        //private MonoBehaviour timerClass = new MonoBehaviour();
        private IEnumerable damageOverTimeCoroutine;
        private DamageManager()
        {
            var playerScales = new Dictionary<DamageType, float>(5);
            playerScales[DamageType.MELEE] = 1;
            playerScales[DamageType.RANGED] = 1;
            playerScales[DamageType.FIRE] = 0;
            playerScales[DamageType.MAGIC] = 1.2f;
            playerScales[DamageType.GAS] = 1;
            damageScaleMap["Player"] = playerScales;
            //timerClass = new MonoBehaviour();
        }
        public void TakeDamage(GameObject target, DamageType damageType)
        {
            // Lookup the mapping based on a tag on the target (perhaps it is immune to fire)
            // Todo: If player?
            if (damageType == DamageType.GAS)
            {
                float damageRate = GameConstants.Instance.GasDamageAmount;
                float damageDuration = GameConstants.Instance.GasDamageDuration;

                TakeDamageOverTime(target, damageRate, damageDuration);
            }
        }
        public void Heal()
        {
            GameConstants.Instance.timingClass.StopAllCoroutines();
        }
        private void TakeDamageImpulse(GameObject target, float amount)
        {

        }
        private void TakeDamageOverTime(GameObject target, float rate, float duration)
        {
            string targetType = "Player"; // Hack
            float damageRate = rate * damageScaleMap[targetType][DamageType.GAS];
            // Todo: Call player takes damage?
            // Bug: Coroutines are only on MOnoBehaviors.
            //StartCoroutine(damageOverTimeCoroutine);
            IEnumerator coroutine = InflictPlayerDamageOverTime(damageRate, duration);
            GameConstants.Instance.timingClass.StartCoroutine(coroutine);
        }
        private IEnumerator InflictPlayerDamageOverTime(float damageRate, float damageDuration)
        {
            float lastTime = Time.time;
            float duration = 0;
            while (duration < damageDuration)
            {
                duration += Time.time - lastTime;
                lastTime = Time.time;
                GameConstants.Instance.PlayerHealth -= damageRate * Time.deltaTime;
                yield return new WaitForSeconds(DamageUpdateFrequency);
            }
        }
    }
}

using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game.Enemies
{
    public class LifeDrivenBoss : BossBase
    {

        [Space(10)]
        [Header("Life driven variables")]
        [SerializeField] private PhaseConfig[] _phases;

        protected override IEnumerator Randomize_behaviour_paterns()
        {
            while (_start_movements)
            {
                yield return new WaitForSeconds(Random.Range(_minDelayBetweenPowers, _maxDelayBetweenPowers));

                float c = Random.Range(0f, 1f);
                Debug.Log($"Boss: {c}");

                //Reduce shoots interval
                if (c <= 0.15f)
                {
                    //Set
                    _minDelayBetweenShots -= 0.1f;
                    _maxDelayBetweenShots -= 0.1f;

                    _minDelayBetweenShots = Mathf.Clamp(_minDelayBetweenShots, 0, _maxDelayBetweenShots);
                    _maxDelayBetweenShots = Mathf.Clamp(_maxDelayBetweenShots, _minDelayBetweenShots, _maxDelayBetweenShots);
                }
                //Gets faster
                else if (c <= 0.3f)
                {
                    //Set
                    _speed += 0.25f;
                }
                
                else if (c <= 0.8f)
                {
                    _superpowers.Use_super_power(_phases, LifePercent);
                }

                yield return null;
            }
        }
    }
}
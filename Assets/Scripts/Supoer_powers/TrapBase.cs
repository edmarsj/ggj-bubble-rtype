using System.Collections.Generic;
using UnityEngine;

namespace Game.Super_powers
{
    public class TrapBase : PausableBehaviour
    {
        [Space(10)]
        [Header("Trap general settings")]

        [SerializeField] protected float _minTrapDistance = 2f;

        /// <summary>
        /// We want to spawn as many traps as we can, but we can't overlap with any existing ones
        /// </summary>
        protected static List<Transform> _currentTraps = new();

        protected bool _trapSet = false;


        public bool TrySetTrap()
        {
            if (!_trapSet)
            {
                var testPosition = TrapArea.Instance.GetRandom2DPosition();

                foreach (var trap in _currentTraps)
                {
                    if (!trap)
                    {
                        continue;
                    }

                    if (Vector2.Distance(trap.position, testPosition) < _minTrapDistance)
                    {
                        return false;
                    }
                }
                _trapSet = true;
                transform.position = testPosition;
                _currentTraps.Add(transform);                
            }

            return true;
        }
    }

}
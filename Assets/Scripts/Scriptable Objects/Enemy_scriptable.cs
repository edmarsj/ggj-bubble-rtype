using UnityEngine;

namespace Game.Enemies
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
    public class Enemy_scriptable : ScriptableObject
    {
        [Header("Configs")]
        public float Speed;
        public int Points;
        public float minDelayBetweenShots;
        public float maxDelayBetweenShots;

        [Header("Advanced configs")]
        [Range(1, 5f)]
        public int Scale_multiplier;
        public Sprite Sprite;
    }
}
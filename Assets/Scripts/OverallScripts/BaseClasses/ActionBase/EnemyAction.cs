using UnityEngine;

namespace SzymonPeszek.BaseClasses
{
    public class EnemyAction : ScriptableObject
    {
        [Header("Attack Action", order = 0)]
        [Header("Attack Animation Name", order = 1)]
        public string actionAnimation;
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace Script
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected NavMeshAgent _meshAgent;

        public abstract void Damage(int _damage , GameObject hitObj);
        public abstract void Dead();
        public abstract void Live();

        [HideInInspector] public bool isDead;

        [HideInInspector] public bool isHit;
    }
}

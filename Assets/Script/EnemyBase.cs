using System;
using UnityEngine;
using UnityEngine.AI;

namespace Script
{
    public abstract class EnemyBase : MonoBehaviour , Health
    {
        public int health;
        protected NavMeshAgent _meshAgent;
      
        public void Damage(int _damage)
        {
            health -= _damage;
        }
        
        public abstract void Dead();
        public abstract void Live();

        [HideInInspector] public bool isDead;

        [HideInInspector] public bool isHit;
       
    }
}

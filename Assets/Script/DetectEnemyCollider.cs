using UnityEngine;

namespace Script
{
    public class DetectEnemyCollider : MonoBehaviour
    {
        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Skill"))
            { 
                _enemy.Damage(3,other.gameObject);
            }
        }
    }
}

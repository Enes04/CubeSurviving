using UnityEngine;
using UnityEngine.AI;

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
                _enemy.Hit(50, other.gameObject, 10);
            }

            if (other.CompareTag("Tornado"))
            {
                _enemy.transform.SetParent(other.transform);
                _enemy.TornadoScript(other.gameObject);
            }
        }
    }
}
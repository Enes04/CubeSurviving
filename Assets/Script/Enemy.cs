using DamageNumbersPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script
{
    public class Enemy : EnemyBase
    {
        private PlayerMovement _playerMovement;
        public DamageNumber prefab;
        private EnemySpawnManager _enemySpawnManager;
        private GameObject rotateObject;
        private Renderer myRenderer;
        private Color baseColor;
        private ParticleManager _particleManager;

        private void Awake()
        {
            _particleManager = FindObjectOfType<ParticleManager>();
            _enemySpawnManager = FindObjectOfType<EnemySpawnManager>();
            _playerMovement = FindObjectOfType<PlayerMovement>();
            rotateObject = transform.GetChild(0).gameObject;
            _meshAgent = GetComponent<NavMeshAgent>();
            myRenderer = transform.GetChild(0).GetComponent<Renderer>();
            baseColor = myRenderer.material.color;
        }


        void Update()
        {
            if (!isDead && !isHit)
            {
                _meshAgent.SetDestination(_playerMovement.transform.position);
            }
        }

        public void Hit(int _damage, GameObject hitObj,int backhitForce)
        {
            Damage(_damage);
            BackHitAnim(hitObj,backhitForce);
            ShowDamageText(_damage);
            if (health <= 0)
                Dead();
        }


        #region BackHitAnimation

        public void BackHitAnim(GameObject obj,int backHitforce)
        {
            isHit = true;
            Vector3 direction = (transform.position - _playerMovement.transform.position).normalized * backHitforce;
            direction = new Vector3(direction.x, 0, direction.z);
            GetComponent<Collider>().isTrigger = false;
            _meshAgent.enabled = false;
            transform.DOKill();
            transform.DOPunchScale(new Vector3(.1f, .3f, .1f), .2f);
            HitMaterial();
            transform.DOMove(transform.position + direction, 1).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                CancelInvoke("SkillBackHitDisable");
                Invoke("SkillBackHitDisable", .1f);
            });
        }

        public void SkillBackHitDisable()
        {
            isHit = false;
            _meshAgent.enabled = true;
            GetComponent<Collider>().isTrigger = true;
        }

        public void HitMaterial()
        {
            //characterModel.transform.DOKill();
            myRenderer.material.DOColor(Color.white, .1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                myRenderer.material.DOColor(baseColor, .1f).SetEase(Ease.Linear);
            });
        }

        #endregion

        #region EnemyStatus

        public override void Dead()
        {
            ParticleSystem currentDeadParticle = Instantiate(_particleManager.deadParticle);
            currentDeadParticle.transform.position =
                new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
            currentDeadParticle.Play();
            gameObject.SetActive(false);
            _meshAgent.enabled = false;
            isDead = true;
        }

        public override void Live()
        {
            RandomPos();
            _meshAgent.enabled = true;
            isDead = false;
        }

        public void RandomPos()
        {
            float radius = 20f;
            Vector3 randomPos = Random.insideUnitSphere * radius;
            randomPos += _playerMovement.transform.position;
            randomPos.y = 0f;

            Vector3 direction = randomPos - _playerMovement.transform.position;
            direction.Normalize();

            float dotProduct = Vector3.Dot(transform.forward, direction);
            float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * direction.magnitude);

            randomPos.x = Mathf.Cos(dotProductAngle) * radius + _playerMovement.transform.position.x;
            randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius +
                          _playerMovement.transform.position.z;
            transform.position = randomPos;
        }

        #endregion

        #region OnEnableDisable

        private void OnEnable()
        {
            _enemySpawnManager.LiveEnemyAdd();
        }

        private void OnDisable()
        {
            _enemySpawnManager.DeadEnemyRemove();
            _enemySpawnManager._enemyPool.Add(gameObject);
        }

        #endregion

        #region ShowDamageText

        public void ShowDamageText(int damage)
        {
            DamageNumber newDamageNumber =
                prefab.Spawn(new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    damage);
        }

        #endregion
    }
}
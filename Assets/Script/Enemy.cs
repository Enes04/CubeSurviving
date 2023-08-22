using System;
using DamageNumbersPro;
using DG.Tweening;
using Script;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : EnemyBase
{
    private PlayerMovement _playerMovement;
    public DamageNumber prefab;
    private EnemySpawnManager _enemySpawnManager;
    private GameObject rotateObject;
    private Renderer myRenderer;
    private Color baseColor;
    private void Awake()
    {
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

    public override void Damage(int _damage, GameObject hitObj)
    {
        BackHitAnim(hitObj);
        ShowDamageText(_damage);
    }


    #region BackHitAnimation

    public void BackHitAnim(GameObject obj)
    {
        isHit = true;
        Vector3 direction = (transform.position - _playerMovement.transform.position).normalized * 10;
        direction = new Vector3(direction.x, transform.position.y, direction.z);
        GetComponent<Collider>().isTrigger = false;
        _meshAgent.enabled = false;
        transform.DOPunchScale(new Vector3(.1f,.3f,.1f), .2f);
        HitMaterial();
        transform.DOMove(transform.position+direction, 1).SetEase(Ease.OutQuad).OnComplete(() =>
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
        gameObject.SetActive(false);
        _meshAgent.enabled = false;
        isDead = true;
    }

    public override void Live()
    {
        transform.position =
            _playerMovement.transform.position + new Vector3(0, 0, Random.Range(-10, 10));
        _meshAgent.enabled = true;
        isDead = false;
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
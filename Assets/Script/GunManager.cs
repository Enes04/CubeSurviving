using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject muzzle;
    [HideInInspector] public List<GameObject> bullet;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        bullet = new List<GameObject>();
        BulletPoolStart();
    }

    public void BulletPoolStart()
    {
        for (int i = 0; i < 70; i++)
        {
            GameObject currentBullet = Instantiate(_gameManager.bulletPrefab);
            currentBullet.transform.SetParent(_gameManager.bulletParent.transform);
            bullet.Add(currentBullet);
            currentBullet.SetActive(false);
        }
    }
    public void BulletPoolAdd(GameObject bullets)
    {
        bullet.Add(bullets);
    }
}
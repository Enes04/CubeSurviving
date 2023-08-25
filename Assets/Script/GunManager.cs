using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Skills currentSkill;
    public GameObject muzzle;
    private List<GameObject> bullet;
    private GameManager _gameManager;
    public float rateOfFire;
    private float currentTime;
    
    private WaitSkillTime currentSkillTimeCoolDown;
    
    private void Start()
    {
      
        _gameManager = FindObjectOfType<GameManager>();
        bullet = new List<GameObject>();
        BulletPoolStart();
        
        for (int i = 0; i < _gameManager.waitSkillTimes.Length; i++)
        {
            if (currentSkill == _gameManager.waitSkillTimes[i].mySkill)
                currentSkillTimeCoolDown = _gameManager.waitSkillTimes[i];
        }
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

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentTime += Time.deltaTime;
            if (rateOfFire < currentTime)
            {
                currentSkillTimeCoolDown.StartCoolDown(rateOfFire);
                currentTime = 0;
                FireShoot();
            }
        }
    }
    public void BulletPoolAdd(GameObject bullets)
    {
        bullet.Add(bullets);
    }
    public void FireShoot()
    {
        GameObject currentBullet = bullet[0];
        bullet.RemoveAt(0);
        currentBullet.transform.rotation = transform.GetChild(0).rotation;
        currentBullet.transform.position = muzzle.transform.position;
        currentBullet.SetActive(true);
        currentBullet.GetComponent<Bullet>().Shot();
    }
}

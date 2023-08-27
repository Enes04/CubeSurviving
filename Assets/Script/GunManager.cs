using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Skills[] currentSkill;
    public GameObject muzzle;
    private List<GameObject> bullet;
    public ParticleSystem tornado;
    private GameManager _gameManager;
    public float[] rateOfFire;
    private float currentTime;
    private float currentTime1;
    private WaitSkillTime[] currentSkillTimeCoolDown;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        bullet = new List<GameObject>();
        currentSkillTimeCoolDown = new WaitSkillTime[rateOfFire.Length];
        BulletPoolStart();
        for (int j = 0; j < currentSkill.Length; j++)
        {
            for (int i = 0; i < _gameManager.waitSkillTimes.Length; i++)
            {
                if (currentSkill[j] == _gameManager.waitSkillTimes[i].mySkill)
                    currentSkillTimeCoolDown[j] = _gameManager.waitSkillTimes[i];
            }
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
        currentTime += Time.deltaTime;
        currentTime1 += Time.deltaTime;
        if (rateOfFire[0] < currentTime)
        {
            currentSkillTimeCoolDown[0].StartCoolDown(rateOfFire[0]);
            currentTime = 0;
            FireShoot();
        }
        if (rateOfFire[1] < currentTime1)
        {
            currentSkillTimeCoolDown[1].StartCoolDown(rateOfFire[1]);
            currentTime1 = 0;
            FireTornado();
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
    public void FireTornado()
    {
        tornado.Play();
        tornado.transform.position = transform.position;
    }
}
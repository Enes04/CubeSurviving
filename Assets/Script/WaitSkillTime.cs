using System;
using UnityEngine;
using UnityEngine.UI;

public enum Skills
{
    singleFireShoot,
    dash,
    tornado,
    earthquake
}
public class WaitSkillTime : MonoBehaviour
{
    public Skills mySkill;
    public Image cdImage;
    private float cooldownTime;
    public bool cdActive;

    private void Awake()
    {
        cdImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (cdActive)
        {
            cdImage.fillAmount += Time.deltaTime / cooldownTime;
            if (cdImage.fillAmount >= 1)
            {
                cdActive = false;
            }
        }
    }

    public void StartCoolDown(float _cooldownTime)
    {
        if (!cdActive)
        {
            ResetCoolDown();
            cooldownTime = _cooldownTime;
            cdActive = true;
        }
    }

    public void ResetCoolDown()
    {
        cdImage.fillAmount = 0;
    }
    
}

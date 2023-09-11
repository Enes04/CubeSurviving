using System;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tornado : Askill
{
    public Image tornadoImage;
    public float cooldown;
    public ParticleSystem tornadoObj;

    private void Start()
    {
        Cooldown = cooldown;
        CooldownImage = tornadoImage;
    }

    protected override void ActivateSkill()
    {
        tornadoObj.Play();
        GetComponent<Collider>().enabled = true;
        CancelInvoke("CloseSkill");
        Invoke("CloseSkill", 20);
    }

    public void CloseSkill()
    {
        GetComponent<Collider>().enabled = false;
        tornadoObj.Stop();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<Enemy>())
            {
                child.GetComponent<Enemy>().TornadoClear();
                child.GetComponent<Enemy>().Dead();
            }
                
        }
    }
}
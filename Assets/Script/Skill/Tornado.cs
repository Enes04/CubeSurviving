using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tornado : Askill
{
    [FormerlySerializedAs("_tornadoImage")] public Image tornadoImage;
    [FormerlySerializedAs("_cooldown")] public float cooldown;
    [FormerlySerializedAs("TornadoObj")] public ParticleSystem tornadoObj;

    private void Start()
    {
        Cooldown = cooldown;
        CooldownImage = tornadoImage;
    }

    protected override void ActivateSkill()
    {
        tornadoObj.Play();
        CancelInvoke("CloseSkill");
        Invoke("CloseSkill",2);
    }

    public void CloseSkill()
    {
        tornadoObj.Stop();
    }
}

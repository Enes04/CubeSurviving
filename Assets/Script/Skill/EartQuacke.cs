using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EartQuacke : Askill
{
    private VisualEffect vfx;
    private PlayerMovement _playerMovement;
    public Image eartquackeImage;
    public float cooldown;
    public GameObject fireObj;
    private bool _fire;
    
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        Cooldown = cooldown;
        CooldownImage = eartquackeImage;
        vfx = fireObj.GetComponentInChildren<VisualEffect>();
    }

    protected override void ActivateSkill()
    {
        _fire = true;
        vfx.Play();
        fireObj.transform.position = _playerMovement.transform.position;
        fireObj.transform.rotation = _playerMovement.transform.GetChild(0).rotation;
        CancelInvoke("CloseSkill");
        Invoke("CloseSkill",3);
    }

    private void Update()
    {
        if (_fire)
        {
            fireObj.transform.Translate(Vector3.forward * speed*Time.deltaTime);
        }
    }

    public void CloseSkill()
    {
        _fire = false;
    }
}

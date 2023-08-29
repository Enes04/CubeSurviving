using UnityEngine;
using UnityEngine.UI;

public class BasicFire : Askill
{
    public Image skillImage;
    private GunManager _gunManager;
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        _gunManager = FindObjectOfType<GunManager>();
        Cooldown = cooldown;
        CooldownImage = skillImage;
    }

    protected override void ActivateSkill()
    {
        GameObject currentBullet = _gunManager.bullet[0];
        _gunManager.bullet.RemoveAt(0);
        currentBullet.transform.rotation = transform.GetChild(0).rotation;
        currentBullet.transform.position = _gunManager.muzzle.transform.position;
        currentBullet.SetActive(true);
        currentBullet.GetComponent<Bullet>().Shot();
    }
}
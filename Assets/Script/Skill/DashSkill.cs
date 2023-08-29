using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DashSkill : Askill
{
    public Image skillImage;
    private PlayerMovement _playerMovement;
    public float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        Cooldown = cooldown;
        CooldownImage = skillImage;
    }

    protected override void ActivateSkill()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.DOMove(transform.position+(move * 25), .1f).SetEase(Ease.Linear).OnComplete((() => _playerMovement.dash = false));
    }
    
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EartQuacke : Askill
{
    private PlayerMovement _playerMovement;
    public Image eartquackeImage;
    public float cooldown;
    public GameObject fireObj;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        Cooldown = cooldown;
        CooldownImage = eartquackeImage;
    }

    protected override void ActivateSkill()
    {
        fireObj.transform.position = _playerMovement.transform.position;
        fireObj.transform.DOMove(Vector3.forward*100, 2f);
    }

    public void CloseSkill()
    {
       
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Askill : MonoBehaviour
{
    public float Cooldown { get; protected set; }
    public float CooldownTimer { get; protected set; }
    public Image CooldownImage { get; protected set; }

    public bool CanUse()
    {
        return CooldownTimer <= 0;
    }

    public void Use()
    {
        if (CanUse())
        {
            CooldownTimer = Cooldown;
            ActivateSkill();
        }
    }

    protected virtual void ActivateSkill()
    {
    }

    public void UpdateCooldown()
    {
        if (CooldownTimer > 0)
        {
            CooldownTimer -= Time.deltaTime;
            UpdateCooldownUI();
        }
    }

    protected void UpdateCooldownUI()
    {
        float fillAmount = CooldownTimer / Cooldown;
        CooldownImage.fillAmount = fillAmount;
    }
}
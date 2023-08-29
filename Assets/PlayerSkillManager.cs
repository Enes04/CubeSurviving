using System;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
     BasicFire basicFire;
     Tornado tornadoSkill;
     DashSkill _dashSkill;

    private void Start()
    {
        basicFire = FindObjectOfType<BasicFire>();
        tornadoSkill = FindObjectOfType<Tornado>();
        _dashSkill = FindObjectOfType<DashSkill>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicFireSkill();
        TornadoSkill();
        DashSkill();
    }

    public void BasicFireSkill()
    {
        basicFire.UpdateCooldown();
        if (basicFire.CanUse())
        {
            basicFire.Use();
        }
    }

    public void TornadoSkill()
    {
        tornadoSkill.UpdateCooldown();
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (tornadoSkill.CanUse())
            {
                tornadoSkill.Use();
            }
        }
    }

    public void DashSkill()
    {
        _dashSkill.UpdateCooldown();
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_dashSkill.CanUse())
                _dashSkill.Use();
        }
    }
}
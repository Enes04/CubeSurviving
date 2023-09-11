using System;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    BasicFire basicFire;
    Tornado tornadoSkill;
    DashSkill _dashSkill;
    EartQuacke _eartQuacke;

    private void Start()
    {
        basicFire = FindObjectOfType<BasicFire>();
        tornadoSkill = FindObjectOfType<Tornado>();
        _dashSkill = FindObjectOfType<DashSkill>();
        _eartQuacke = FindObjectOfType<EartQuacke>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicFireSkill();
        TornadoSkill();
        DashSkill();
        EarthQuackeSkill();
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

    public void EarthQuackeSkill()
    {
        _eartQuacke.UpdateCooldown();
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (_eartQuacke.CanUse())
                _eartQuacke.Use();
        }
    }
}
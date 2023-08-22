using System.Collections;
using System.Collections.Generic;
using AkilliMum;
using UnityEngine;

public class ChangeBluredWheelRotDump : MonoBehaviour
{
    private SimpleRotaterAround[] _scripts;
    // Start is called before the first frame update
    void Start()
    {
        //get rotation scripts
        _scripts = FindObjectsOfType<SimpleRotaterAround>();
    }

    public void Change(float value)
    {
        foreach (var simpleRotaterAround in _scripts)
        {
            simpleRotaterAround.Angle = value;
        }
    }
}

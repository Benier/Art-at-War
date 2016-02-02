using UnityEngine;
using System.Collections;

public class DefensiveCondition : Condition
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Test()
    {
        if (Input.GetKey(KeyCode.P))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

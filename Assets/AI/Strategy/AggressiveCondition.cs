using UnityEngine;
using System.Collections;

public class AggressiveCondition : Condition
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Test(Unit u)
    {
        if (Input.GetKey(KeyCode.O))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

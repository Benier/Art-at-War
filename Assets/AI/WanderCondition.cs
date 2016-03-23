using UnityEngine;
using System.Collections;

public class WanderCondition : Condition {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool Test(Unit u)
    {
        if (Input.GetKey(KeyCode.W))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

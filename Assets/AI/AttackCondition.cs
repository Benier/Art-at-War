using UnityEngine;
using System.Collections;

public class AttackCondition : Condition {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool Test(Unit u)
    {
        if (Input.GetKey(KeyCode.A) && u.AP > 0)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
}

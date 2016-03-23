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
        //float xInterval = inputBaseTexture.width / mapGen.MAP_WIDTH;
        //float yInterval = inputBaseTexture.height / mapGen.MAP_LENGTH;
        //correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        //correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        if (/*Input.GetKey(KeyCode.A) && */u.AP > 0)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
}

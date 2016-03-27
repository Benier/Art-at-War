﻿using UnityEngine;
using System.Collections;

public class WanderAction : Action {
    MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Execute(Unit u)
    {
        //Debug.Log("Wandering");
        GameObject target = new GameObject();
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);

        target.transform.position = new Vector3(x, 0, z);
        //u.ability = Unit.Ability.Attack;
        u.MoveToTarget(target);
    }
}

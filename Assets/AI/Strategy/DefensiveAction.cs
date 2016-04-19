using UnityEngine;
using System.Collections;

public class DefensiveAction : Action
{
    float reward;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Execute(Unit u)
    {
        //Debug.Log("Collapsing In Defensively");
    }

    public float GetReward()
    {
        return reward;
    }
}

using UnityEngine;
using System.Collections;

public class AggressiveAction : Action
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
        //Debug.Log("Spreading Out Aggressively");
    }

    public float GetReward()
    {
        return reward;
    }
}

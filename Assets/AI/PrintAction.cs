using UnityEngine;
using System.Collections;

public class PrintAction : Action
{
    float reward;
    string output;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public PrintAction(string s)
    {
        output = s;
    }

    public void Execute(Unit u)
    {
        Debug.Log(output);
        u.AP -= 2;
    }

    public float GetReward()
    {
        return reward;
    }

    public string GetName()
    {
        return "Print";
    }
}

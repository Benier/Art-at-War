using UnityEngine;
using System.Collections;

public class QPrintAction : QAction
{
    string output;
    float reward;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public QPrintAction(string s)
    {
        output = s;
    }

    public void Execute(Unit u)
    {
        //Debug.Log(output);
        u.AP -= 2;
    }

    public float GetReward()
    {
        return reward;
    }
}

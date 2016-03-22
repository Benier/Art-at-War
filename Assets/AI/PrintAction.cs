using UnityEngine;
using System.Collections;

public class PrintAction : Action
{
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

    public void Execute()
    {
        //Debug.Log(output);
    }
}

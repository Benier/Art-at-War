using UnityEngine;
using System.Collections;

/// <summary>
/// Interface for all actions to implement
/// </summary>
public interface Action{

    void Execute(Unit u);
    float GetReward();
    string GetName();
}

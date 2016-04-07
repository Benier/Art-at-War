using UnityEngine;
using System.Collections;

/// <summary>
/// Interface for all QActions to implement
/// </summary>
public interface QAction
{
    void Execute(Unit u);
    float GetReward();
}

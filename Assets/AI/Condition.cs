using UnityEngine;
using System.Collections;

/// <summary>
/// Inferface for all conditions to implement
/// </summary>
public interface Condition{

    bool Test(Unit u);
}

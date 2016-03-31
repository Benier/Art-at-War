using UnityEngine;
using System.Collections;

public class QActionName : MonoBehaviour
{
    public string From;
    public string To;

    public QActionName(string from, string to = null)
    {
        From = from;
        To = to;
    }
   
    public override string ToString()
    {
        return GetActionName();
    }
   
    public string GetActionName()
    {
        if (To == null)
        {
            return From;
        }
        return QMethod.ActionNameFromTo(From, To);
    }

}

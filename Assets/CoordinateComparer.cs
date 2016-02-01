using System.Collections;
using System.Collections.Generic;
using System;

public class CoordinateComparer : IEqualityComparer<Coordinate>
{
    public bool Equals(Coordinate compare, Coordinate compareTo)
    {
        if(compare.GetX() == compareTo.GetX() && compare.GetZ() == compareTo.GetZ())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(Coordinate coord)
    {
        int hCode = (int)((coord.GetX() * 1000) + coord.GetZ());
        return hCode.GetHashCode();
    }
}

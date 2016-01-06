using UnityEngine;
using System.Collections;

public class Coordinate {

    float x;
    float y;
    float z;

    public Coordinate()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Coordinate(float posx, float posz)
    {
        x = posx;
        y = 0;
        z = posz;
    }

    public float GetX()
    {
        return x;
    }

    public void SetX(float posx)
    {
        x = posx;
    }

    public float GetY()
    {
        return y;
    }

    public void SetY(float posy)
    {
        y = posy;
    }

    public float GetZ()
    {
        return z;
    }

    public void SetZ(float posz)
    {
        z = posz;
    }
}

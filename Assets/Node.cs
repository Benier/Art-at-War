using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// THIS IS PART OF THE A* PATHFINDING THAT I NO LONGER NEED.
/// </summary>
public class Node
{
    int PX;
    int PY;
    public Node parent;
    public int X;
    public int Y;
    public int F;
    public int G;
    int H;
    public List<Node> neighbours = new List<Node>();
    public Vector3 Position;

    public Node(int posX, int posY)
    {
        X = posX;
        Y = posY;
        G = int.MaxValue;
        Position = new Vector3(X, 0, Y);
    }

    public void AddNeighbour(Node neigh)
    {
        this.neighbours.Add(neigh);
    }

    public float GetF()
    {
        return F;
    }

    public void SetF(int f)
    {
        F = f;
    }

    public void SetG(int g)
    {
        G = g;
    }

    public int CalculateH(Node goal)
    {
        H = (X - goal.X) + (Y - goal.Y);
        return H;
    }

    public int CalculateF(Node goal)
    {
        F = G + CalculateH(goal);
        return F;
    }

    public bool Compare(Node n)
    {
        if (this == n) return true;

        if((this.X == n.X) && this.Y == n.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

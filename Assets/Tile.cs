using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool occupied;
    public int [,] pixels;
	// Use this for initialization
	void Start () {
        occupied = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public List<List<int>> ToList()
    {
        List<List<int>> list = new List<List<int>>();
        for(int x = 0; x < pixels.GetLength(0); x++)
        {
            List<int> inList = new List<int>();
            for (int y = 0; y < pixels.GetLength(1); y++)
            {
                inList.Add(pixels[x,y]);
            }
            list.Add(inList);
        }
        return list;
    }
}

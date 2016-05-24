using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    public bool occupied;
    public int [,] pixels;
    int occupiedPixels;
    public float percentOpen;
	// Use this for initialization
	void Start () {
        occupied = false;
	}
	
	// Update is called once per frame
	public void UpdatePixels (int x, int y, int faction)
    {
        pixels[x, y] = faction;
        if(faction != 0)
        {
            occupiedPixels++;
        }
        percentOpen = (pixels.GetLength(0) * pixels.GetLength(1)) - occupiedPixels / (pixels.GetLength(0) * pixels.GetLength(1));
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

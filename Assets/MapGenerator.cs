using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    Random random = new Random();

    static int MAP_WIDTH = 40;
    static int MAP_LENGTH = 40;
    static int scale_factor = 1;

    int num_hills = 10;
    float y_max = 1;
    float y_min = 0;
    float max_radius = 4;
    static CoordinateComparer coordComp = new CoordinateComparer();
    public Dictionary<Coordinate, GameObject> map = new Dictionary<Coordinate, GameObject>(coordComp);
    //int[,] map;

    //Dictionary<>
	// Use this for initialization
	void Start ()
    {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void GenerateMap()
    {
        GameObject blockPrefab;
        //map = new int[MAP_WIDTH, MAP_LENGTH];
        for(int x = MAP_WIDTH / 2 * -1; x < MAP_WIDTH / 2; x++)
        {
            for(int z = MAP_LENGTH / 2 * -1; z < MAP_LENGTH/ 2; z++)
            {
                blockPrefab = Instantiate(Resources.Load("MapBlockPrefab", typeof(GameObject))) as GameObject;
                blockPrefab.transform.position = new Vector3(x * scale_factor, 0 * scale_factor, z * scale_factor);
                map.Add(new Coordinate(x, z), blockPrefab);
            }
        }

        int iterations = 0;
        float radius = max_radius;
        
        /*for (int i = 0; i < num_hills; i++)
        {
            float x = Random.Range(MAP_WIDTH / 2 * -1, MAP_WIDTH / 2);
            float z = Random.Range(MAP_LENGTH / 2 * -1, MAP_LENGTH / 2);

            foreach(KeyValuePair<Coordinate, GameObject> g in map)
            {
                float y = Mathf.Pow(radius, 2) - (Mathf.Pow((g.Key.GetX() - x), 2) + Mathf.Pow((g.Key.GetZ() - z), 2));
                if(y > 0)
                {
                    //float ynorm = (y - y_min) / (y_max - y_min);
                    float ynorm = Mathf.InverseLerp(0, 1, y);
                    g.Key.SetY(ynorm);

                    g.Value.transform.position += new Vector3(0, g.Key.GetY(), 0);
                }
            }
            //map[x, z] += 1;
        }*/
        if(map.ContainsKey(new Coordinate(0, 0)))
        {
            Debug.Log("boo");
        }
        map[new Coordinate(0, 0)].transform.position += new Vector3(0, 2 * scale_factor, 0);
        
        return;
    }
}

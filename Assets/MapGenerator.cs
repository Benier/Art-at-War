using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

    Random random = new Random();

    public Texture2D mapTex;
    public int MAP_WIDTH = 160;
    public int MAP_LENGTH = 160;
    static int scale_factor = 1;

    int num_hills = 20;
    float y_max = 1;
    float y_min = 0;
    float max_radius = 4;
    static CoordinateComparer coordComp = new CoordinateComparer();
    public Dictionary<Coordinate, GameObject> map = new Dictionary<Coordinate, GameObject>(coordComp);
    //int[,] map;
    Node[,] nodeGrid;
   
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public int ConvertXToWorld(int num)
    {
        return num + MAP_WIDTH / 2;
    }

    public int ConvertZToWorld(int num)
    {
        return num + MAP_LENGTH / 2;
    }

    public Dictionary<Coordinate, GameObject> GenerateMap()
    {
        GameObject blockPrefab;
        //map = new int[MAP_WIDTH, MAP_LENGTH];
        nodeGrid = new Node[MAP_LENGTH, MAP_WIDTH];
        for(int x = MAP_WIDTH / 2 * -1; x < MAP_WIDTH / 2; x++)
        {
            for(int z = MAP_LENGTH / 2 * -1; z < MAP_LENGTH/ 2; z++)
            {
                blockPrefab = Instantiate(Resources.Load("MapBlockPrefab", typeof(GameObject))) as GameObject;
                blockPrefab.transform.position = new Vector3(x * scale_factor, 0 * scale_factor, z * scale_factor);
                
                map.Add(new Coordinate(x, z), blockPrefab);
                //populate array of Nodes
                nodeGrid[ConvertZToWorld(z), ConvertXToWorld(x)] = new Node(x, z);
            }
        }

        float radius = max_radius;
        
        for (int i = 0; i < num_hills; i++)
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
        }
        if(map.ContainsKey(new Coordinate(0, 0)))
        {
            //Debug.Log("boo");
        }
        //map[new Coordinate(0, 0)].transform.position += new Vector3(0, 2 * scale_factor, 0);
        
        return map;
    }

    public void SetMapTexture(Texture2D tex)
    {
        foreach(KeyValuePair<Coordinate, GameObject> b in map)
        {
            b.Value.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
        }
    }
}

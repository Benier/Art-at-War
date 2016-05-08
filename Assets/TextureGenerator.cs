using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator : MonoBehaviour {

    public enum Faction
    {
        None,
        Player,
        Enemy
    };
    
    public Texture2D inputBaseTexture;
    Texture2D pencilMaskTexture;
    Texture2D pencilBaseTexture;
    Texture2D charcoalMaskTexture;
    Texture2D charcoalBaseTexture;
    Texture2D waterMaskTexture;
    Texture2D waterBaseTexture;
    Texture2D oilMaskTexture;
    Texture2D oilBaseTexture;
    Texture2D tarMaskTexture;
    Texture2D tarBaseTexture;
    [SerializeField]
    GameObject display;

    public ScorePixel[,] pixels;
    public MapGenerator mapGen;
    public TextAsset inputBaseName;

    public Texture2D outputTexture;

    List<TextureHit> hitQueue = new List<TextureHit>();
    TextureHit pencilHit;
    TextureHit tarHit;
    GameLevel1 gameLvl;

    public int correctX;
    public int correctY;
    float xInterval;
    float yInterval;
    // Use this for initialization
    void Start ()
    {
        inputBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_CHARCOAL") as Texture2D;
        pencilMaskTexture = Resources.Load("PencilStrokes") as Texture2D;
        pencilBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_PENCIL") as Texture2D;
        charcoalMaskTexture = Resources.Load("CharcoalStrokes") as Texture2D;
        charcoalBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_CHARCOAL") as Texture2D;
        waterMaskTexture = Resources.Load("WaterStrokes") as Texture2D;
        waterBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_WATER") as Texture2D;
        oilMaskTexture = Resources.Load("OilStrokes") as Texture2D;
        oilBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_OIL") as Texture2D;
        tarMaskTexture = Resources.Load("TarStrokes") as Texture2D;
        tarBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_TAR") as Texture2D;

        //pencilHit = new TextureHit(new Vector3(200, 0, 200), pencilBaseTexture, pencilMaskTexture);
        //tarHit = new TextureHit(new Vector3(300, 600, 400), inputBaseTexture, pencilMaskTexture);
        //hitQueue.Add(pencilHit);
        //hitQueue.Add(tarHit);
        //SetTexture();
        pixels = new ScorePixel[inputBaseTexture.width, inputBaseTexture.height];

        //Set up matrix of pixels that can hold a score value.
        for (int x = 0; x < inputBaseTexture.width; x++)
        {
            for(int y = 0; y < inputBaseTexture.height; y++)
            {
                pixels[x, y] = new ScorePixel();
            }
        }
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        xInterval = inputBaseTexture.width / mapGen.MAP_WIDTH;
        yInterval = inputBaseTexture.height / mapGen.MAP_LENGTH;
        gameLvl = GameObject.Find("GameLvl1").GetComponent<GameLevel1>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKey(KeyCode.L))
        {
            SetTexture();
        }
	}

    public void SetTexture()
    {
        display.GetComponent<Renderer>().material.SetTexture("_MainTex", GenerateTexture());
    }

    /// <summary>
    /// Generate a new texture by generating as many passes are there are hits in the queue.
    /// </summary>
    /// <returns></returns>
    public Texture2D GenerateTexture()
    {
        outputTexture = new Texture2D(inputBaseTexture.width, inputBaseTexture.height);

        for (int i = 0; i < hitQueue.Count; i++)        
        {
            outputTexture = GeneratePass(hitQueue[i], outputTexture);
        }
        return outputTexture;
    }

    /// <summary>
    /// Add Pencil Hit to queue of hits.
    /// </summary>
    /// <param name="pos">Position of hit.</param>
    /// <param name="origin">Position of shooter.</param>
    public void AddPencilHit(Vector3 pos, Vector3 origin)
    {
        Vector3 texPos;
        //Correct the positions by changing it from 0,0 top left to 0, 0 centre, then adding the offset based on the position.
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, pencilBaseTexture, pencilMaskTexture, (int)Faction.Enemy, origin);
        hitQueue.Add(pHit);
    }

    /// <summary>
    /// Add Charcoal Hit to queue of hits.
    /// </summary>
    /// <param name="pos">Position of hit.</param>
    /// <param name="origin">Position of shooter.</param>
    public void AddCharcoalHit(Vector3 pos, Vector3 origin)
    {
        Vector3 texPos;
        //Correct the positions by changing it from 0,0 top left to 0, 0 centre, then adding the offset based on the position.
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, charcoalBaseTexture, charcoalMaskTexture, (int)Faction.Enemy, origin);
        hitQueue.Add(pHit);
    }

    /// <summary>
    /// Add Water colour Hit to queue of hits.
    /// </summary>
    /// <param name="pos">Position of hit.</param>
    /// <param name="origin">Position of shooter.</param>
    public void AddWaterHit(Vector3 pos, Vector3 origin)
    {
        Vector3 texPos;
        //Correct the positions by changing it from 0,0 top left to 0, 0 centre, then adding the offset based on the position.
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, waterBaseTexture, waterMaskTexture, (int)Faction.Player, origin);
        hitQueue.Add(pHit);
    }

    /// <summary>
    /// Add Oil paint Hit to queue of hits.
    /// </summary>
    /// <param name="pos">Position of hit.</param>
    /// <param name="origin">Position of shooter.</param>
    public void AddOilHit(Vector3 pos, Vector3 origin)
    {
        Vector3 texPos;
        //Correct the positions by changing it from 0,0 top left to 0, 0 centre, then adding the offset based on the position.
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, oilBaseTexture, oilMaskTexture, (int)Faction.Player, origin);
        hitQueue.Add(pHit);
    }

    /// <summary>
    /// Add Tar Hit to queue of hits.
    /// </summary>
    /// <param name="pos">Position of hit.</param>
    /// <param name="origin">Position of shooter.</param>
    public void AddTarHit(Vector3 pos, Vector3 origin)
    {
        Vector3 texPos;
        //Correct the positions by changing it from 0,0 top left to 0, 0 centre, then adding the offset based on the position.
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, tarBaseTexture, tarMaskTexture, (int)Faction.None, origin);
        hitQueue.Add(pHit);
    }

    /// <summary>
    /// Converts a 2D position to 3D world position. 
    /// </summary>
    /// <param name="pos">A 2D position</param>
    /// <returns></returns>
    public Vector3 ConvertToWorldCoord(Vector3 pos)
    {
        Vector3 worldPos;
        int worldX = (int)Mathf.FloorToInt((pos.x / xInterval) - mapGen.MAP_WIDTH / 2);
        int worldZ = (int)Mathf.FloorToInt((pos.z / yInterval) - mapGen.MAP_LENGTH / 2);
        worldX = Mathf.Clamp(worldX, -mapGen.MAP_WIDTH / 2, (mapGen.MAP_WIDTH / 2) - 1);
        worldZ = Mathf.Clamp(worldZ, -mapGen.MAP_LENGTH / 2, (mapGen.MAP_LENGTH / 2) - 1);
        worldPos = new Vector3(worldX, 0, worldZ);

        return worldPos;
    }

    /// <summary>
    /// Generates a texture with a new TextureHit added.
    /// </summary>
    /// <param name="hit">New TextureHit to be added to texture.</param>
    /// <param name="outputText">New output texture after generation.</param>
    /// <returns></returns>
    public Texture2D GeneratePass(TextureHit hit, Texture2D outputText)
    {
        Texture2D tempTexture = outputText;//new Texture2D(hit.baseTexture.width, hit.baseTexture.height);
        Texture2D hitMask = hit.maskTexture;
        //TextureScale.Point(hitMask, hitMask.width / (int)hit.distance, hitMask.height / (int)hit.distance);

        //hitMask.Resize(hitMask.width / 1, hitMask.height / 1);
            //(int)hit.distance, hitMask.height / (int)hit.distance);
        for (int x = 0; x < hitMask.width; x++)
        {
            for (int y = 0; y < hitMask.height; y++)
            {
                //If the pixel in the hit is within height and widge bounds of the large texture.
                if (!(hit.position.x - (hitMask.width / 2) + x < 0)
                    && !(hit.position.z - (hitMask.height / 2) + y < 0)
                    && !(hit.position.x - (hitMask.width / 2) + x >= hit.baseTexture.width)
                    && !(hit.position.z - (hitMask.height / 2) + y >= hit.baseTexture.height))
                {
                    //If the pixel's alpha is not transparent
                    if (hitMask.GetPixel(x, y).a != 0)
                    {
                        Color pixel = new Color(hit.baseTexture.GetPixel((int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)).r, hit.baseTexture.GetPixel((int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)).g, hit.baseTexture.GetPixel((int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)).b, 1);//hit.maskTexture.GetPixel(x, y).a);
                        tempTexture.SetPixel((int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y), pixel);
                        Vector3 worldPos = ConvertToWorldCoord(new Vector3((int)(hit.position.x - (hitMask.width / 2) + x), 0, (int)(hit.position.z - (hitMask.height / 2) + y)));

                        //If the map does not contain the position of that pixel, break.
                        if(!mapGen.map.ContainsKey(new Coordinate(worldPos.x, worldPos.z)))
                        {
                            Debug.Log(worldPos.x + ", " + worldPos.z);
                            Debug.Break();
                        }
                        //If the pixel does not belong to any faction, set it to the hit's faction and increment the points accordingly.
                        if (pixels[(int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)].faction == (int)Faction.None)
                        {
                            
                            if (hit.faction == (int)Faction.Player)
                            {
                                
                                gameLvl.playerPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                            else if(hit.faction == (int)Faction.Enemy)
                            {
                                gameLvl.enemyPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                        }
                        //If the pixel does belong to a faction but not the hit's faction, set it to the hit's faction and update the points accordingly.
                        else if(pixels[(int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)].faction != hit.faction)
                        {
                            if (pixels[(int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)].faction == (int)Faction.Player)
                            {
                                gameLvl.playerPoints -= 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                if(hit.faction != (int)Faction.None)
                                {
                                    gameLvl.enemyPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                }
                            }
                            else
                            {
                                gameLvl.enemyPoints -= 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                if (hit.faction != (int)Faction.None)
                                {
                                    gameLvl.playerPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                }
                            }
                        }
                        //Set pixel's faction to hit's faction.
                        pixels[(int)(hit.position.x - (hitMask.width / 2) + x), (int)(hit.position.z - (hitMask.height / 2) + y)].faction = hit.faction;
                    }
                }
                //Color tempCol = new Color(hit.baseTexture.GetPixel(x, y).r, hit.baseTexture.GetPixel(x, y).g, hit.baseTexture.GetPixel(x, y).b, hit.maskTexture.GetPixel(x, y).a);
                //tempTexture.SetPixel(x, y, tempCol);
            }
        
        }
        tempTexture.Apply();
        mapGen.SetMapTexture(tempTexture);
        return tempTexture;
    }
}

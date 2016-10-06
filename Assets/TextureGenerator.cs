using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator : MonoBehaviour {

    public bool loaded;
    public GameObject loadActText;
    public bool generating;
    public enum Faction
    {
        None,
        Player,
        Enemy
    };

    [System.Serializable]
    public class TextureContainer
    {
        public Texture2D texture;
    }
    TextureContainer container;
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
    public TextureHit tempHit;

    List<TextureHit> hitQueue = new List<TextureHit>();
    TextureHit pencilHit;
    TextureHit tarHit;
    GameLevel1 gameLvl;

    public int correctX;
    public int correctY;
    float xInterval; //number of pixels across X per tile
    float yInterval; //number of pixels across Y per tile

    // Use this for initialization
    void Awake ()
    {
        generating = false;
        loaded = true;
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

        outputTexture = new Texture2D(inputBaseTexture.width, inputBaseTexture.height);
        container = new TextureContainer();
        container.texture = outputTexture;
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
    /// Using Coroutine, generate a new texture by generating as many passes are there are hits in the queue.
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineGenerateTexture()
    {
        for (int i = 0; i < hitQueue.Count; i++)
        {
            //outputTexture = GeneratePass(hitQueue[i], outputTexture);
            yield return outputTexture;
        }
    }

    /// <summary>
    /// Generate a new texture by generating as many passes are there are hits in the queue.
    /// </summary>
    /// <returns></returns>
    public Texture2D GenerateTexture()
    {
        loaded = false;
        //loadActText.SetActive(true);
        for (int i = 0; i < hitQueue.Count; i++)        
        {
            tempHit = hitQueue[i];
            //outputTexture = container.texture;
            StartCoroutine("GeneratePass");
            //StartCoroutine(CoroutineGeneratePass(hitQueue[i], outputTexture, container));
            
        }
        loaded = true;
        loadActText.SetActive(false);
        return outputTexture;
    }

    public void ThreadGenerateTexture()
    {
        GenerateTexture();
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
    public IEnumerator GeneratePass()
    {
        Texture2D tempTexture = outputTexture;//new Texture2D(hit.baseTexture.width, hit.baseTexture.height);
        //TextureScale.Point(hitMask, hitMask.width / (int)hit.distance, hitMask.height / (int)hit.distance);

        //hitMask.Resize(hitMask.width / 1, hitMask.height / 1);
        //(int)hit.distance, hitMask.height / (int)hit.distance);
        for (int x = 0; x < tempHit.maskWidth; x++)
        {
            for (int y = 0; y < tempHit.maskHeight; y++)
            {
                //If the pixel in the hit is within height and widge bounds of the large texture.
                if (!(tempHit.position.x - (tempHit.maskWidth / 2) + x < 0)
                    && !(tempHit.position.z - (tempHit.maskHeight / 2) + y < 0)
                    && !(tempHit.position.x - (tempHit.maskWidth / 2) + x >= tempHit.texWidth)
                    && !(tempHit.position.z - (tempHit.maskHeight / 2) + y >= tempHit.texHeight))
                {
                    //If the pixel's alpha is not transparent
                    if (tempHit.maskTexture.GetPixel(x, y).a != 0)
                    {
                        Color pixel = new Color(tempHit.baseTexture.GetPixel((int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)).r, tempHit.baseTexture.GetPixel((int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)).g, tempHit.baseTexture.GetPixel((int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)).b, 1);//hit.maskTexture.GetPixel(x, y).a);
                        tempTexture.SetPixel((int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y), pixel);
                        Vector3 worldPos = ConvertToWorldCoord(new Vector3((int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), 0, (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)));

                        //If the map does not contain the position of that pixel, break.
                        if(!mapGen.map.ContainsKey(new Coordinate(worldPos.x, worldPos.z)))
                        {
                            Debug.Log(worldPos.x + ", " + worldPos.z);
                            Debug.Break();
                        }
                        //If the pixel does not belong to any faction, increment the points accordingly.
                        if (pixels[(int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)].faction == (int)Faction.None)
                        {
                            
                            if (tempHit.faction == (int)Faction.Player)
                            {
                                
                                gameLvl.playerPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                            else if(tempHit.faction == (int)Faction.Enemy)
                            {
                                gameLvl.enemyPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                        }
                        //If the pixel does belong to a faction but not the hit's faction, update the points accordingly.
                        else if(pixels[(int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)].faction != tempHit.faction)
                        {
                            if (pixels[(int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)].faction == (int)Faction.Player)
                            {
                                gameLvl.playerPoints -= 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                if(tempHit.faction != (int)Faction.None)
                                {
                                    gameLvl.enemyPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                }
                            }
                            else
                            {
                                gameLvl.enemyPoints -= 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                if (tempHit.faction != (int)Faction.None)
                                {
                                    gameLvl.playerPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                }
                            }
                        }
                        //Set pixel's faction to hit's faction.
                        pixels[(int)(tempHit.position.x - (tempHit.maskWidth / 2) + x), (int)(tempHit.position.z - (tempHit.maskHeight / 2) + y)].faction = tempHit.faction;
                        int tileLocalX = (int)((tempHit.position.x - (tempHit.maskWidth / 2) + x) % xInterval);
                        int tileLocalY = (int)((tempHit.position.z - (tempHit.maskWidth / 2) + y) % yInterval);
                        mapGen.map[new Coordinate(worldPos.x, worldPos.z)].GetComponent<Tile>().UpdatePixels(tileLocalX, tileLocalY, tempHit.faction);
                        List<List<int>> debugLIst = mapGen.map[new Coordinate(worldPos.x, worldPos.z)].GetComponent<Tile>().ToList();
                        yield return null;
                    }
                    
                }
                //Color tempCol = new Color(hit.baseTexture.GetPixel(x, y).r, hit.baseTexture.GetPixel(x, y).g, hit.baseTexture.GetPixel(x, y).b, hit.maskTexture.GetPixel(x, y).a);
                //tempTexture.SetPixel(x, y, tempCol);
            }
        
        }
        tempTexture.Apply();
        mapGen.SetMapTexture(tempTexture);
        outputTexture = tempTexture;
        generating = false;
        //return tempTexture;
    }


    /// <summary>
    /// Using coroutine, generates a texture with a new TextureHit added.
    /// </summary>
    /// <param name="hit">New TextureHit to be added to texture.</param>
    /// <param name="outputText">New output texture after generation.</param>
    /// <returns></returns>
    public IEnumerator CoroutineGeneratePass(TextureHit hit, Texture2D outputText, TextureContainer container)
    {
        Texture2D tempTexture = outputText;//new Texture2D(hit.baseTexture.width, hit.baseTexture.height);
        //TextureScale.Point(hitMask, hitMask.width / (int)hit.distance, hitMask.height / (int)hit.distance);

        //hitMask.Resize(hitMask.width / 1, hitMask.height / 1);
        //(int)hit.distance, hitMask.height / (int)hit.distance);
        for (int x = 0; x < hit.maskWidth; x++)
        {
            for (int y = 0; y < hit.maskHeight; y++)
            {
                //If the pixel in the hit is within height and widge bounds of the large texture.
                if (!(hit.position.x - (hit.maskWidth / 2) + x < 0)
                    && !(hit.position.z - (hit.maskHeight / 2) + y < 0)
                    && !(hit.position.x - (hit.maskWidth / 2) + x >= hit.texWidth)
                    && !(hit.position.z - (hit.maskHeight / 2) + y >= hit.texHeight))
                {
                    //If the pixel's alpha is not transparent
                    if (hit.maskTexture.GetPixel(x, y).a != 0)
                    {
                        Color pixel = new Color(hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)).r, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)).g, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)).b, 1);//hit.maskTexture.GetPixel(x, y).a);
                        tempTexture.SetPixel((int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y), pixel);
                        Vector3 worldPos = ConvertToWorldCoord(new Vector3((int)(hit.position.x - (hit.maskWidth / 2) + x), 0, (int)(hit.position.z - (hit.maskHeight / 2) + y)));

                        //If the map does not contain the position of that pixel, break.
                        if (!mapGen.map.ContainsKey(new Coordinate(worldPos.x, worldPos.z)))
                        {
                            Debug.Log(worldPos.x + ", " + worldPos.z);
                            Debug.Break();
                        }
                        //If the pixel does not belong to any faction, increment the points accordingly.
                        if (pixels[(int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)].faction == (int)Faction.None)
                        {

                            if (hit.faction == (int)Faction.Player)
                            {

                                gameLvl.playerPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                            else if (hit.faction == (int)Faction.Enemy)
                            {
                                gameLvl.enemyPoints += 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                            }
                        }
                        //If the pixel does belong to a faction but not the hit's faction, update the points accordingly.
                        else if (pixels[(int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)].faction != hit.faction)
                        {
                            if (pixels[(int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)].faction == (int)Faction.Player)
                            {
                                gameLvl.playerPoints -= 1 * (int)(mapGen.map[new Coordinate(worldPos.x, worldPos.z)].transform.position.y + 1);
                                if (hit.faction != (int)Faction.None)
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
                        pixels[(int)(hit.position.x - (hit.maskWidth / 2) + x), (int)(hit.position.z - (hit.maskHeight / 2) + y)].faction = hit.faction;
                        int tileLocalX = (int)((hit.position.x - (hit.maskWidth / 2) + x) % xInterval);
                        int tileLocalY = (int)((hit.position.z - (hit.maskWidth / 2) + y) % yInterval);
                        mapGen.map[new Coordinate(worldPos.x, worldPos.z)].GetComponent<Tile>().UpdatePixels(tileLocalX, tileLocalY, hit.faction);
                        List<List<int>> debugLIst = mapGen.map[new Coordinate(worldPos.x, worldPos.z)].GetComponent<Tile>().ToList();
                    }
                }
                //Color tempCol = new Color(hit.baseTexture.GetPixel(x, y).r, hit.baseTexture.GetPixel(x, y).g, hit.baseTexture.GetPixel(x, y).b, hit.maskTexture.GetPixel(x, y).a);
                //tempTexture.SetPixel(x, y, tempCol);
            }

        }
        tempTexture.Apply();
        mapGen.SetMapTexture(tempTexture);
        container.texture = tempTexture;
        yield return null;
    }
}

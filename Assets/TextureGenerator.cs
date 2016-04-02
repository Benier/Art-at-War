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
    void Start () {
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

    public Texture2D GenerateTexture()
    {
        outputTexture = new Texture2D(inputBaseTexture.width, inputBaseTexture.height);

        for (int i = 0; i < hitQueue.Count; i++)        
        {
            outputTexture = GeneratePass(hitQueue[i], outputTexture);
        }
        return outputTexture;
    }

    public void AddPencilHit(Vector3 pos)
    {
        Vector3 texPos;
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, pencilBaseTexture, pencilMaskTexture, (int)Faction.Enemy);
        hitQueue.Add(pHit);
    }

    public void AddCharcoalHit(Vector3 pos)
    {
        Vector3 texPos;
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, charcoalBaseTexture, charcoalMaskTexture, (int)Faction.Enemy);
        hitQueue.Add(pHit);
    }

    public void AddWaterHit(Vector3 pos)
    {
        Vector3 texPos;
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, waterBaseTexture, waterMaskTexture, (int)Faction.Player);
        hitQueue.Add(pHit);
    }

    public void AddOilHit(Vector3 pos)
    {
        Vector3 texPos;
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, oilBaseTexture, oilMaskTexture, (int)Faction.Player);
        hitQueue.Add(pHit);
    }

    public void AddTarHit(Vector3 pos)
    {
        Vector3 texPos;
        correctX = (int)((pos.x + mapGen.MAP_WIDTH / 2) * xInterval);//(int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        correctY = (int)((pos.z + mapGen.MAP_LENGTH / 2) * yInterval);//(int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(texPos, tarBaseTexture, tarMaskTexture, (int)Faction.None);
        hitQueue.Add(pHit);
    }

    public Vector3 ConvertToWorldCoord(Vector3 pos)
    {
        Vector3 worldPos;
        int worldX = (int)((pos.x / xInterval) - mapGen.MAP_WIDTH / 2);
        int worldZ = (int)((pos.z / yInterval) - mapGen.MAP_LENGTH / 2);
        worldPos = new Vector3(worldX, 0, worldZ);

        return worldPos;
    }

    public Texture2D GeneratePass(TextureHit hit, Texture2D outputText)
    {
        Texture2D tempTexture = outputText;//new Texture2D(hit.baseTexture.width, hit.baseTexture.height);
        for (int x = 0; x < hit.maskTexture.width; x++)
        {
            for (int y = 0; y < hit.maskTexture.height; y++)
            {
                if (!(hit.position.x - (hit.maskTexture.width / 2) + x < 0)
                    && !(hit.position.z - (hit.maskTexture.height / 2) + y < 0)
                    && !(hit.position.x - (hit.maskTexture.width / 2) + x >= hit.baseTexture.width)
                    && !(hit.position.z - (hit.maskTexture.height / 2) + y >= hit.baseTexture.height))
                {
                    if (hit.maskTexture.GetPixel(x, y).a != 0)
                    {
                        Color pixel = new Color(hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).r, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).g, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).b, 1);//hit.maskTexture.GetPixel(x, y).a);
                        tempTexture.SetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y), pixel);
                        Vector3 worldPos = ConvertToWorldCoord(new Vector3((int)(hit.position.x - (hit.maskTexture.width / 2) + x), 0, (int)(hit.position.z - (hit.maskTexture.height / 2) + y)));

                        if (pixels[(int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)].faction == (int)Faction.None)
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
                        else if(pixels[(int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)].faction != hit.faction)
                        {
                            if (pixels[(int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)].faction == (int)Faction.Player)
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
                        pixels[(int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)].faction = hit.faction;
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

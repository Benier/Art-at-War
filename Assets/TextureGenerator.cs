using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator : MonoBehaviour {

    Texture2D inputBaseTexture;
    Texture2D pencilMaskTexture;
    Texture2D pencilBaseTexture;
    [SerializeField]
    GameObject display;

    MapGenerator mapGen;
    public TextAsset inputBaseName;

    public Texture2D outputTexture;

    List<TextureHit> hitQueue = new List<TextureHit>();
    TextureHit pencilHit;
    TextureHit tarHit;
	// Use this for initialization
	void Start () {
        inputBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_CHARCOAL") as Texture2D;
        pencilMaskTexture = Resources.Load("PencilStrokes") as Texture2D;
        pencilBaseTexture = Resources.Load("A_la_Recherche_du_Temps_Perdu_OIL") as Texture2D;

        pencilHit = new TextureHit(new Vector3(200, 500, 200), pencilBaseTexture, pencilMaskTexture);
        tarHit = new TextureHit(new Vector3(300, 600, 400), inputBaseTexture, pencilMaskTexture);
        //hitQueue.Add(pencilHit);
        //hitQueue.Add(tarHit);
        //SetTexture();
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
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
        float xInterval = inputBaseTexture.width / mapGen.MAP_WIDTH;
        float yInterval = inputBaseTexture.height / mapGen.MAP_LENGTH;
        int correctX = (int)((pos.x * xInterval) + ((mapGen.MAP_WIDTH * xInterval) / 2));
        int correctY = (int)((pos.z * yInterval) + ((mapGen.MAP_LENGTH * yInterval) / 2));
        texPos = new Vector3(correctX, 0, correctY);

        TextureHit pHit = new TextureHit(pos, pencilBaseTexture, pencilMaskTexture);
        hitQueue.Add(pHit);
    }

    public Texture2D GeneratePass(TextureHit hit, Texture2D outputText)
    {
        Texture2D tempTexture = outputText;//new Texture2D(hit.baseTexture.width, hit.baseTexture.height);
        for (int x = 0; x < hit.maskTexture.width; x++)
        {
            for (int y = 0; y < hit.maskTexture.height; y++)
            {
                if (!(hit.position.x - (hit.maskTexture.width / 2) + x < 0)
                    && !(hit.position.y - (hit.maskTexture.height / 2) + y < 0)
                    && !(hit.position.x - (hit.maskTexture.width / 2) + x >= hit.baseTexture.width)
                    && !(hit.position.y - (hit.maskTexture.height / 2) + y >= hit.baseTexture.height))
                {
                    if (hit.maskTexture.GetPixel(x, y).a != 0)
                    {
                        Color pixel = new Color(hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).r, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).g, hit.baseTexture.GetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y)).b, 1);//hit.maskTexture.GetPixel(x, y).a);
                        tempTexture.SetPixel((int)(hit.position.x - (hit.maskTexture.width / 2) + x), (int)(hit.position.z - (hit.maskTexture.height / 2) + y), pixel);
                    }
                }
                //Color tempCol = new Color(hit.baseTexture.GetPixel(x, y).r, hit.baseTexture.GetPixel(x, y).g, hit.baseTexture.GetPixel(x, y).b, hit.maskTexture.GetPixel(x, y).a);
                //tempTexture.SetPixel(x, y, tempCol);
            }
        
        }
        tempTexture.Apply();
        return tempTexture;
    }
}

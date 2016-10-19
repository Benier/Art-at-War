using UnityEngine;
using System.Collections;

public class LoadingActionTexture : MonoBehaviour
{
    public Texture2D texture;
    static LoadingActionTexture instance;
    TextureGenerator texGen;

    void Awake()
    {
        instance = this;
        gameObject.AddComponent<GUITexture>().enabled = false;
        gameObject.GetComponent<GUITexture>().texture = texture;
        transform.position = new Vector3(0.0f, 0.0f, 5.0f);
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        DontDestroyOnLoad(this);
    }

    public void Load(int index)
    {
        if(texGen.generating)
        {
            instance.GetComponent<GUITexture>().enabled = true;
        }
        else
        {
            instance.GetComponent<GUITexture>().enabled = false;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

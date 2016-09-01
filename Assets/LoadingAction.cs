using UnityEngine;
using System.Collections;

public class LoadingAction : MonoBehaviour
{
    public TextureGenerator texGen;
    public Texture2D texture;

    void Awake()
    {
        gameObject.SetActive(false);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

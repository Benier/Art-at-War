using UnityEngine;
using System.Collections;

public class PencilProjectile : MonoBehaviour {

    TextureGenerator texGen;
	// Use this for initialization
	void Start () {
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        texGen.AddPencilHit(collision.contacts[0].point);
        texGen.GenerateTexture();
        gameObject.SetActive(false);
    }
}

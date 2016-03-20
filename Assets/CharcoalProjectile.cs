using UnityEngine;
using System.Collections;

public class CharcoalProjectile : MonoBehaviour
{

    TextureGenerator texGen;
    GameObject display;
    // Use this for initialization
    void Start()
    {
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        display = GameObject.Find("Cube");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        texGen.AddCharcoalHit(collision.contacts[0].point);
        display.GetComponent<Renderer>().material.SetTexture("_MainTex", texGen.GenerateTexture());
        gameObject.SetActive(false);
    }
}

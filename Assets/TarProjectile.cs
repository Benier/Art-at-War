using UnityEngine;
using System.Collections;
/// <summary>
/// Projectile for Tar faction. Triggers texture generation when collision with other GameObjects occur.
/// </summary>
public class TarProjectile : MonoBehaviour
{

    TextureGenerator texGen;
    //GameObject display;
    Vector3 origin;

    // Use this for initialization
    void Start()
    {
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        //display = GameObject.Find("Cube");
        origin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        texGen.AddTarHit(collision.contacts[0].point, origin);
        //display.GetComponent<Renderer>().material.SetTexture("_MainTex", texGen.GenerateTexture());
        gameObject.SetActive(false);
    }
}

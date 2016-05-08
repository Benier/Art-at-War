using UnityEngine;
using System.Collections;
/// <summary>
/// Projectile for Oil paints faction. Triggers texture generation when collision with other GameObjects occur.
/// </summary>
public class OilProjectile : MonoBehaviour
{

    TextureGenerator texGen;
    Vector3 origin;
    // Use this for initialization
    void Start()
    {
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        origin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        texGen.AddOilHit(collision.contacts[0].point, origin);
        texGen.GenerateTexture();
        gameObject.SetActive(false);
    }
}

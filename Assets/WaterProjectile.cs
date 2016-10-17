using UnityEngine;
using System.Collections;
/// <summary>
/// Projectile for water colour faction. Triggers texture generation when collision with other GameObjects occur.
/// </summary>
public class WaterProjectile : MonoBehaviour
{

    TextureGenerator texGen;
    Vector3 origin;
    public TextureGenerator.Faction faction;
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
        texGen.generating = true;
        texGen.AddWaterHit(collision.contacts[0].point, origin, faction);
        texGen.GenerateTexture();
        //StartCoroutine(texGen.CoroutineGenerateTexture());
        gameObject.SetActive(false);
    }
}

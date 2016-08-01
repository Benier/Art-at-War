using UnityEngine;
using System.Collections;
using System.Threading;
/// <summary>
/// Projectile for Oil paints faction. Triggers texture generation when collision with other GameObjects occur.
/// </summary>
public class OilProjectile : MonoBehaviour
{

    TextureGenerator texGen;
    Vector3 origin;
    Thread texGenThread;
    // Use this for initialization
    void Start()
    {
        texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        origin = gameObject.transform.position;
        texGenThread = new Thread(new ThreadStart(texGen.ThreadGenerateTexture));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        texGen.AddOilHit(collision.contacts[0].point, origin);
        //
        //if (!texGenThread.IsAlive)
        //{
        //    texGenThread.Start();
        //}

        texGen.GenerateTexture();
        //StartCoroutine(texGen.CoroutineGenerateTexture());
        gameObject.SetActive(false);
    }
}

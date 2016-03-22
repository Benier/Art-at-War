using UnityEngine;
using System.Collections;

public class TextureHit
{
    public Vector3 position;
    public Texture2D baseTexture;
    public Texture2D maskTexture;
    public int faction;

    public TextureHit(Vector3 pos, Texture2D baseText, Texture2D maskText, int fact)
    {
        position = pos;
        baseTexture = baseText;
        maskTexture = maskText;
        faction = fact;
    }
}

using UnityEngine;
using System.Collections;
/// <summary>
/// Represents a blot of texture added to the texture being generated.
/// </summary>
public class TextureHit
{
    public Vector3 position;
    public Texture2D baseTexture;
    public Texture2D maskTexture;
    public int faction;
    public Vector3 shotOrigin;
    public float distance;
    public int maskWidth;
    public int maskHeight;
    public int texWidth;
    public int texHeight;

    public TextureHit(Vector3 pos, Texture2D baseText, Texture2D maskText, int fact, Vector3 shotOrig)
    {
        position = pos;
        baseTexture = baseText;
        maskTexture = maskText;
        faction = fact;
        shotOrigin = shotOrig;
        distance = Vector3.Distance(shotOrigin, position);
        maskWidth = maskText.width;
        maskHeight = maskText.height;
        texWidth = baseText.width;
        texHeight = baseText.height;
    }
}

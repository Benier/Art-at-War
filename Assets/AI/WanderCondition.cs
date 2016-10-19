using UnityEngine;
using System.Collections;
/// <summary>
/// Condition class for WanderAction. Triggered when the potential rating (ration for blank space) is below 35%.
/// </summary>
public class WanderCondition : Condition {
    float threshold = 0.35f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool Test(Unit u, TextureGenerator texGen)
    {
        if (GetPotential(u, texGen) <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float GetPotential(Unit u, TextureGenerator texGen)
    {
        float potential = 0.0f;
        float totalPixels;
        float xInterval = texGen.inputBaseTexture.width / texGen.mapGen.MAP_WIDTH;
        float yInterval = texGen.inputBaseTexture.height / texGen.mapGen.MAP_LENGTH;
        int correctX = (int)((u.transform.position.x + texGen.mapGen.MAP_WIDTH / 2) * xInterval);
        int correctY = (int)((u.transform.position.z + texGen.mapGen.MAP_LENGTH / 2) * yInterval);
        int pixelRange = (int)((u.attRange + texGen.mapGen.MAP_WIDTH / 2) * xInterval);

        for (int x = correctX - pixelRange; x < correctX + pixelRange; x++)
        {
            for (int y = correctY - pixelRange; y < correctY + pixelRange; y++)
            {
                if (!(x < 0) && !(x >= texGen.inputBaseTexture.width)
                    && !(y < 0) && !(y >= texGen.inputBaseTexture.height))
                {
                    if (texGen.pixels[x, y].faction == (int)TextureGenerator.Faction.None)
                    {
                        potential++;
                    }
                }
            }
        }
        totalPixels = ((correctX + pixelRange) - (correctX - pixelRange)) * ((correctY + pixelRange) - (correctY - pixelRange));

        return potential / (float)totalPixels;
    }
}

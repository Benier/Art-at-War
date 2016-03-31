using UnityEngine;
using System.Collections;

public class AttackAction : Action {
    MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    TextureGenerator texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Execute(Unit u)
    {
        GameObject target = new GameObject();
        float x; // = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z; // = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (GetNWScorePotential(u) > 0.70f)
        {
            x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, 0);
            z = Random.Range(0, mapGen.MAP_LENGTH / 2);

        }
        else if (GetNEScorePotential(u) > 0.70f)
        {
            x = Random.Range(0, mapGen.MAP_WIDTH / 2);
            z = Random.Range(0, mapGen.MAP_LENGTH / 2);
        }
        else if (GetSWScorePotential(u) > 0.70f)
        {
            x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, 0);
            z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, 0);
        }
        else
        {
            x = Random.Range(0, mapGen.MAP_WIDTH / 2);
            z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, 0);

        }
        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);

        target.transform.position = new Vector3(x, 0, z);
        //u.ability = Unit.Ability.Attack;
        u.AttackTarget(target);
        //Debug.Log("Attacking");
    }

    float GetNWScorePotential(Unit u)
    {
        float potential = 0.0f;
        float totalPixels;
        float xInterval = texGen.inputBaseTexture.width / texGen.mapGen.MAP_WIDTH;
        float yInterval = texGen.inputBaseTexture.height / texGen.mapGen.MAP_LENGTH;
        int correctX = (int)((u.transform.position.x + texGen.mapGen.MAP_WIDTH / 2) * xInterval);
        int correctY = (int)((u.transform.position.z + texGen.mapGen.MAP_LENGTH / 2) * yInterval);
        int pixelRange = (int)((u.attRange * 1.5 + texGen.mapGen.MAP_WIDTH / 2) * xInterval);

        for (int x = correctX - pixelRange; x < correctX; x++)
        {
            for (int y = correctY - pixelRange; y < correctY; y++)
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
        totalPixels = ((correctX) - (correctX - pixelRange)) * ((correctY) - (correctY - pixelRange));

        return potential / (float)totalPixels;
    }

    float GetNEScorePotential(Unit u)
    {
        float potential = 0.0f;
        float totalPixels;
        float xInterval = texGen.inputBaseTexture.width / texGen.mapGen.MAP_WIDTH;
        float yInterval = texGen.inputBaseTexture.height / texGen.mapGen.MAP_LENGTH;
        int correctX = (int)((u.transform.position.x + texGen.mapGen.MAP_WIDTH / 2) * xInterval);
        int correctY = (int)((u.transform.position.z + texGen.mapGen.MAP_LENGTH / 2) * yInterval);
        int pixelRange = (int)((u.attRange * 1.5 + texGen.mapGen.MAP_WIDTH / 2) * xInterval);

        for (int x = correctX; x < correctX + pixelRange; x++)
        {
            for (int y = correctY - pixelRange; y < correctY; y++)
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
        totalPixels = ((correctX + pixelRange) - (correctX)) * ((correctY) - (correctY - pixelRange));

        return potential / (float)totalPixels;
    }

    float GetSWScorePotential(Unit u)
    {
        float potential = 0.0f;
        float totalPixels;
        float xInterval = texGen.inputBaseTexture.width / texGen.mapGen.MAP_WIDTH;
        float yInterval = texGen.inputBaseTexture.height / texGen.mapGen.MAP_LENGTH;
        int correctX = (int)((u.transform.position.x + texGen.mapGen.MAP_WIDTH / 2) * xInterval);
        int correctY = (int)((u.transform.position.z + texGen.mapGen.MAP_LENGTH / 2) * yInterval);
        int pixelRange = (int)((u.attRange * 1.5 + texGen.mapGen.MAP_WIDTH / 2) * xInterval);

        for (int x = correctX - pixelRange; x < correctX; x++)
        {
            for (int y = correctY; y < correctY + pixelRange; y++)
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
        totalPixels = ((correctX) - (correctX - pixelRange)) * ((correctY + pixelRange) - (correctY));

        return potential / (float)totalPixels;
    }

    float GetSEScorePotential(Unit u)
    {
        float potential = 0.0f;
        float totalPixels;
        float xInterval = texGen.inputBaseTexture.width / texGen.mapGen.MAP_WIDTH;
        float yInterval = texGen.inputBaseTexture.height / texGen.mapGen.MAP_LENGTH;
        int correctX = (int)((u.transform.position.x + texGen.mapGen.MAP_WIDTH / 2) * xInterval);
        int correctY = (int)((u.transform.position.z + texGen.mapGen.MAP_LENGTH / 2) * yInterval);
        int pixelRange = (int)((u.attRange * 1.5 + texGen.mapGen.MAP_WIDTH / 2) * xInterval);

        for (int x = correctX; x < correctX + pixelRange; x++)
        {
            for (int y = correctY; y < correctY + pixelRange; y++)
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
        totalPixels = ((correctX + pixelRange) - (correctX)) * ((correctY + pixelRange) - (correctY));

        return potential / (float)totalPixels;
    }
}

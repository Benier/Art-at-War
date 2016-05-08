using UnityEngine;
using System.Collections;

public class AttackSEAction : Action
{
    float reward;
    public enum Direction
    {
        None,
        NW,
        NE,
        SW,
        SE
    };

    MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    TextureGenerator texGen = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
    Direction dir;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Execute(Unit u)
    {
        GameObject target = new GameObject();
        float x = u.gameObject.transform.position.x; // = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = u.gameObject.transform.position.z; // = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);

        GetSEScorePotential(u);
        x = Random.Range(0, mapGen.MAP_WIDTH / 2);
        z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, 0);

        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);

        target.transform.position = new Vector3(x, 0, z);
        //u.ability = Unit.Ability.Attack;
        u.AttackTarget(target);
        Debug.Log("Attacking SE");
    }

    Direction GetBestDir(Unit u)
    {
        Direction tempDir = Direction.None;
        if (GetNWScorePotential(u) >= GetNEScorePotential(u)
            && GetNWScorePotential(u) >= GetSWScorePotential(u)
            && GetNWScorePotential(u) >= GetSEScorePotential(u))
        {
            tempDir = Direction.NW;
        }
        if (GetNEScorePotential(u) >= GetNWScorePotential(u)
            && GetNEScorePotential(u) >= GetSWScorePotential(u)
            && GetNEScorePotential(u) >= GetSEScorePotential(u))
        {
            tempDir = Direction.NE;
        }
        if (GetSWScorePotential(u) >= GetNWScorePotential(u)
            && GetSWScorePotential(u) >= GetNEScorePotential(u)
            && GetSWScorePotential(u) >= GetSEScorePotential(u))
        {
            tempDir = Direction.SW;
        }
        if (GetSEScorePotential(u) >= GetNWScorePotential(u)
            && GetSEScorePotential(u) >= GetNEScorePotential(u)
            && GetSEScorePotential(u) >= GetSWScorePotential(u))
        {
            tempDir = Direction.SE;
        }
        return tempDir;
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
                    else
                    {
                        potential--;
                    }
                    reward = potential;
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
                    else
                    {
                        potential--;
                    }
                    reward = potential;
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
                    else
                    {
                        potential--;
                    }
                    reward = potential;
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
                    else
                    {
                        potential--;
                    }
                    reward = potential;
                }
            }
        }
        totalPixels = ((correctX + pixelRange) - (correctX)) * ((correctY + pixelRange) - (correctY));

        return potential / (float)totalPixels;
    }

    public float GetReward()
    {
        return reward;
    }
}

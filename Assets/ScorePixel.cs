using System.Collections;
/// <summary>
/// Allows pixels to hold a faction. Faction 0 is no faction, 1 is player, 2 is enemy.
/// </summary>
public class ScorePixel
{

    public int faction;

    public ScorePixel()
    {
        faction = 0;
    }

    public void SetFaction(int fact)
    {
        faction = fact;
    }
}

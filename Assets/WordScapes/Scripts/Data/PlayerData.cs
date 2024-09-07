using System;

[Serializable]
public class PlayerData
{
    public int unlockedLevel;
    public int coin;
    public int diamond;
    public int brilliance;

    public int numIdea;
    public int numPoint;
    public int numRocket;

    public bool musicOn;
    public bool soundOn;

    public PlayerData ()
    {
        unlockedLevel = 1;
        coin = 200;
        diamond = 0;
        brilliance = 0;

        numIdea = 3;
        numPoint = 3;
        numRocket = 1;

        musicOn = true;
        soundOn = true;
    }

    public void SetData (int unlockedLevel, int coin, int diamond, int brilliance, 
        int numIdea, int numPoint, int numRocket,
        bool musicOn, bool soundOn)
    {
        this.unlockedLevel = unlockedLevel;
        this.coin = coin;
        this.diamond = diamond;
        this.brilliance = brilliance;

        this.numIdea = numIdea;
        this.numPoint = numPoint;
        this.numRocket = numRocket;

        this.musicOn = musicOn;
        this.soundOn = soundOn;
    }
}

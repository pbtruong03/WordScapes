using System;

[Serializable]
public class RankData : IComparable<RankData>
{
    public string playerName;
    public int totalBrilliance;

    public RankData(string playerName, int totalBrilliance)
    {
        this.playerName = playerName;
        this.totalBrilliance = totalBrilliance;
    }

    public int CompareTo(RankData other)
    {
        return other.totalBrilliance - this.totalBrilliance;
    }
}

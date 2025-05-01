using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public float money = 0;
    public int jobXP = 0;
    public string currentJob = "unemployed";
    public List<ResumeEntry> resumeEntries = new();
}

[Serializable]
public class ResumeEntry
{
    public string jobID = "unemployed";
    public int duration = 0;
    public float earnedMoney = 0;
    public int earnedXP = 0;
}
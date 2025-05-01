using System;
using TMPro;
using UnityEngine;

public class JobResumeItem : MonoBehaviour
{
    public TextMeshProUGUI jobNameText, durationText, earnedMoneyText, earnedXPText;

    public ResumeEntry entry;
    
    private void Start()
    {
        jobNameText.text = PlayerDataManager.jobs[entry.jobID].name;
        durationText.text = $"Duration: {entry.duration} Days";
        earnedMoneyText.text = $"Money Earned: ${entry.earnedMoney}";
        earnedXPText.text = $"Job XP Earned: {entry.earnedXP} XP";
    }
}

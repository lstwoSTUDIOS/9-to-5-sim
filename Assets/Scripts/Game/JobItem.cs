using System;
using Jobs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JobItem : MonoBehaviour
{
    public TextMeshProUGUI jobNameText, baseWageText, xpRewardPerShift, shiftLength, requiredXpText;
    [FormerlySerializedAs("button")] public Button applyButton;

    [NonSerialized]
    public JobScriptableObject job;
    
    private void Start()
    {
        jobNameText.text = job.name;
        baseWageText.text = $"Base Wage: ${job.baseWage}";
        xpRewardPerShift.text = $"XP per Shift: {job.xpReward} XP";
        shiftLength.text = $"Shift Length: {Mathf.Round(job.shiftLength / 60)}:{(Mathf.Round(job.shiftLength % 60) < 10 ? $"0{Mathf.Round(job.shiftLength % 60)}" : Mathf.Round(job.shiftLength % 60))}";
        requiredXpText.text = $"{job.xpRequired} XP";
        applyButton.interactable = PlayerDataManager.playerData.jobXP >= job.xpRequired;
    }
}

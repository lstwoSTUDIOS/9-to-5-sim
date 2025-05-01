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
    public JobScriptableObject Job;
    
    private void Start()
    {
        jobNameText.text = Job.data.name;
        baseWageText.text = $"Base Wage: ${Job.data.baseWage}";
        xpRewardPerShift.text = $"XP per Shift: {Job.data.xpReward} XP";
        shiftLength.text = $"Shift Length: {Mathf.Round(Job.data.shiftLength / 60)}:{(Mathf.Round(Job.data.shiftLength % 60) < 10 ? $"0{Mathf.Round(Job.data.shiftLength % 60)}" : Mathf.Round(Job.data.shiftLength % 60))}";
        requiredXpText.text = $"{Job.data.xpRequired} XP";
        applyButton.interactable = PlayerDataManager.playerData.jobXP >= Job.data.xpRequired || PlayerDataManager.playerData.currentJob == Job.data.id;
    }

    public void ApplyToJob()
    {
        if (PlayerDataManager.playerData.jobXP < Job.data.xpRequired &&
            PlayerDataManager.playerData.currentJob != Job.data.id)
        {
            return;
        }
        
        PlayerDataManager.playerData.jobXP -= Job.data.xpRequired;
        PlayerDataManager.playerData.currentJob = Job.data.id;
        
        MainMenuManager.Instance.RefreshJob();
    }
}

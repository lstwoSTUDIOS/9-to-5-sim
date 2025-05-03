using System;
using System.Collections.Generic;
using System.Linq;
using Game.Modifiers;
using Jobs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static JobScriptableObject.JobData CurrentJobData;
    
    public static MainMenuManager Instance;

    public static int weekDayCount = 7;
    public static int currentDay = 0;
    
    public TextMeshProUGUI moneyText, jobXPText, jobText;
    public GameObject modifiersRoot;
    public GameObject jobListRoot;
    public GameObject modifierItemPrefab;
    public GameObject jobItemPrefab;
    public JobScriptableObject[] jobs;
    public Image background;

    private void Awake()
    {
        ModifierManager.LoadCurrentModifiers();
        
        Instance = this;
        
        Time.timeScale = 1f;
        
        PlayerDataManager.jobs = jobs.ToDictionary(x => x.data.id);
        PlayerDataManager.LoadData();
        
        currentDay = 0;

        foreach (var entry in PlayerDataManager.playerData.resumeEntries)
        {
            currentDay += entry.duration;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        var data = PlayerDataManager.playerData;
        moneyText.text = $"Money: ${data.money}";
        jobXPText.text = $"Job XP: {data.jobXP} XP";
        jobText.text = $"Current Job: {PlayerDataManager.jobs[data.currentJob].data.name}";

        var randJobId = jobs.Where(x => !PlayerDataManager.jobs[x.data.id].data.isHidden).OrderBy(x => Guid.NewGuid()).FirstOrDefault()?.data.id;
        
        if (randJobId != null)
        {
            var randJob = PlayerDataManager.jobs[randJobId];
            var jobImg = randJob.data.screenshot;
            background.sprite = jobImg;
        }

        foreach (var modifier in ModifierManager.currentModifiers)
        {
            var modifierItemObj = Instantiate(modifierItemPrefab, modifiersRoot.transform);
            modifierItemObj.GetComponentInChildren<TextMeshProUGUI>().text = modifier.Name;
        }

        currentDay = 0;

        foreach (var entry in data.resumeEntries)
        {
            currentDay += entry.duration;
        }
        
        foreach (var job in jobs)
        {
            if (job.data.isHidden)
            {
                continue;
            }
            
            var jobObj = Instantiate(jobItemPrefab, jobListRoot.transform);
            var jobItem = jobObj.GetComponent<JobItem>();

            jobItem.Job = job;
        }
    }

    public void StartShift()
    {
        var currentJob = PlayerDataManager.jobs[PlayerDataManager.playerData.currentJob];
        CurrentJobData = ModifierManager.ApplyAllModifiers(currentJob.data);
        currentJob.StartShift();
    }

    public void RefreshJob()
    {
        jobText.text = $"Current Job: {PlayerDataManager.jobs[PlayerDataManager.playerData.currentJob].data.name}";
    }
}
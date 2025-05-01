using System;
using System.Collections.Generic;
using System.Linq;
using Jobs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText, jobXPText, jobText;
    public GameObject resumeRoot;
    public GameObject jobListRoot;
    public GameObject resumeEntryPrefab;
    public GameObject jobItemPrefab;
    public JobScriptableObject[] jobs;
    public Image background;

    private void Awake()
    {
        Time.timeScale = 1f;
        
        PlayerDataManager.jobs = jobs.ToDictionary(x => x.id);
        PlayerDataManager.LoadData();
        
        var data = PlayerDataManager.playerData;
        moneyText.text = $"Money: ${data.money}";
        jobXPText.text = $"Job XP: {data.jobXP} XP";
        jobText.text = $"Current Job: {PlayerDataManager.jobs[data.currentJob].name}";

        var randJobId = data.resumeEntries.Where(x => !PlayerDataManager.jobs[x.jobID].isHidden).OrderBy(x => Guid.NewGuid()).FirstOrDefault()?.jobID;
        
        if (randJobId != null)
        {
            var randJob = PlayerDataManager.jobs[randJobId];
            var jobImg = randJob.screenshot;
            background.sprite = jobImg;
        }

        foreach (var entry in data.resumeEntries)
        {
            var entryObj = Instantiate(resumeEntryPrefab, resumeRoot.transform);
            var entryItem = entryObj.GetComponent<JobResumeItem>();

            entryItem.entry = entry;
        }
        
        foreach (var job in jobs)
        {
            if (job.isHidden)
            {
                continue;
            }
            
            var jobObj = Instantiate(jobItemPrefab, jobListRoot.transform);
            var jobItem = jobObj.GetComponent<JobItem>();

            jobItem.job = job;
        }
    }

    public void StartShift()
    {
        PlayerDataManager.jobs[PlayerDataManager.playerData.currentJob].StartShift();
    }
}
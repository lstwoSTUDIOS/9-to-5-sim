using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BasicGameStuff;
using Jobs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseJob : MonoBehaviour
{
    private static readonly int Property = Animator.StringToHash("Pop Up Stats Page");
    public List<BaseJobObjective> objectives = new();
    public float jobTime;
    public JobScriptableObject.JobData job => MainMenuManager.CurrentJobData;
    public float currentWage;

    public TextMeshProUGUI objectivesText;
    public GameObject objectivesPanel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI payPerHourText;

    public GameObject statsPanel;
    public Image statsPanelBG, statsPanelFadeOut;
    public TextMeshProUGUI statsWageText, statsMoneyText, statsXpText;

    public int Hour => (int) (jobTime / 60) + 9;
    public int Minute => (int) (jobTime % 60);

    public bool IsJobRunning => jobTime < job.shiftLength;
    
    protected abstract IEnumerator JobLogic();

    protected virtual void Awake()
    {
        jobTime = 0;
        currentWage = job.baseWage;
    }

    protected virtual void Start()
    {
        StartCoroutine(JobLogic());
    }

    public virtual void Update()
    {
        if (!IsJobRunning && Time.timeScale != 0f)
        {
            StartCoroutine(FinishJob());
            Time.timeScale = 0f;
            return;
        }
        
        if (!IsJobRunning)
        {
            return;
        }
        
        jobTime += Time.deltaTime * 8;
        
        var text = "";
        
        foreach (var objective in objectives)
        {
            GUIObjective(objective, ref text, 0);
        }
        
        objectivesText.text = text;
        
        timeText.text = $"{(Hour < 10 ? "0" : "" )}{Hour}:{(Minute < 10 ? "0" : "" )}{Minute}";
        currentWage = Mathf.Round(currentWage * 100f) / 100f;
        payPerHourText.text = $"Current Wage: ${currentWage}{(currentWage % 0.01f == 0 ? "0" : "")}";
    }

    private static void GUIObjective(BaseJobObjective objective, ref string fullText, int indentation)
    {
        for (var i = 0; i < indentation; i++)
        {
            fullText += "    ";
        }
        
        fullText += $"{objective.DisplayName}\n";

        foreach (var obj in objective.subObjectives)
        {
            GUIObjective(obj, ref fullText, indentation + 1);
        }
    }

    private IEnumerator FinishJob()
    {
        var moneyMade = currentWage * job.shiftLength / 60f;
        var xpMade = job.xpReward;

        PlayerController.MouseCaptured = false;

        statsWageText.text = $"Wage (Pay per Hour): ${currentWage}";
        statsMoneyText.text = $"Money Earned: ${moneyMade}";
        statsXpText.text = $"Xp Reward: {xpMade} XP";

        var data = PlayerDataManager.playerData;

        data.money += moneyMade;
        data.jobXP += xpMade;

        var lastResumeEntry = data.resumeEntries.LastOrDefault();

        if (lastResumeEntry != null && lastResumeEntry?.jobID == job.id)
        {
            lastResumeEntry.duration += 1;
            lastResumeEntry.earnedMoney += moneyMade;
            lastResumeEntry.earnedXP += xpMade;
        }
        else
        {
            var resumeEntry = new ResumeEntry
            {
                jobID = job.id,
                duration = 1,
                earnedMoney = moneyMade,
                earnedXP = xpMade
            };

            data.resumeEntries.Add(resumeEntry);
        }

        PlayerDataManager.SaveData();

        var timer = 0f;

        while (timer < 2f)
        {
            timer += Time.unscaledDeltaTime;
            var x = timer / 2f;
            var t = x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
            var y = Mathf.Lerp(-1080f, 0f, t);
            var rect = (RectTransform)statsPanel.transform;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);
            statsPanelBG.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.8f, t));
            yield return null;
        }

        timer = 0f;

        while (timer < 10f)
        {
            timer += Time.unscaledDeltaTime;
            var x = timer / 10f;
            var t = x == 0f ? 0f : Mathf.Pow(2f, 10f * x - 10f);
            statsPanelFadeOut.color = new Color(0, 0, 0, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }

        SceneManager.LoadSceneAsync("MainMenu");
    }
}

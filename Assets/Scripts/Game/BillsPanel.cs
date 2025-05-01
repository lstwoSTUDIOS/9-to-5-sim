using System;
using System.Collections;
using System.Linq;
using Game.Modifiers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BillsPanel : MonoBehaviour
{
    public float rent = 250;
    public float tax = 0.1f;
    public Vector2 expensesRange = new(50, 250);
    public float insurance = 125;

    public TextMeshProUGUI rentText, taxText, expensesText, insuranceText, totalText, modifierText;
    public GameObject bankruptcyPanel;

    private float total;
    
    private void OnEnable()
    {
        if (MainMenuManager.currentDay % MainMenuManager.weekDayCount != 0 || MainMenuManager.currentDay == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        var expenses = Random.Range(expensesRange.x, expensesRange.y);
        total = rent + tax + expenses + insurance;
        
        rentText.text = $"Rent: ${rent:N2}";
        taxText.text = $"Income Tax: ${tax:N2}";
        expensesText.text = $"Expenses: ${expenses:N2}";
        insuranceText.text = $"Insurance: ${insurance:N2}";
        totalText.text = $"Total: ${total:N2}";

        var modifier = ModifierManager.idToModifierMap.Values.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        
        if (modifier != null)
        {
            PlayerDataManager.playerData.modifiers.Add(modifier.Id);
            modifierText.text = modifier.Name;
            ModifierManager.LoadCurrentModifiers();
        }
    }

    public void Pay()
    {
        var money = PlayerDataManager.playerData.money;

        if (money >= total)
        {
            PlayerDataManager.playerData.money -= total;
            gameObject.SetActive(false);
        }
        else
        {
            bankruptcyPanel.SetActive(true);
            PlayerDataManager.playerData = new();
            PlayerDataManager.SaveData();
            StartCoroutine(Close());
        }
    }

    private IEnumerator Close()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}

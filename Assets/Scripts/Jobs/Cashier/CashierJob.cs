using System;
using System.Collections;
using System.Linq;
using Jobs.Cashier;
using UnityEngine;
using Random = UnityEngine.Random;

public class CashierJob : BaseJob
{
    public static CashierJob Instance;
    
    public Vector2 timeBetweenCustomers;
    public Transform itemSpawnTransform;
    public GameObject[] itemPrefabs;
    public GameObject customerPrefab;
    public Transform queueTransform;
    public Transform deskTransform;
    public Material[] customerMaterials;

    [NonSerialized] 
    public int customerCount;
    
    [NonSerialized]
    public CashierJobCustomerObjective currentCustomerObjective;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(DecreasePay());
    }

    protected override IEnumerator JobLogic()
    {
        while (IsJobRunning)
        {
            var objectiveObject = new GameObject("Customer Objective")
            {
                transform =
                {
                    parent = transform
                }
            };

            var objective = objectiveObject.AddComponent<CashierJobCustomerObjective>();
            objective.onComplete += () =>
            {
                currentCustomerObjective = (CashierJobCustomerObjective) objectives.FirstOrDefault(obj => !obj.IsFinished);
                objectives.Remove(objective);
                customerCount--;
            };
            objectives.Add(objective);
            customerCount++;
            
            currentCustomerObjective ??= objective;
            
            yield return new WaitForSeconds(Random.Range(timeBetweenCustomers.x, timeBetweenCustomers.y));
        }
    }

    private IEnumerator DecreasePay()
    {
        while (IsJobRunning)
        {
            yield return new WaitForSeconds(0.325f);
            currentWage -= 0.01f;
        }
    }
}
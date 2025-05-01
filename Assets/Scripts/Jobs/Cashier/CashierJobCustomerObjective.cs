using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs.Cashier
{
    public class CashierJobCustomerObjective : BaseJobObjective
    {
        public override string DisplayName => "Serve Customer";
        public override bool IsFinished => isFinished;

        private bool isFinished;
        private bool isAnimationFinished;
        private Customer customer;

        private IEnumerator Start()
        {
            yield return StartCoroutine(CreateCustomer());
            CreateSubObjective();
        }

        private IEnumerator CreateCustomer()
        {
            var customerObj = Instantiate(CashierJob.Instance.customerPrefab);
            customer = customerObj.GetComponent<Customer>();
            customer.ParentObjective = this;
            yield return StartCoroutine(customer.CustomerWalkIn());
        }

        private void CreateSubObjective()
        {
            var scanItemsObjectiveObj = new GameObject("Scan Customer's Items Objective")
            {
                transform =
                {
                    parent = transform
                }
            };

            var scanItemsObjective = scanItemsObjectiveObj.AddComponent<CashierJobScanItemsObjective>();
            scanItemsObjective.parentObjective = this;
            scanItemsObjective.onComplete += () => subObjectives.Remove(scanItemsObjective);
            subObjectives.Add(scanItemsObjective);
            
            var takeMoneyObjectiveObj = new GameObject("Take Customer's Money Objective")
            {
                transform =
                {
                    parent = transform
                }
            };

            var takeMoneyObjective = takeMoneyObjectiveObj.AddComponent<CashierJobTakeMoneyObjective>();
            takeMoneyObjective.onComplete += FinishObjective;
            takeMoneyObjective.onComplete += () => subObjectives.Remove(takeMoneyObjective);
            subObjectives.Add(takeMoneyObjective);
            
            scanItemsObjective.onComplete += () =>
            {
                StartCoroutine(customer.Pay());
                customer.doller.OnScan += () => takeMoneyObjective.FinishObjective();
            };
        }

        public override void FinishObjective()
        {
            StartCoroutine(Finish());
        }

        private IEnumerator Finish()
        {
            yield return StartCoroutine(customer.Dip());
            isFinished = true;
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
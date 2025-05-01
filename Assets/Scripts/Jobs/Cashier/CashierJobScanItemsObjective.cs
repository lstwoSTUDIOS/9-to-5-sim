using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs.Cashier
{
    public class CashierJobScanItemsObjective : BaseJobObjective
    {
        public override string DisplayName => "Scan Customer's Items";
        public override bool IsFinished => isFinished;

        [NonSerialized]
        public CashierJobCustomerObjective parentObjective;

        private bool isFinished;
        private int itemCount = int.MaxValue;
        private int spawnedItemCount = 0;

        private IEnumerator Start()
        {
            itemCount = Random.Range(1, 15);
            
            for (var i = 0; i < itemCount; i++)
            {
                var objectiveObject = new GameObject("Scan Customer Items Objective")
                {
                    transform =
                    {
                        parent = transform
                    }
                };

                var objective = objectiveObject.AddComponent<CashierJobScanItemObjective>();
                objective.onComplete += Refresh;
                objective.onComplete += () => subObjectives.Remove(objective);

                subObjectives.Add(objective);

                spawnedItemCount++;
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void Refresh()
        {
            if (spawnedItemCount != itemCount || !subObjectives.All(obj => obj.IsFinished))
            {
                return;
            }
            
            FinishObjective();
        }

        public override void FinishObjective()
        {
            isFinished = true;
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
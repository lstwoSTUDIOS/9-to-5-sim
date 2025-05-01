
using UnityEngine;

namespace Jobs.Cashier
{
    public class CashierJobScanItemObjective : BaseJobObjective
    {
        public override string DisplayName => "Scan Customer's Item";
        public override bool IsFinished => isFinished;

        private bool isFinished;

        private void Awake()
        {
            var pos = CashierJob.Instance.itemSpawnTransform.position;
            var prefab = CashierJob.Instance.itemPrefabs[Random.Range(0, CashierJob.Instance.itemPrefabs.Length)];
            var obj = Instantiate(prefab, pos, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), transform);
            
            obj.GetComponent<ScanableItem>().OnScan += FinishObjective;
        }

        public override void FinishObjective()
        {
            CashierJob.Instance.currentWage += 0.1f;
            isFinished = true;
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
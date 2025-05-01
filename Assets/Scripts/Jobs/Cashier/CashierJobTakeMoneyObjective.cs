namespace Jobs.Cashier
{
    public class CashierJobTakeMoneyObjective : BaseJobObjective
    {
        public override string DisplayName => "Take Customer's Money";
        public override bool IsFinished => isFinished;

        private bool isFinished;
        
        public override void FinishObjective()
        {
            CashierJob.Instance.currentWage += 0.05f;
            isFinished = true;
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}
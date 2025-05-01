using Jobs;

namespace Game.Modifiers
{
    public class SalaryIncrease05xModifier : BaseModifier
    {
        public override string Id => "SalaryDecreaseModifier05";
        public override string Name => "Salary Decrease (x0.5)";

        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.baseWage *= 0.5f;
        }
    }
}
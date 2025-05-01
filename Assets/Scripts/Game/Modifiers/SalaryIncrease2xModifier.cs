using Jobs;

namespace Game.Modifiers
{
    public class SalaryIncrease2xModifier : BaseModifier
    {
        public override string Id => "SalaryIncreaseModifier";
        public override string Name => "Salary Increase (x2)";

        public override JobScriptableObject.JobData SetupModifier(JobScriptableObject.JobData job)
        {
            job.baseWage *= 2;
            return job;
        }
    }
}
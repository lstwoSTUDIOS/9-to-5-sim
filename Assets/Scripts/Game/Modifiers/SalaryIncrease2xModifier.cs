using Jobs;

namespace Game.Modifiers
{
    public class SalaryIncrease2xModifier : BaseModifier
    {
        public override string Id => "SalaryIncreaseModifier2";
        public override string Name => "Salary Increase (x2)";

        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.baseWage *= 2;
        }
    }
}
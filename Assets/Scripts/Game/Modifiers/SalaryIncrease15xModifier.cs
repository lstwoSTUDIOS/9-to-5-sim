using Jobs;

namespace Game.Modifiers
{
    public class SalaryIncrease15xModifier : BaseModifier
    {
        public override string Id => "SalaryIncreaseModifier15";
        public override string Name => "Salary Increase (x1.5)";

        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.baseWage *= 1.5f;
        }
    }
}
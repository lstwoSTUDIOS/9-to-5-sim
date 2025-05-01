using Jobs;

namespace Game.Modifiers
{
    public class OvertimeModifier : BaseModifier
    {
        public override string Id => "OvertimeModifier";
        public override string Name => "Overtime (+1 Hour per Shift)";
        
        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.shiftLength += 60;
        }
    }
}
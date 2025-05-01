using Jobs;

namespace Game.Modifiers
{
    public class TimeOffModifier : BaseModifier
    {
        public override string Id => "TimeOff";
        public override string Name => "1 Hour Off (-1 Hour per Shift)";
        
        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.shiftLength -= 60;
        }
    }
}
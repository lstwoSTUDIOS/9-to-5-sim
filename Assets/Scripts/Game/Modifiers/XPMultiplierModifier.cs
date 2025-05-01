using Jobs;

namespace Game.Modifiers
{
    public class XPMultiplierModifier : BaseModifier
    {
        public override string Id => "XPMultiplierModifier";
        public override string Name => "XP Multiplier (2x XP per Shift)";
        
        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.xpReward *= 2;
        }
    }
}
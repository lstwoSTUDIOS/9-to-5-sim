using Jobs;
using UnityEngine;

namespace Game.Modifiers
{
    public class XPMultiplier05Modifier : BaseModifier
    {
        public override string Id => "XPMultiplier05Modifier";
        public override string Name => "XP Multiplier (0.5x XP per Shift)";
        
        public override void SetupModifier(ref JobScriptableObject.JobData job)
        {
            job.xpReward = Mathf.RoundToInt(job.xpReward * 0.5f);
        }
    }
}
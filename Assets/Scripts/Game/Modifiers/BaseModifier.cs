using Jobs;

namespace Game.Modifiers
{
    public abstract class BaseModifier
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        
        public abstract JobScriptableObject.JobData SetupModifier(JobScriptableObject.JobData job);
    }
}
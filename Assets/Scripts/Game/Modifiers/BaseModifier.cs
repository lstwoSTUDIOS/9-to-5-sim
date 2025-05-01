using Jobs;

namespace Game.Modifiers
{
    public abstract class BaseModifier
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        
        public abstract void SetupModifier(ref JobScriptableObject.JobData job);
    }
}
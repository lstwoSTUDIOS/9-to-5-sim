using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseJobObjective : MonoBehaviour
{
    public abstract string DisplayName { get; }
    
    public List<BaseJobObjective> subObjectives = new();
    public Action onComplete;
    
    public virtual bool IsFinished => subObjectives.All(obj => obj.IsFinished);
    
    public abstract void FinishObjective();
}

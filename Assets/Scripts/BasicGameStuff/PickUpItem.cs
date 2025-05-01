using UnityEngine;
using UnityEngine.Events;

namespace BasicGameStuff
{
    public class PickUpItem : MonoBehaviour
    {
        public bool lookAtPlayer = false;
        public float grabSpeedMultiplier = 1.0f;

        [Space(5)]
        public UnityEvent onPickupStart;
        [Space(5)]
        public UnityEvent onPickupEnd;

        public virtual void OnPickupStart()
        {
            onPickupStart.Invoke();
        }

        public virtual void OnPickupEnd()
        {
            onPickupEnd.Invoke();
        }
    }
}

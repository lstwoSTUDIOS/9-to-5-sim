using System;
using UnityEngine;

namespace Jobs.Cashier
{
    public class ScanableItem : MonoBehaviour
    {
        public Action OnScan;

        public void Scan()
        {
            OnScan?.Invoke();
            Destroy(gameObject);
        }
    }
}

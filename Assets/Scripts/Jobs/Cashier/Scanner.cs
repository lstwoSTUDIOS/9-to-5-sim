using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs.Cashier
{
    public class Scanner : MonoBehaviour
    {
        public AudioSource audioSource;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<ScanableItem>(out var item))
            {
                return;
            }
            
            item.Scan();
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
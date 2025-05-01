using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jobs.Cashier
{
    public class ConveyorBelt : MonoBehaviour
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
        private static readonly int MetallicGlossMap = Shader.PropertyToID("_MetallicGlossMap");
        
        public float speed;
        public float materialOffsetMul;
        public Vector2 direction;
        
        private Material material;
        private List<GameObject> objsInTrigger = new();
        private List<GameObject> objsTouching = new();
        private Vector3 Direction => new(direction.x, 0, direction.y);
        
        private bool ShouldMove => objsInTrigger.Count == 0;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            if (!ShouldMove)
            {
                return;
            }
            
            material.SetTextureOffset(BaseMap, material.GetTextureOffset(BaseMap) + direction * (speed * Time.deltaTime * materialOffsetMul));
            material.SetTextureOffset(MetallicGlossMap, direction * (speed * Time.deltaTime * materialOffsetMul));

            foreach (var obj in objsTouching)
            {
                obj.transform.position += Direction * (speed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody != null && !objsTouching.Contains(other.gameObject))
            {
                objsTouching.Add(other.gameObject);
                RefreshLists();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (objsTouching.Contains(other.gameObject))
            {
                objsTouching.Remove(other.gameObject);
                RefreshLists();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody != null && !objsInTrigger.Contains(other.gameObject))
            {
                objsInTrigger.Add(other.gameObject);
                RefreshLists();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (objsInTrigger.Contains(other.gameObject))
            {
                objsInTrigger.Remove(other.gameObject);
                RefreshLists();
            }
        }

        private void RefreshLists()
        {
            var objsInTrigger = this.objsInTrigger.ToList();

            foreach (var obj in objsInTrigger.Where(obj => obj == null))
            {
                this.objsInTrigger.Remove(obj);
            }
            
            var objsTouching = this.objsTouching.ToList();

            foreach (var obj in objsTouching.Where(obj => obj == null))
            {
                this.objsTouching.Remove(obj);
            }
        }
    }
}
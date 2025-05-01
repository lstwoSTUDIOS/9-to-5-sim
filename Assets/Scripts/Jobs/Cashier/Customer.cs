using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jobs.Cashier
{
    public class Customer : MonoBehaviour
    {
        public ScanableItem doller;
        
        [NonSerialized]
        public CustomerType CustomerType;
        
        [NonSerialized]
        public CashierJobCustomerObjective ParentObjective;
        
        private Animation animationPlayer;

        private void Awake()
        {
            animationPlayer = GetComponent<Animation>();
            doller.GetComponent<Rigidbody>().isKinematic = true;
            doller.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        private IEnumerator MoveToPos(GameObject customerObj, Vector3 destPos, Quaternion destRot)
        {
            var startPos = customerObj.transform.position;
            var startRot = customerObj.transform.rotation;

            var animCounter = 0f;

            while (animCounter < 3f)
            {
                animCounter += Time.deltaTime;
                
                var x = animCounter / 3f;
                var t = x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
                
                customerObj.transform.position = Vector3.Lerp(startPos, destPos, t);
                customerObj.transform.rotation = Quaternion.Lerp(startRot, destRot, t);
                
                yield return null;
            }
        }

        public IEnumerator CustomerWalkIn()
        {
            animationPlayer.Play("Customer Walk In");
            transform.Find("Capsule").GetComponent<Renderer>().material = CashierJob.Instance.customerMaterials[Random.Range(0, CashierJob.Instance.customerMaterials.Length)];
            transform.Find("Nametag").GetComponent<TextMeshPro>().text = XBoxNameGenerator.GenerateUsername();
            
            yield return new WaitForSeconds(animationPlayer.GetClip("Customer Walk In").length);

            var isInQueue = false;
            
            if (CashierJob.Instance.currentCustomerObjective == ParentObjective)
            {
                animationPlayer.Play("Customer Walk To Checkout");
                yield return new WaitForSeconds(animationPlayer.GetClip("Customer Walk To Checkout").length);
            }
            else
            {
                isInQueue = true;
                var queueTransform = CashierJob.Instance.queueTransform;
                yield return StartCoroutine(MoveToPos(gameObject, queueTransform.position + -queueTransform.forward * ((CashierJob.Instance.customerCount - 2) * 1.5f), 
                    Quaternion.Euler(queueTransform.transform.rotation.eulerAngles.x, 180, queueTransform.transform.rotation.eulerAngles.z)));
            }
            
            yield return new WaitUntil(() => CashierJob.Instance.currentCustomerObjective == ParentObjective);

            if (!isInQueue) yield break;
            
            var deskTransform = CashierJob.Instance.deskTransform;
            yield return StartCoroutine(MoveToPos(gameObject, deskTransform.position,
                Quaternion.Euler(deskTransform.transform.rotation.eulerAngles.x, -deskTransform.transform.rotation.eulerAngles.y, deskTransform.transform.rotation.eulerAngles.z)));
        }

        public IEnumerator Dip()
        {
            animationPlayer.Play("Customer Dip");
            yield return new WaitForSeconds(animationPlayer.GetClip("Customer Dip").length);
            Destroy(gameObject);
        }

        public IEnumerator Pay()
        {
            animationPlayer.Play("Customer Give Money");
            yield return new WaitForSeconds(animationPlayer.GetClip("Customer Give Money").length);
            doller.GetComponent<Rigidbody>().isKinematic = false;
            doller.gameObject.layer = LayerMask.NameToLayer("PickUp");
        }
    }
}
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Jobs
{
    [CreateAssetMenu(fileName = "JobScriptableObject", menuName = "lstwo/JobScriptableObject")]
    public class JobScriptableObject : ScriptableObject
    {
        public bool isHidden = false;
        [FormerlySerializedAs("displayName")] public new string name;
        public string id;
        public SceneReference scene;
        public int xpReward;
        [FormerlySerializedAs("wage")] public float baseWage;
        [FormerlySerializedAs("jobDuration")] public float shiftLength;
        public Sprite screenshot;
        public int xpRequired;
        
        public void StartShift()
        {
            SceneManager.LoadScene(scene);
        }
    }
}

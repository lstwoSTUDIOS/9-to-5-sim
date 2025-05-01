using System;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Jobs
{
    [CreateAssetMenu(fileName = "JobScriptableObject", menuName = "lstwo/JobScriptableObject")]
    public class JobScriptableObject : ScriptableObject
    {
        public JobData data;
        
        public void StartShift()
        {
            SceneManager.LoadScene(data.scene);
        }

        [Serializable]
        public struct JobData
        {
            public bool isHidden;
            [FormerlySerializedAs("displayName")] public string name;
            public string id;
            public SceneReference scene;
            public int xpReward;
            [FormerlySerializedAs("wage")] public float baseWage;
            [FormerlySerializedAs("jobDuration")] public float shiftLength;
            public Sprite screenshot;
            public int xpRequired;
        }
    }
}

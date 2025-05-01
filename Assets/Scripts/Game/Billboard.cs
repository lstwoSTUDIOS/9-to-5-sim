using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.up, 180f);
    }
}

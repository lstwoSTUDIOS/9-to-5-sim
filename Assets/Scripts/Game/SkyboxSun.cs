using System;
using AdvancedProceduralSkybox;
using UnityEngine;

[ExecuteInEditMode]
public class SkyboxSun : MonoBehaviour
{
    public Material skybox;
    
    public void Update()
    {
        if (skybox is null)
            return;
        
        SkyboxProperty.SetSunDirection(skybox, -transform.forward);

        var flippedRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
        var flippedMatrix = Matrix4x4.Rotate(Quaternion.Inverse(flippedRotation));
        SkyboxProperty.SetMoonMatrix(skybox, flippedMatrix);
    }
}
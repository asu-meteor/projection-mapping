using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ProjMatPropertiesSetter : MonoBehaviour
{
    public Camera personCam;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        // View * Projection matrix for the personCam.
        Matrix4x4 personCamVP = personCam.projectionMatrix * personCam.worldToCameraMatrix;

        // World transformation matrix for the object.
        Matrix4x4 myToWorld = transform.localToWorldMatrix;

        // Set these matrices to the material of the Renderer.
        rend.material.SetMatrix("_PersonCamVP", personCamVP);
        rend.material.SetMatrix("_MyToWorld", myToWorld);
    }
}

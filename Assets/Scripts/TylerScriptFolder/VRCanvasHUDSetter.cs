
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Add this to the HUD Canvas. Upon start of the scene, the canvas will become the child of the VR camera.
/// </summary>
public class VRCanvasHUDSetter : MonoBehaviour
{
    private void Awake()
    {
        gameObject.transform.SetParent(Camera.main.transform);
    }
}
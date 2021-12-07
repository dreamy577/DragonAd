using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSet : MonoBehaviour
{
    public Camera boss01AreaCam;
    public Material boss01CamMat;

    public Camera boss02AreaCam;
    public Material boss02CamMat;

    void Awake()
    {
        if (boss01AreaCam.targetTexture != null)
            boss01AreaCam.targetTexture.Release();

        boss01AreaCam.targetTexture = new RenderTexture(Screen.width, Screen.height,24);
        boss01CamMat.mainTexture = boss01AreaCam.targetTexture;
    }
}

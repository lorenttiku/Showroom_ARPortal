using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    public Material[] materials;

    public Transform device;

    bool wasInFront;
    bool inOtherWorld;

    void Start()
    {
        SetMaterials(false);
    }

    void SetMaterials(bool fullRender)
    {

        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal; 

        foreach (var mat in materials)
        {
            mat.SetInt("_StencilTest", (int)CompareFunction.NotEqual);
        }
    }


    void OnTriggerStay(Collider other)
    {


    }


    void OnDestroy()
    {
        SetMaterials(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}

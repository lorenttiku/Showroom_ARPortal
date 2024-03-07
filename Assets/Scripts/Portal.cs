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

    bool GetIsInFront()
    {
        Vector3 pos = transform.InverseTransformPoint(device.position);
        return pos.z >= 0 ? true : false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != device)
            return;
        wasInFront = GetIsInFront();
    }





    void OnTriggerStay(Collider other)
    {

        if (other.transform != device)

            return;
        bool isInFront = GetIsInFront();
        if((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);
        }

        wasInFront = isInFront;
        
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

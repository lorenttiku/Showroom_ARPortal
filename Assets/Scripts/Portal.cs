using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class Portal : MonoBehaviour
{
    public static event Action<bool> OnGetInPortal;

    public Material[] materials;

    public Transform device;

    bool wasInFront;
    public bool inOtherWorld;
  
    void Start()
    {

        SetMaterials(false);

    }

    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;


        foreach (var mat in materials)
        {
            mat.SetInt("_StencilTest", (int)stencilTest);
        }
    }

   
    bool GetIsInFront()
    {
        Vector3 pos = transform.InverseTransformPoint(device.position);
        return pos.z >= 0 ? true: false;
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
        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            OnGetInPortal?.Invoke(!inOtherWorld);
            inOtherWorld = !inOtherWorld;
            SetMaterials(inOtherWorld);
        }

        wasInFront = isInFront;
        
    }


    private void OnDestroy()
    {
        SetMaterials(true);
    }

    void Update() 
    {
        
    }
}

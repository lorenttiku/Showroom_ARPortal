using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InterdimensionalTransport : MonoBehaviour
{
    public Material[] materials;

    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name != "AR Camera")
            return;


        if (transform.position.z > other.transform.position.z)
        {
            Debug.Log("Outside of the other world");
            foreach (var mat in materials)
            {
                mat.SetInt("_StencilTest", (int)CompareFunction.Equal);
            }
        }

        else
        {
            Debug.Log("Inside of the other world");
            foreach (var mat in materials)
            {
                mat.SetInt("_StencilTest", (int)CompareFunction.NotEqual);
            }
        }
        
    }



    private void OnDestroy()
    {
        foreach (var mat in materials)
        {
            mat.SetInt("_StencilTest", (int)CompareFunction.NotEqual);
        }
    }
    void Update()
    {
        
    }
}

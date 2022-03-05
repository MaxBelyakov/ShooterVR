using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryPosition : MonoBehaviour
{
    public Transform headCamera;
    public float x_correction;
    public float y_correction;
    public float z_correction;

    private float x;
    private float y;
    private float z;

    public XRGrabInteractable controller;
    
    void Start()
    {
        controller.activated.AddListener(Test);
        
    }

    private void Test(ActivateEventArgs arg0)
    {
        print ("head y: " + headCamera.position.y);
        print ("belt y: " + transform.position.y);
    }

    void Update()
    {
        //if (Mathf.Abs(headCamera.position.y - transform.position.y) > y_correction + 0.5f)
        //    transform.position = new Vector3(transform.position.x, headCamera.position.y - 0.5f, transform.position.z);
        x = headCamera.position.x + 0.25f;
        y = headCamera.position.y - 0.5f;
        z = headCamera.position.z;
        transform.position = new Vector3(x, y, z);

    }
}

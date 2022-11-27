using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.XR.OpenXR.Features.Interactions.HTCViveControllerProfile;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;    
    private InputDevice targetDevice;
    public Animator handAnimator;

    //[SerializeField] private GameObject controllerWithHints;
    //[SerializeField] private GameObject interactionRay;

    private bool showController = true;

    private void OnEnable()
    {
        //if (ExhibitionBehaviour.Instance != null)
        //    ExhibitionBehaviour.Instance.UI.SettingsWindow.OnShowControllerHints += ShowControllerHints;
    }

    private void OnDisable()
    {
        //if (ExhibitionBehaviour.Instance != null)
        //    ExhibitionBehaviour.Instance.UI.SettingsWindow.OnShowControllerHints -= ShowControllerHints;
    }

    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void UpdateHandAnimation()
    {
        float summ = 0;

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);

            summ += triggerValue;
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);

            summ += gripValue;
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

        // Hide controller when any button pressed
       /* if (summ > 0.01f)
            controllerWithHints?.SetActive(false);
        else if (showController)
            controllerWithHints?.SetActive(true);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            UpdateHandAnimation();
        }
    }

    private void ShowControllerHints(bool value)
    {
        showController = value;
        //controllerWithHints.SetActive(value);
    }
}

// Script make active teleportation ray when primary button is pressed
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    [SerializeField] private XRController LeftHandRayController;                  // Left hand ray
    [SerializeField] private InputHelpers.Button teleportActivationButton;        // Activation button

    private float activationThreshold = 0.1f;                                     // Button threshold

    void Update()
    {
        // Activate/Deactivate teleportation ray
        if (LeftHandRayController)
            LeftHandRayController.gameObject.SetActive(CheckIfActivated(LeftHandRayController));
    }

    private bool CheckIfActivated(XRController controller)
    {
        // Listening for activation button press with activation threshold
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}

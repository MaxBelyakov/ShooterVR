// Script make active teleportation ray when primary button is pressed
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    [SerializeField] private XRController LeftHandRayController;                  // Left hand ray
    [SerializeField] private InputHelpers.Button teleportActivationButton;        // Activation button
    [SerializeField] private InputActionProperty rightMoveLocomotionControls;

    [SerializeField] private Animator _avatarAnimator;

    private float activationThreshold = 0.1f;                                     // Button threshold

    private void Start()
    {
        rightMoveLocomotionControls.action.performed += UpdateMoveAnimation;
        rightMoveLocomotionControls.action.canceled += FinishMoveAnimation;
    }

    private void OnDestroy()
    {
        rightMoveLocomotionControls.action.performed -= UpdateMoveAnimation;
        rightMoveLocomotionControls.action.canceled -= FinishMoveAnimation;
    }

    private void FinishMoveAnimation(InputAction.CallbackContext obj)
    {
        _avatarAnimator.SetFloat("Move", 0);
    }

    private void UpdateMoveAnimation(InputAction.CallbackContext obj)
    {
        _avatarAnimator.SetFloat("Move", Mathf.Abs(obj.ReadValue<Vector2>().magnitude));
    }

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

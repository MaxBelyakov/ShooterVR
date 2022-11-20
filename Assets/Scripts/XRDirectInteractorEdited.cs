// Edited version of XRDirectInteractor.
// Manage the select action trigger, when interact with weapon hand stick it, else simple state it
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectInteractorEdited : XRDirectInteractor
{
    [SerializeField] private SphereCollider handCollider;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Deactivate hand collider to avoid conflict with weapon colliders
        handCollider.enabled = false;

        // Stick in hands only weapons
        if (args.interactableObject.transform.GetComponentInChildren<IWeapon>() != null)
            selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Sticky;
        else
            selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.State;

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Return hand collider
        handCollider.enabled = true;

        // Return to default action trigger
        selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.State;

        base.OnSelectExited(args);
    }
}

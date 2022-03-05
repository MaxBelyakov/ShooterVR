// Edited version of XRDirectInteractor.
// Manage the select action trigger, when interact with weapon hand stick it, else simple state it

using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectInteractorEdited : XRDirectInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Only for weapons
        if ((args.interactableObject.transform.tag == "pistol item") ||
        (args.interactableObject.transform.tag == "machine gun item") ||
        (args.interactableObject.transform.tag == "shotgun item") ||
        (args.interactableObject.transform.tag == "bow item"))
            selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.Sticky;
        else
            selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.State;

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Return to default action trigger
        selectActionTrigger = XRBaseControllerInteractor.InputTriggerType.State;

        base.OnSelectExited(args);
    }
}

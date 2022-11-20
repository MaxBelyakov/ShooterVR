using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableArrow : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Get arrow in hand. Activate box collider and reset first collision flag
        args.interactableObject.transform.GetComponent<BoxCollider>().isTrigger = false;
        args.interactableObject.transform.GetComponent<Arrow>().FirstCollision = true;
        
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Put the arrow. Unparent and switch off kinematic. Arrow is ready for shooting or as ammo
        args.interactableObject.transform.parent = null;
        args.interactableObject.transform.GetComponent<Rigidbody>().isKinematic = false;

        base.OnSelectExited(args);
    }
}

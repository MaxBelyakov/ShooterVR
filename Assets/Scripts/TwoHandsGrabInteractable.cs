// Custom XRGrabInteractable
// Control two hands weapon grabbing

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandsGrabInteractable : XRGrabInteractable
{
    public XRSimpleInteractable secondHandGrabPoint;            // Second hand grab point
    private IXRSelectInteractor firstInteractor;                // First grabbed hand object
    private IXRSelectInteractor secondInteractor;               // Second grabbed hand object
    private IXRSelectInteractable secondInteractable;           // Second grabbed point object
    private Quaternion attachInitialRotation;                   // Return the object to start rotation when dropped

    void Start()
    {
        // Listening for grab second hand point
        secondHandGrabPoint.selectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoint.selectExited.AddListener(OnSecondHandRelease);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Compute the rotation of the first hand if weapon in two hands
        // Upward rotation is available just by second hand (transform.up parametr of LookRotation)
        if (secondInteractor != null && firstInteractor != null)
            firstInteractor.transform.rotation = Quaternion.LookRotation(secondInteractor.transform.position - firstInteractor.transform.position, secondInteractor.transform.up);
        
        base.ProcessInteractable(updatePhase);
    }

    // Listener. Save second hand interactor/interactable objects
    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        secondInteractor = args.interactorObject;
        secondInteractable = args.interactableObject;
    }

    // Listener. Remove second hand interactor/interactable objects
    public void OnSecondHandRelease(SelectExitEventArgs interactor)
    {
        secondInteractor = null;
        secondInteractable = null;
    }

    // Listener. Save first hand interactor object and its start rotation (hand rotation) 
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        firstInteractor = args.interactorObject;
        attachInitialRotation = args.interactorObject.transform.localRotation;
    }

    // Listener. Remove first hand and second hand objects
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        firstInteractor = null;

        if (secondInteractor != null)
        {
            // fix: Call SelectExit to second interaction to finish interaction of the second hand
            interactionManager.SelectExit(secondInteractor, secondInteractable);

            secondInteractor = null;
            secondInteractable = null;
        }
        
        // Return hand rotation to start position
        args.interactorObject.transform.localRotation = attachInitialRotation;
    }
}
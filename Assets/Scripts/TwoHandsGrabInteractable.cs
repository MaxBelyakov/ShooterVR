using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandsGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private IXRSelectInteractor firstInteractor;
    private IXRSelectInteractor secondInteractor;
    private Quaternion attachInitialRotation;

    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && firstInteractor != null)
        {
            // Compute the rotation
            firstInteractor.transform.rotation = Quaternion.LookRotation(secondInteractor.transform.position - firstInteractor.transform.position, secondInteractor.transform.up);
        }
        base.ProcessInteractable(updatePhase);
    }

    public void OnSecondHandGrab(SelectEnterEventArgs interactor)
    {
        //print("second hand GRAB");
        secondInteractor = interactor.interactorObject;
    }

    public void OnSecondHandRelease(SelectExitEventArgs interactor)
    {
        //print("second hand RELEASE");
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        //print("first grab entered");
        firstInteractor = interactor.interactorObject;
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.interactorObject.transform.localRotation;
    }

    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        //print("first grab exited");
        base.OnSelectExited(interactor);
        firstInteractor = null;
        secondInteractor = null;
        interactor.interactorObject.transform.localRotation = attachInitialRotation;
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = isSelected && !interactor.Equals(interactorsSelecting[0]);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
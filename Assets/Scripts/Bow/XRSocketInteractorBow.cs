using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorBow : XRSocketInteractor
{
    private string arrowTag = "arrow";

    private Bow _bow;

    protected override void Start()
    {
        _bow = transform.parent.GetComponent<Bow>();

        base.Start();
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(arrowTag);
    }

    // Hover socket just for selected weapon ammo
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(arrowTag);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        _bow.IsCharged = true;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        _bow.IsCharged = false;
        base.OnSelectExited(args);
    }
}
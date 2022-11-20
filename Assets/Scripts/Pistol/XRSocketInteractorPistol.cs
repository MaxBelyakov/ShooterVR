using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorPistol : XRSocketInteractor
{
    public PistolMagazine Magazine { get; set; }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && (interactable.transform.GetComponent<PistolMagazine>() != null);
    }

    // Hover socket just for selected weapon ammo
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && (interactable.transform.GetComponent<PistolMagazine>() != null);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Magazine = args.interactableObject.transform.GetComponent<PistolMagazine>();
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Magazine = null;
        base.OnSelectExited(args);
    }
}
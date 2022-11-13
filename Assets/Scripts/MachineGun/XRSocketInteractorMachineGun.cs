// Machine Gun socket interactor controller

using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorMachineGun : XRSocketInteractor
{
    public MachineGunMagazine Magazine { get; set; }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && (interactable.transform.GetComponent<MachineGunMagazine>() != null);
    }

    // Hover socket just for selected weapon ammo
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && (interactable.transform.GetComponent<MachineGunMagazine>() != null);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Magazine = args.interactableObject.transform.GetComponent<MachineGunMagazine>();
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Magazine = null;
        base.OnSelectExited(args);
    }
}
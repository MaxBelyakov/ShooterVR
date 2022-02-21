// Custom XRGrabInteractable
// Layer 6 - Player
// Layer 11 - Weapon Magazine
// Layer 0 - Default

using UnityEngine.XR.Interaction.Toolkit;

public class MagazineGrabInteractable : XRGrabInteractable
{
    // For each type of ammo check the weapon in the hands
    private bool weaponInHands()
    {
        if (gameObject.tag == "pistol magazine ammo")
            if (WeaponController.s_weapon == "Pistol")
                return true;
        if (gameObject.tag == "machine gun ammo")
            if (WeaponController.s_weapon == "Machine Gun")
                return true;
        return false;
    }

    // Ignore grabbing weapon magazine when pistol not in the hands
    // When magazine is inside the pistol the layer is "Weapon Magazine", else layer is "Default" (installing in PistolController)
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform.gameObject.layer == 6 && gameObject.layer == 11)
            return base.IsSelectableBy(interactor) && weaponInHands();
        else
            return base.IsSelectableBy(interactor);
    }
}
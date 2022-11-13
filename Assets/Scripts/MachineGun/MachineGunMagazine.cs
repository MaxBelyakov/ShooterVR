// Custom XRGrabInteractable
// Layer 6 - Player
// Layer 11 - Weapon Magazine
// Layer 0 - Default

using UnityEngine.XR.Interaction.Toolkit;

public class MachineGunMagazine : XRGrabInteractable
{
    private int _maxBullets = 50;
    private int _bullets;

    public int MaxBullets { get { return _maxBullets; } }

    public int Bullets { get { return _bullets; } }

    private MachineGunMagazine()
    {
        _bullets = _maxBullets;
    }

    public void RemoveBullet()
    {
        if (_bullets > 0)
            _bullets -= 1;
    }

    // Ignore grabbing weapon magazine when pistol not in the hands
    // When magazine is inside the pistol the layer is "Weapon Magazine", else layer is "Default" (installing in Pistol class)
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform.gameObject.layer == 6 && gameObject.layer == 11)
            return base.IsSelectableBy(interactor) && (Player.Instance.Weapon?.GetType() == typeof(MachineGun));
        else
            return base.IsSelectableBy(interactor);
    }
}
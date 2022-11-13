// Custom XRGrabInteractable
// Layer 6 - Player
// Layer 11 - Weapon Magazine
// Layer 0 - Default

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolMagazine : XRGrabInteractable
{
    private int _maxBullets = 7;
    private int _bullets;
    private bool _empty;

    public int MaxBullets { get { return _maxBullets; } }

    public int Bullets { get { return _bullets; } }

    private PistolMagazine()
    {
        _bullets = _maxBullets;
    }

    void Update()
    {
        // Pistol empty magazine has no bullets inside
        if (Bullets <= 0 && !_empty)
        {
            foreach (Transform child in transform.GetChild(0).transform)
                Destroy(child.gameObject);

            _empty = true;
        }
    }

    public void RemoveBullet()
    {
        if (_bullets > 0)
            _bullets -= 1;
    }

    // Ignore grabbing magazine when pistol not in the hands
    // When magazine is inside the pistol the layer is "Weapon Magazine", else layer is "Default" (installing in Pistol class)
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform.gameObject.layer == 6 && gameObject.layer == 11)
            return base.IsSelectableBy(interactor) && (Player.Instance.Weapon?.GetType() == typeof(Pistol));
        else
            return base.IsSelectableBy(interactor);
    }
}
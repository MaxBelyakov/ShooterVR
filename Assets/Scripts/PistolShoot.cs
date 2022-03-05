// All functions here is calling by Pistol Animation

using UnityEngine;

public class PistolShoot : ShootEffects
{
    [SerializeField] private Transform barrelLocation;              // Location of barrel
    [SerializeField] private Transform casingExitLocation;          // Location of casing exit
    [SerializeField] private Transform magazineLocation;            // Location of magazine

    //This function creates the bullet behavior
    public void Shoot()
    {   
        // Minus bullet from counter
        magazineLocation.GetChild(0).transform.GetComponent<MagazineController>().bulletsCurrent--;

        ShowShootingEffects(barrelLocation, Pistol.s_flashDestroyTimer, Pistol.s_bulletRange, Pistol.s_shotPower, Pistol.s_tag);
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, Pistol.s_ejectPower);

        // Finish shooting
        WeaponController.s_shooting = false;
    }

    void NoBulletsSounds()
    {
        // Shot sound effect
        GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);

        // Finish shooting
        WeaponController.s_shooting = false;
    }
}
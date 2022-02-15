// All functions here is calling by Machine gun Animation

using UnityEngine;

public class MachineGunShoot : ShootEffects
{
    [SerializeField] private Transform barrelLocation;              // Location of barrel
    [SerializeField] private Transform casingExitLocation;          // Location of casing exit
    [SerializeField] private Transform magazineLocation;            // Location of magazine

    // Shoot behavior
    void Shoot()
    {
        // Minus bullet from counter
        magazineLocation.GetChild(0).transform.GetComponent<MagazineController>().bulletsCurrent--;

        ShowShootingEffects(barrelLocation, MachineGun.s_flashDestroyTimer, MachineGun.s_bulletRange, MachineGun.s_shotPower);
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, MachineGun.s_ejectPower);
        
        // Finish shooting
        WeaponController.s_shooting = false;
    }

    void MachineGunNoBullets()
    {
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);
        
        // Finish shooting
        WeaponController.s_shooting = false;
    }
}
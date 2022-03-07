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

        ShowShootingEffects(barrelLocation, MachineGun.s_flashDestroyTimer, MachineGun.s_bulletRange, MachineGun.s_shotPower, MachineGun.s_tag);
    }

    // This function creates a casing at the ejection slot
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, MachineGun.s_ejectPower);
    }

    void MachineGunNoBullets()
    {
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);
    }
}
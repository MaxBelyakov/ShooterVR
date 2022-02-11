using UnityEngine;

public class MachineGunShoot : ShootEffects
{
    public Transform barrelLocation;
    public Transform casingExitLocation;

    void Update()
    {
        if (Input.GetButton("Fire1") && !WeaponController.s_reloading)
        {
            WeaponController.s_shooting = true;

            if (MachineGun.s_bulletsCurrent > 0)
                this.GetComponent<Animator>().SetTrigger("Shoot");
            else
                this.GetComponent<Animator>().SetTrigger("NoBullets");
        }
    }

    // Shoot behavior. Call by Animation
    void Shoot()
    {
        // Minus bullet from counter
        MachineGun.s_bulletsCurrent -= 1;

        ShowShootingEffects(barrelLocation, MachineGun.s_flashDestroyTimer, MachineGun.s_bulletRange, MachineGun.s_shotPower);
    }

    // This function creates a casing at the ejection slot. Call by Animation
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
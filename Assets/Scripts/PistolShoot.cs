using UnityEngine;

public class PistolShoot : ShootEffects
{
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;
    [SerializeField] private Transform magazineLocation;

    //This function creates the bullet behavior. Call by Animation
    public void Shoot()
    {   
        // Minus bullet from counter
        magazineLocation.GetChild(0).transform.GetComponent<PistolMagazineController>().bulletsCurrent--;

        ShowShootingEffects(barrelLocation, Pistol.s_flashDestroyTimer, Pistol.s_bulletRange, Pistol.s_shotPower);
    }

    // This function creates a casing at the ejection slot. Call by Animation
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
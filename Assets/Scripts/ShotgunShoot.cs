using UnityEngine;

public class ShotgunShoot : ShootEffects
{
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    // This function creates the bullet behavior. Different of ShootEffect class. Call by Animation
    void Shoot()
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(shotAudio);

        // Create the muzzle flash
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        // Destroy the muzzle flash effect
        Destroy(tempFlash, Shotgun.s_flashDestroyTimer);

        // Create buckshot
        Buckshot();
    }

    // This function creates a casing at the ejection slot. Call by Animation
    void CasingRelease()
    {
        ShowCasingEffects(casingExitLocation, Shotgun.s_ejectPower);

        // Finish shooting
        WeaponController.s_shooting = false;
    }

    void NoBulletsSounds()
    {
        // Shot sound effect
        this.GetComponent<AudioSource>().PlayOneShot(noBulletsAudio);

        // Finish shooting
        WeaponController.s_shooting = false;
    }

    // Generate random shotgun bullet points and creates impacts and holes
    void Buckshot()
    {
        // Do it for each random point
        for (int i = 0; i < Shotgun.s_buckshotBullets; i++)
        {
            // Random buckshot correction
            Vector3 correction = new Vector3(Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f), Random.Range(-0.2f,0.2f));

            // Create bullet and make force
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position + correction, barrelLocation.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * Shotgun.s_shotPower);
        }
    }
}
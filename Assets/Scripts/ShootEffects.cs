using UnityEngine;

public class ShootEffects
{
    private IWeapon _weapon;

    public ShootEffects(IWeapon weapon)
    {
        _weapon = weapon;
    }

    // Create Flash effect and Bullet force
    public void ShowShootingEffects(Transform point, float destroyTimer, float range, float shotPower, string weapon_tag)
    {
        // Shot sound effect
        _weapon.PlayAudio(_weapon.ShotAudio);

        // Create the muzzle flash
        GameObject tempFlash;
        tempFlash = GameObject.Instantiate(_weapon.MuzzleFlashPrefab, point.position, point.rotation);
        // Destroy the muzzle flash effect
        GameObject.Destroy(tempFlash, destroyTimer);

        if (_weapon.GetType() == typeof(Shotgun))
        {
            // Generate random shotgun bullet points and creates impacts and holes
            for (int i = 0; i < Shotgun.BuckshotBullets; i++)
            {
                // Random buckshot correction
                Vector3 correction = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

                // Create bullet and make force
                GameObject bullet = GameObject.Instantiate(_weapon.BulletPrefab, point.position + correction, point.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(point.forward * shotPower);
                bullet.transform.tag = weapon_tag;
            }
        }
        else
        {
            // Create bullet and make force
            GameObject bullet = GameObject.Instantiate(_weapon.BulletPrefab, point.position, point.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(point.forward * shotPower);
            bullet.transform.tag = weapon_tag;
        }
    }

    // Create casing effect
    public void ShowCasingEffects(Transform point, float power)
    {
        //Create the casing
        GameObject casing = GameObject.Instantiate(_weapon.CasingPrefab, point.position, point.rotation);
        
        //Add force on casing to push it out
        casing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(power * 0.7f, power), (point.position - point.right * 0.3f - point.up * 0.6f), 1f, 3f);
        //Add torque to make casing spin in random direction
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
    }

    public void NoBulletsEffects()
    {
        // Shot sound effect
        _weapon.PlayAudio(_weapon.NoBulletsAudio);
    }

    public void ReloadEffects()
    {
        // Shot sound effect
        _weapon.PlayAudio(_weapon.ReloadAudio);
    }

}
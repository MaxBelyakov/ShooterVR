using UnityEngine;

public class ShotgunReload : MonoBehaviour
{
    public AudioClip reloadAudio;           // Reload sound

    void Update()
    {   
        // Waiting for reload button
        if (Input.GetButtonDown("Reload") && !WeaponController.s_shooting && !WeaponController.s_reloading)
        {
            if (Shotgun.s_bulletsCurrent < Shotgun.s_bulletsAll && Inventory.s_shotgun_MagazineInventoryCurrent > 0)
            {
                WeaponController.s_reloading = true;

                this.GetComponent<Animator>().SetTrigger("reload");
            }
        }
    }

    // Add 2 bullets. Calling from Animation
    void AddBullet()
    {   
        if (Shotgun.s_bulletsCurrent < Shotgun.s_bulletsAll && Inventory.s_shotgun_MagazineInventoryCurrent > 0)
        {
            // Add bullets to counter
            Shotgun.s_bulletsCurrent ++;

            // Minus ammo in inventory
            Inventory.s_shotgun_MagazineInventoryCurrent --;

            // Reload audio effeect
            this.GetComponent<AudioSource>().PlayOneShot(reloadAudio);
        }
    }

    // Calling at the end of animation
    void FinishReloading()
    {
        WeaponController.s_reloading = false;
    }
}
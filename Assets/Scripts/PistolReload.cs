using UnityEngine;

public class PistolReload : MonoBehaviour
{
    private GameObject magazine;            // Child of pistol magazine location 

    public GameObject magazinePrefab;       // Magazine Prefab
    public Transform magazineLocation;      // Magazine location

    public AudioClip reloadAudio;           // Reload sound

    void Start()
    {
        // Find magazine in magazine location
        magazine = magazineLocation.GetChild(0).gameObject;
    }

    void Update()
    {   
        // Waiting for reload button
        /*if (Input.GetButtonDown("Reload") && !WeaponController.s_shooting && !WeaponController.s_reloading)
        {
            if (Pistol.s_bulletsCurrent < Pistol.s_bulletsAll && Inventory.s_pistol_MagazineInventoryCurrent > 0)
            {
                WeaponController.s_reloading = true;

                // Reload audio effeect
                this.GetComponent<AudioSource>().PlayOneShot(reloadAudio);

                this.GetComponent<Animator>().SetTrigger("reload");
            }
        }*/
    }

    // Drop magazine. Calling from Animation
    void DropMagazine()
    {   
        // Unpin magazin from pistol
        magazine.transform.parent = null;

        // Add rigidbody to drop magazin on the floor
        magazine.AddComponent<Rigidbody>();

    }

    // Add new magazine. Calling from Animation
    void AddMagazine()
    {   
        // Create new magazine
        GameObject newMagazine = Instantiate(magazinePrefab, magazineLocation.position, magazineLocation.rotation);

        // Pin new magazine to the pistol
        newMagazine.transform.parent = magazineLocation;
        magazine = newMagazine;

        // Add bullets to counter
        Pistol.s_bulletsCurrent = Pistol.s_bulletsAll;

        // Minus ammo in inventory
        Inventory.s_pistol_MagazineInventoryCurrent --;

        WeaponController.s_reloading = false;
    }
}
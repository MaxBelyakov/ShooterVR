// Magazine controller
// Define amount of bullets in magazine by default

using UnityEngine;

public class MagazineController : MonoBehaviour
{
    public int bulletsCurrent;

    void Start()
    {
        if (gameObject.tag == "pistol magazine ammo")
            bulletsCurrent = Pistol.s_bulletsAll;
        else if (gameObject.tag == "machine gun ammo")
            bulletsCurrent = MachineGun.s_bulletsAll;
    }
    
    void Update()
    {
        // Pistol empty magazine has no bullets inside
        if (gameObject.tag == "pistol magazine ammo" && bulletsCurrent <= 0)
            foreach (Transform child in transform.GetChild(0).transform)
                Destroy(child.gameObject);
    }
}
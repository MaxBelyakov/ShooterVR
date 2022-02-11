using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public TMP_Text bulletsText;                        // Text show current value of bullets

    public static bool s_shooting = false;              // Global flag, show weapon is shooting now
    public static bool s_reloading = false;             // Global flag, show weapon is reloading now

    public static string s_weapon = "NoWeapon";         // Name of weapon in hands at the current time

    void Start()
    {
        // Add start up bullets to pistol, machine gun and shotgun
        Pistol.s_bulletsCurrent = Pistol.s_bulletsAll;
        MachineGun.s_bulletsCurrent = MachineGun.s_bulletsAll;
        Shotgun.s_bulletsCurrent = Shotgun.s_bulletsAll;
    }

    /*void Update()
    {
        // Update bullets text depends of selected weapon
        if (s_weapon == "noWeapon")
            bulletsText.text = "";
        if (s_weapon == "Pistol")
            bulletsText.text = "Bullets: " + Pistol.s_bulletsCurrent + " / " + Pistol.s_bulletsAll;
        if (s_weapon == "Machine Gun")
            bulletsText.text = "Bullets: " + MachineGun.s_bulletsCurrent + " / " + MachineGun.s_bulletsAll;
        if (s_weapon == "Shotgun")
            bulletsText.text = "Bullets: " + Shotgun.s_bulletsCurrent + " / " + Shotgun.s_bulletsAll;
        if (s_weapon == "Bow")
            bulletsText.text = "";

        // Listen to change weapon input. Check for weapon in inventory, shooting and reloading flags
        if (Input.GetButtonDown("1") && Inventory.s_pistolItem && !s_shooting && !s_reloading)
        {
            s_weapon = "Pistol";
            machineGun.SetActive(false);
            pistol.SetActive(true);
            shotgun.SetActive(false);
            bow.SetActive(false);
        }
        if (Input.GetButtonDown("2") && Inventory.s_machineGunItem && !s_shooting && !s_reloading)
        {
            s_weapon = "Machine Gun";
            pistol.SetActive(false);
            machineGun.SetActive(true);
            shotgun.SetActive(false);
            bow.SetActive(false);
        }
        if (Input.GetButtonDown("3") && Inventory.s_shotgunItem && !s_shooting && !s_reloading)
        {
            s_weapon = "Shotgun";
            pistol.SetActive(false);
            machineGun.SetActive(false);
            shotgun.SetActive(true);
            bow.SetActive(false);
        }
        if (Input.GetButtonDown("4") && Inventory.s_bowItem && !s_shooting && !s_reloading)
        {
            s_weapon = "Bow";
            pistol.SetActive(false);
            machineGun.SetActive(false);
            shotgun.SetActive(false);
            bow.SetActive(true);
        }

        // Listen for drop weapon input. Check for shooting and reloading flags
        if (Input.GetButtonDown("Drop") && !s_shooting && !s_reloading)
        {            
            if (s_weapon == "Pistol")
            {
                // Hide weapon in hands
                pistol.SetActive(false);

                // Remove item from inventory
                Inventory.s_pistolItem = false;

                // Create new item and drop it to the ground
                GameObject pistolItem = Instantiate(pistolItemPrefab, transform.position + transform.forward + transform.up / 3f, transform.rotation);
                pistolItem.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
                pistolItem.GetComponent<Rigidbody>().AddTorque(transform.forward * 25f);

                // Select free hands
                s_weapon = "noWeapon";
                
            }
            if (s_weapon == "Machine Gun")
            {
                // Hide weapon in hands
                machineGun.SetActive(false);

                // Remove item from inventory
                Inventory.s_machineGunItem = false;

                // Create new item and drop it to the ground
                GameObject machineGunItem = Instantiate(machineGunItemPrefab, transform.position + transform.forward + transform.up / 3f, transform.rotation);
                machineGunItem.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
                machineGunItem.GetComponent<Rigidbody>().AddTorque(transform.up * 25f);

                // Select free hands
                s_weapon = "noWeapon";
            }
            if (s_weapon == "Shotgun")
            {
                // Hide weapon in hands
                shotgun.SetActive(false);

                // Remove item from inventory
                Inventory.s_shotgunItem = false;

                // Create new item and drop it to the ground
                GameObject shotgunItem = Instantiate(shotgunItemPrefab, transform.position + transform.forward + transform.up / 3f, transform.rotation);
                shotgunItem.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
                shotgunItem.GetComponent<Rigidbody>().AddTorque(transform.up * 25f);
                shotgunItem.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                // Select free hands
                s_weapon = "noWeapon";
            }
            if (s_weapon == "Bow")
            {
                // Hide weapon in hands
                bow.SetActive(false);

                // Remove item from inventory
                Inventory.s_bowItem = false;

                // Create new item and drop it to the ground
                GameObject bowItem = Instantiate(bowItemPrefab, transform.position + transform.forward + transform.up / 3f, transform.rotation);
                bowItem.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);

                // Select free hands
                s_weapon = "noWeapon";
            }
        }
    }*/
}

public class Pistol : WeaponController
{
    public static int s_bulletsAll = 77;             // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 50f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 80f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class MachineGun : WeaponController
{
    public static int s_bulletsAll = 50;            // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 50f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 100f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class Shotgun : WeaponController
{
    public static int s_bulletsAll = 2;             // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 250f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 130f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
    public static int s_buckshotBullets = 6;        // Buckshot bullets amount
}

public class Bow : WeaponController
{
    public static float stringSpeed = 130f;         // Speed of string stretch
    public static float shootSpeed = 850f;          // Speed of string unstretch
    public static float heightLimit = -0.3f;        // Bow height limit when shooting
    public static float arrowMass = 0.03f;          // Arrow mass, affect to hit power
}
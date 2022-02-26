using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public TMP_Text bulletsText;                        // Text show current value of bullets

    public static bool s_shooting = false;              // Global flag, show weapon is shooting now
    public static bool s_reloading = false;             // Global flag, show weapon is reloading now

    public static string s_weapon = "noWeapon";         // Name of weapon in hands at the current time

    void Start()
    {
        // Add start up bullets to shotgun
        Shotgun.s_bulletsCurrent = Shotgun.s_bulletsAll;
    }

    void Update()
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
    }
}

public class Pistol : WeaponController
{
    public static int s_ammoAll = 3;                // Max ammo inventory size
    public static int s_bulletsAll = 7;             // All bullets in magazine
    public static int s_bulletsCurrent = 0;         // Current bullets in magazine
    public static float s_ejectPower = 50f;         // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 80f;          // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class MachineGun : WeaponController
{
    public static int s_ammoAll = 3;                // Max ammo inventory size
    public static int s_bulletsAll = 50;            // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 50f;         // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 100f;         // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
}

public class Shotgun : WeaponController
{
    public static int s_ammoAll = 30;               // Max ammo inventory size
    public static int s_ammoBox = 10;               // Amount of bullets in box ammo
    public static int s_bulletsAll = 2;             // All bullets in magazine
    public static int s_bulletsCurrent;             // Current bullets in magazine
    public static float s_ejectPower = 250f;        // Power of casing exit
    public static float s_flashDestroyTimer = 2f;   // Shot flash destroy time
    public static float s_shotPower = 130f;         // Shot power
    public static float s_bulletRange = 100f;       // Bullet working distance
    public static int s_buckshotBullets = 6;        // Buckshot bullets amount
}

public class Bow : WeaponController
{
    public static int s_ammoAll = 30;               // Max arrows inventory size
    public static int s_ammoBox = 10;               // Amount of arrows in quiver
    public static float shootSpeed = 350f;          // Speed of string unstretch
    public static float arrowSpeedStick = 8f;       // Arrow speed limit when start stick in objects or impact effect
    public static float arrowDropSpeedLimit = 3f;   // Arrow drop speed limit when can play drop sound
    public static float inertia = 1000f;            // String inertia divider
    public static float forceReducing = 70f;        // Arrow move force reducing value
    public static float depth = 0.3f;               // Depth that arrow move in target
}
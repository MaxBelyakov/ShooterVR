using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int pistolMagazineInventoryAll = 3;                           // Max value of pistol magazines in inventory
    public static int s_pistol_MagazineInventoryCurrent = 0;             // Current value of pistol magazines in inventory
    public TMP_Text pistol_MagazineInventoryText;                        // Text ammo status in inventory
    public RawImage pistol_ItemTexture;
    public TMP_Text pistol_ItemText;

    public int machineGun_MagazineInventoryAll = 3;                     // Max value of machine gun magazines in inventory
    public static int s_machineGun_MagazineInventoryCurrent = 0;        // Current value of machine gun magazines in inventory
    public TMP_Text machineGun_MagazineInventoryText;                   // Text ammo status in inventory
    public RawImage machineGun_ItemTexture;
    public TMP_Text machineGun_ItemText;

    private int shotgun_MagazineInventoryAll = 30;                      // Max value of shotgun bullets in inventory
    public static int s_shotgun_MagazineInventoryCurrent = 0;           // Current value of shotgun bullets in inventory
    public TMP_Text shotgun_MagazineInventoryText;                      // Text ammo status in inventory
    private int shotgun_ammoPackValue = 10;                             // Value of bullets in 1 ammo pack
    public RawImage shotgun_ItemTexture;
    public TMP_Text shotgun_ItemText;

    private int bow_MagazineInventoryAll = 30;                          // Max value of arrows in inventory
    public static int s_bow_MagazineInventoryCurrent = 0;               // Current value of arrows in inventory
    public TMP_Text bow_MagazineInventoryText;                          // Text ammo status in inventory
    private int bow_ammoPackValue = 10;                                 // Value of arrows in 1 ammo pack
    public RawImage bow_ItemTexture;
    public TMP_Text bow_ItemText;

    public static bool s_pistolItem = false;
    public static bool s_machineGunItem = false;
    public static bool s_shotgunItem = false;
    public static bool s_bowItem = false;

    public TMP_Text popup;                                              // Pop up text to interact with object

    public AudioClip a_magazineTake;                                    // Take magazine sound effect
    
    void Update()
    {
        // Clear popups
        popup.text = "";

        // Update ammo status text
        pistol_MagazineInventoryText.text = s_pistol_MagazineInventoryCurrent + " / " + pistolMagazineInventoryAll;
        machineGun_MagazineInventoryText.text = s_machineGun_MagazineInventoryCurrent + " / " + machineGun_MagazineInventoryAll;
        shotgun_MagazineInventoryText.text = s_shotgun_MagazineInventoryCurrent + " / " + shotgun_MagazineInventoryAll;
        bow_MagazineInventoryText.text = s_bow_MagazineInventoryCurrent + " / " + bow_MagazineInventoryAll;

        // Update inventory weapon color
        UpdateColor (s_pistolItem, pistol_ItemTexture, pistol_ItemText);
        UpdateColor (s_machineGunItem, machineGun_ItemTexture, machineGun_ItemText);
        UpdateColor (s_shotgunItem, shotgun_ItemTexture, shotgun_ItemText);
        UpdateColor (s_bowItem, bow_ItemTexture, bow_ItemText);

        // Check for target
        if (InspectTarget.targetInfo != null)
        {
            // Ask to take ammo
            if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo" 
                || InspectTarget.targetInfo.TargetItem == "machine gun ammo"
                || InspectTarget.targetInfo.TargetItem == "shotgun ammo"
                || InspectTarget.targetInfo.TargetItem == "bow ammo"
                || InspectTarget.targetInfo.TargetItem == "arrow"
                || InspectTarget.targetInfo.TargetItem == "pistol item"
                || InspectTarget.targetInfo.TargetItem == "machine gun item"
                || InspectTarget.targetInfo.TargetItem == "shotgun item"
                || InspectTarget.targetInfo.TargetItem == "bow item")
                    popup.text = "To take press 'E'";

            // Take ammo after check for shooting or reloading
            if (Input.GetButtonDown("Submit") && !WeaponController.s_shooting && !WeaponController.s_reloading)
            {
                // Put pistol ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "pistol magazine ammo")
                {
                    if (s_pistol_MagazineInventoryCurrent < pistolMagazineInventoryAll) {
                        s_pistol_MagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put machine gun ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "machine gun ammo")
                {
                    if (s_machineGun_MagazineInventoryCurrent < machineGun_MagazineInventoryAll) {
                        s_machineGun_MagazineInventoryCurrent ++;
                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put shotgun ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "shotgun ammo")
                {
                    if (s_shotgun_MagazineInventoryCurrent < shotgun_MagazineInventoryAll) {
                        s_shotgun_MagazineInventoryCurrent += shotgun_ammoPackValue;

                        // Decrease amount of bullets if take more than inventory limit
                        if (s_shotgun_MagazineInventoryCurrent > shotgun_MagazineInventoryAll)
                            s_shotgun_MagazineInventoryCurrent = shotgun_MagazineInventoryAll;

                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put bow ammo in inventory
                if (InspectTarget.targetInfo.TargetItem == "bow ammo")
                {
                    if (s_bow_MagazineInventoryCurrent < bow_MagazineInventoryAll) {
                        s_bow_MagazineInventoryCurrent += bow_ammoPackValue;

                        // Decrease amount of bullets if take more than inventory limit
                        if (s_bow_MagazineInventoryCurrent > bow_MagazineInventoryAll)
                            s_bow_MagazineInventoryCurrent = bow_MagazineInventoryAll;

                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put arrow in inventory
                if (InspectTarget.targetInfo.TargetItem == "arrow")
                {
                    if (s_bow_MagazineInventoryCurrent < bow_MagazineInventoryAll) {
                        s_bow_MagazineInventoryCurrent ++;

                        Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                        // Play sound effect
                        this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                    }
                }
                // Put pistol item in inventory
                if (InspectTarget.targetInfo.TargetItem == "pistol item")
                {
                    s_pistolItem = true;
                    
                    Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                    // Play sound effect
                    this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                }
                // Put machine gun item in inventory
                if (InspectTarget.targetInfo.TargetItem == "machine gun item")
                {
                    s_machineGunItem = true;
                    
                    Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                    // Play sound effect
                    this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                }
                // Put shotgun item in inventory
                if (InspectTarget.targetInfo.TargetItem == "shotgun item")
                {
                    s_shotgunItem = true;
                    
                    Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                    // Play sound effect
                    this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                }
                // Put bow item in inventory
                if (InspectTarget.targetInfo.TargetItem == "bow item")
                {
                    s_bowItem = true;
                    
                    Destroy(InspectTarget.targetInfo.TargetObject, 0f);

                    // Play sound effect
                    this.GetComponent<AudioSource>().PlayOneShot(a_magazineTake);
                }
            }
        }
    }

    void UpdateColor(bool itemFlag, RawImage itemTexture, TMP_Text itemText)
    {
        if (itemFlag)
        {
            itemTexture.color = new Color(itemTexture.color.r, itemTexture.color.g, itemTexture.color.b, 1f);
            itemText.color = new Color(itemText.color.r, itemText.color.g, itemText.color.b, 1f);
        } else {
            itemTexture.color = new Color(itemTexture.color.r, itemTexture.color.g, itemTexture.color.b, 0.3f);
            itemText.color = new Color(itemText.color.r, itemText.color.g, itemText.color.b, 0.3f);
        }
    }
}
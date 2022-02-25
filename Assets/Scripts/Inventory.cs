using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int pistolMagazineInventoryAll = 3;                           // Max value of pistol magazines in inventory
    public static int s_pistol_MagazineInventoryCurrent = 0;             // Current value of pistol magazines in inventory
    public TMP_Text pistol_MagazineInventoryText;                        // Text ammo status in inventory
   

    private int shotgun_MagazineInventoryAll = 30;                      // Max value of shotgun bullets in inventory
    public static int s_shotgun_MagazineInventoryCurrent = 0;           // Current value of shotgun bullets in inventory
    public TMP_Text shotgun_MagazineInventoryText;                      // Text ammo status in inventory
    //private int shotgun_ammoPackValue = 10;                             // Value of bullets in 1 ammo pack

    private int bow_MagazineInventoryAll = 30;                          // Max value of arrows in inventory
    public static int s_bow_MagazineInventoryCurrent = 0;               // Current value of arrows in inventory
    public TMP_Text bow_MagazineInventoryText;                          // Text ammo status in inventory
    //private int bow_ammoPackValue = 10;                                 // Value of arrows in 1 ammo pack
      
}
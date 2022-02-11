using UnityEngine;
using Random=UnityEngine.Random;

public class DummyGenerator : MonoBehaviour
{
    public GameObject pistolDummy;                      // Pistol dummy prefab
    public GameObject machineGunDummy;                  // Machine gun dummy prefab
    public GameObject shotgunDummy;                     // Shotgun dummy prefab
    public GameObject bowDummy;                         // Bow dummy prefab

    public GameObject dummyGeneratorField;              // Field with box collider where dummy can respawn

    public static string s_dummyWeapon;                 // Current dummy weapon flag
    public static float s_dummyMass = 3f;               // Dummy mass

    public static bool s_dummy = false;                 // Dummy exists flag

    void Update()
    {
        // Check for dummy in game
        if (!s_dummy)
        {
            // Random choose dummy item and save dummy weapon flag
            object[,] dummyArray = new object[,] 
                {{pistolDummy, "Pistol"}, {machineGunDummy, "Machine Gun"}, {shotgunDummy, "Shotgun"}, {bowDummy, "Bow"}};
            int rnd = new System.Random().Next(0, 4);
            GameObject dummyItem = (GameObject) dummyArray[rnd, 0];
            s_dummyWeapon = (string) dummyArray[rnd, 1];

            // Define dummy size (in current prefab dummy transform is a child of dummy gameobject)
            Vector3 dummySize = dummyItem.transform.GetChild(0).transform.GetComponent<Renderer>().bounds.size;

            // Random choose dummy position inside dummy respawn field
            var fieldX = dummyGeneratorField.transform.position.x;
            var borderX = dummyGeneratorField.transform.GetComponent<BoxCollider>().bounds.size.x;
            var x = Random.Range(fieldX - borderX / 2, fieldX + borderX / 2);

            var fieldZ = dummyGeneratorField.transform.position.z;
            var borderZ = dummyGeneratorField.transform.GetComponent<BoxCollider>().bounds.size.z;
            var z = Random.Range(fieldZ - borderZ / 2, fieldZ + borderZ / 2);

            var fieldY = dummyGeneratorField.transform.localPosition.y;
            var borderY = dummyGeneratorField.transform.GetComponent<BoxCollider>().bounds.size.y;
            var y = Random.Range(fieldY - borderY / 2 + dummySize.y,  fieldY + borderY / 2 - dummySize.y);

            Vector3 dummyPosition = new Vector3 (x, y, z);

            // Avoid collision with another objects before dummy creation
            if (!Physics.CheckBox(dummyPosition, dummySize / 2, dummyGeneratorField.transform.rotation))
            {
                s_dummy = true;
                
                // Create dummy
                Instantiate(dummyItem, dummyPosition, dummyGeneratorField.transform.rotation);
            }
        }
    }
}
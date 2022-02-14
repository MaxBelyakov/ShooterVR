// Pistol magazine controller
// Define amount of bullets in magazine by default

using UnityEngine;

public class PistolMagazineController : MonoBehaviour
{
    public int bulletsCurrent = 7;
    
    void Update()
    {
        // Empty magazine has no bullets inside
        if (bulletsCurrent <= 0)
            foreach (Transform child in transform.GetChild(0).transform)
                Destroy(child.gameObject);
    }
}
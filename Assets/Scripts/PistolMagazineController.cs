using UnityEngine;

public class PistolMagazineController : MonoBehaviour
{
    public int bulletsCurrent = 7;
    
    void Update()
    {
        if (bulletsCurrent <= 0)
        {
            foreach (Transform child in transform.GetChild(0).transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

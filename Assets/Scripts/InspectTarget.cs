using UnityEngine;

// Contains information about current target
public class TargetInfo
{
    public string Name { get; set; }                // Target name
    public float Distance { get; set; }             // Distance to target
    public string TargetItem { get; set; }          // Target inventory identificator
    public GameObject TargetObject { get; set; }    // Target object
}

// Collect information about current target
public class InspectTarget : MonoBehaviour
{
    private float submitDistance = 2.4f;               // Distance to interact with object

    public static TargetInfo targetInfo;            // Save information about current target here

    void Update()
    {
        // Inspect target element. Look inside collider element to find other colliders inside collider.
        // Use capasule cast to expand ray of search
        RaycastHit target;
        Camera FPSCamera = this.GetComponent<Camera>();
        if (Physics.CapsuleCast(FPSCamera.transform.position, FPSCamera.transform.position, 0.05f, FPSCamera.transform.forward, out target, submitDistance))
        {
            // Save information
            targetInfo = new TargetInfo 
            { 
                Name = target.collider.transform.name, 
                Distance = target.distance, 
                TargetItem = target.collider.transform.tag, 
                TargetObject = target.collider.transform.gameObject 
            };
        } else
            targetInfo = null;
    }
}
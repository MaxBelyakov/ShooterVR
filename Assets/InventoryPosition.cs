using UnityEngine;

public class InventoryPosition : MonoBehaviour
{
    public Transform headCamera;

    void Update()
    {
        float x = headCamera.position.x;
        float y = headCamera.position.y - 0.5f;
        float z = headCamera.position.z;

        transform.position = new Vector3(x, y, z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, headCamera.eulerAngles.y, transform.eulerAngles.z);
    }
}
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TMP_Text bulletsText;  // Text show current value of bullets

    [SerializeField] private Transform headCamera;

    void Update()
    {
        // Inventory rotation
        float x = headCamera.position.x;
        float y = headCamera.position.y - 0.5f;
        float z = headCamera.position.z;

        transform.position = new Vector3(x, y, z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, headCamera.eulerAngles.y, transform.eulerAngles.z);

        // Show bullets
        ShowBullets();
    }

    public void ShowBullets()
    {
        if (Player.Instance.Weapon != null)
            bulletsText.text = Player.Instance.Weapon.GetBullets().ToString() + " / " + Player.Instance.Weapon.GetMaxBullets().ToString();
        else
            bulletsText.text = "";
    }
}
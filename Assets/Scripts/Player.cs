using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public IWeapon Weapon { get; set; }

    private void Awake()
    {
        Instance = this;
    }
}
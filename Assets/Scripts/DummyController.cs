// Dummy item controller. Is a child of DummyGenerator

using UnityEngine;

public class DummyController : DummyGenerator
{
    // Listening for collision something with dummy
    void OnCollisionEnter(Collision collision)
    {
        // Dummy weapon compares with current player weapon and start drop the dummy
        if ((collision.transform.tag == Shotgun.s_tag && s_dummyWeapon == "Shotgun")
        || (collision.transform.tag == MachineGun.s_tag && s_dummyWeapon == "Machine Gun")
        || (collision.transform.tag == Pistol.s_tag && s_dummyWeapon == "Pistol")
        || (collision.transform.tag == "arrow" && s_dummyWeapon == "Bow"))
            DropDummy();
    }

    void DropDummy()
    {
        // Avoid duplicate rigidbody component (foe example when shotgun shoot)
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            // Dummy drop to the ground
            gameObject.AddComponent<Rigidbody>();
            transform.GetComponent<Rigidbody>().mass = s_dummyMass;

            // Need new dummy flag
            s_dummy = false;
        }
    }
}
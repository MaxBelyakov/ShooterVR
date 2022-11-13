// Dummy item controller. Is a child of DummyGenerator

using UnityEngine;

public class DummyController : DummyGenerator
{
    // Listening for collision something with dummy
    void OnCollisionEnter(Collision collision)
    {
        // Dummy weapon compares with current player weapon and start drop the dummy
        if ((collision.transform.tag == "shotgun bullet" && s_dummyWeapon == "Shotgun")
        || (collision.transform.tag == "machine gun bullet" && s_dummyWeapon == "Machine Gun")
        || (collision.transform.tag == "pistol bullet" && s_dummyWeapon == "Pistol")
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
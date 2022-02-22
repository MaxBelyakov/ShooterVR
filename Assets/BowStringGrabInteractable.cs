using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BowStringGrabInteractable : XRGrabInteractable
{
    public Transform bowBody;
    public Transform stringStart;
    public Transform stringEnd;

    private Vector3 stringStartPosition;
    private Vector3 stringEndPosition;
    private Vector3 stringPosition;

    private float stringPower;

    private float stringX;
    private float stringY;
    private float stringZ;
    private float stringEndZ;

    private bool bowFire;
    private bool stringInertia = false;                 // Need to return string in start position after inertia

    public AudioClip shootAudio;                        // Bow shoot sound

    void Start()
    {
        stringStartPosition = stringStart.localPosition;
        stringEndPosition = stringEnd.localPosition;

        stringX = stringStartPosition.x;
        stringY = stringStartPosition.y;
        stringZ = stringStartPosition.z;
        stringEndZ = stringEndPosition.z;
    }

    void Update()
    {
        // Return string to start position with inertia correction
        if (bowFire && transform.localPosition.z <= stringZ + stringPower / 1000f)
        {
            transform.localPosition += new Vector3(0, 0, 0.1f) * Time.deltaTime * stringPower;
        }
        else if (bowFire)
        {
            // Signal to return string in start position
            stringInertia = true;
        }

        // Return string to start position after inertia correction
        if (stringInertia && transform.localPosition.z > stringZ)
        {
            bowFire = false;
            transform.localPosition += new Vector3(0, 0, -0.01f) * Time.deltaTime * stringPower;
        } else if (stringInertia)
        {
            // Inertia finished, shooting process is finished
            stringInertia = false;
            WeaponController.s_shooting = false;
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (!bowFire && isSelected)
        {         
            if (transform.localPosition.x != stringX || transform.localPosition.y != stringY)
                transform.localPosition = new Vector3(stringX, stringY, transform.localPosition.z);

            if (transform.localPosition.z < stringEndZ)
                transform.localPosition = new Vector3(stringX, stringY, stringEndZ);
            if (transform.localPosition.z > stringZ)
                transform.localPosition = new Vector3(stringX, stringY, stringZ);

            

            base.ProcessInteractable(updatePhase);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        transform.parent = bowBody;
        transform.position = stringStartPosition;

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Calculate string power
        stringPower = (Mathf.Abs(transform.localPosition.z) - Mathf.Abs(stringZ)) * Bow.shootSpeed;

        // There is a difference between "s_shooting" and "bowFire"
        // First show that weapon is working, second - release the string flag
        WeaponController.s_shooting = true;
        bowFire = true;

        // Shoot sound
        GetComponent<AudioSource>().PlayOneShot(shootAudio);

        base.OnSelectExited(args);
    }
}
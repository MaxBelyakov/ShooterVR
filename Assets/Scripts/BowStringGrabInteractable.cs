using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BowStringGrabInteractable : XRGrabInteractable
{
    public XRGrabInteractable bowGrabInteractable;      // Bow Grab Interactable component

    public Transform bowBody;                           // Bow main body transform component
    public Transform stringStart;                       // String start position
    public Transform stringEnd;                         // String end position

    private float stringPower;                          // String power, depends on string length

    private float stringX;                              // String start X position
    private float stringY;                              // String start Y position
    private float stringZ;                              // String start Z position

    private bool bowFire;                               // There is a difference between "s_shooting" and "bowFire
                                                        // First show that weapon is working, second - release the string flag

    private bool stringInertia = false;                 // Need to return string in start position after inertia

    public AudioClip shootAudio;                        // Bow shoot sound

    void Start()
    {
        // Get string start coordinates
        stringX = stringStart.localPosition.x;
        stringY = stringStart.localPosition.y;
        stringZ = stringStart.localPosition.z;

        // Add listener for drop bow trigger
        bowGrabInteractable.selectExited.AddListener(DropStringAndArrow);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Get the notch in hand
        if (!bowFire && isSelected)
        {
            // Move string with hand
            stringStart.localPosition = transform.localPosition;
            
            // Allow string move just by Z axis
            if (stringStart.localPosition.x != stringX || transform.localPosition.y != stringY)
                stringStart.localPosition = new Vector3(stringX, stringY, stringStart.localPosition.z);

            // Allow string move just between string start and end Z coordinates
            if (stringStart.localPosition.z < stringEnd.localPosition.z)
                stringStart.localPosition = new Vector3(stringX, stringY, stringEnd.localPosition.z);
            if (stringStart.localPosition.z > stringZ)
                stringStart.localPosition = new Vector3(stringX, stringY, stringZ);

            base.ProcessInteractable(updatePhase);
        }
    }

    void Update()
    {
        // Return string to start position + add inertia, then shoot arrow
        if (bowFire && stringStart.localPosition.z <= stringZ + stringPower / Bow.inertia)
            stringStart.localPosition += new Vector3(0, 0, 0.1f) * Time.deltaTime * stringPower;
        else if (bowFire)
            ArrowShoot();

        // Return string to start position after inertia
        if (stringInertia && stringStart.localPosition.z > stringZ)
            stringStart.localPosition += new Vector3(0, 0, -0.01f) * Time.deltaTime * stringPower;
        else if (stringInertia)
        {
            // Inertia finished, shooting process is finished
            stringInertia = false;
            Bow.s_shooting = false;
        }
    }

    // Get the notch in hand
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Reparent notch back to bow (there is no parenting when grab)
        transform.parent = bowBody;

        base.OnSelectEntered(args);
    }

    // Release the notch
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Calculate string power. Shoot speed multiplicates with string length
        float stringPosition = stringStart.localPosition.z;
        stringPower = (Mathf.Abs(stringPosition) - Mathf.Abs(stringZ)) * Bow.shootSpeed;

        // Release string flag
        bowFire = true;

        // Shoot sound
        GetComponent<AudioSource>().PlayOneShot(shootAudio);

        // Return notch to start position
        transform.localPosition = new Vector3(stringX, stringY, stringZ);

        base.OnSelectExited(args);
    }

    private void ArrowShoot()
    {
        // Signal to return string in start position
        stringInertia = true;

        // String released
        bowFire = false;

        // Arrow inside the notch socket
        if (stringStart.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            // Select arrow
            IXRSelectInteractable interactable = stringStart.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            Transform arrow = interactable.transform;

            // Start shooting flag (before arrow release to avoid socketing again)
            Bow.s_shooting = true;

            // Release arrow
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);

            // Give the force to arrow
            arrow.GetComponent<Rigidbody>().AddForce(arrow.forward * Bow.shootSpeed * stringPower / Bow.forceReducing);
        }
    }

    // Listener. Bow drop trigger
    private void DropStringAndArrow(SelectExitEventArgs arg0)
    {
        // Notch in the hand. Release the notch
        if (isSelected)
            interactionManager.SelectExit(interactorsSelecting[0], this);

        // Arrow inside the notch socket. Release the arrow
        if (stringStart.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            IXRSelectInteractable interactable = stringStart.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);
        }
    }
}
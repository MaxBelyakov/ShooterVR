using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Bow
{
    public static int s_ammoAll = 30;                       // Max arrows inventory size
    public static int s_ammoBox = 10;                       // Amount of arrows in quiver
    public static float shootSpeed = 350f;                  // Speed of string unstretch
    public static float arrowSpeedStick = 8f;               // Arrow speed limit when start stick in objects or impact effect
    public static float arrowDropSpeedLimit = 3f;           // Arrow drop speed limit when can play drop sound
    public static float inertia = 1000f;                    // String inertia divider
    public static float forceReducing = 70f;                // Arrow move force reducing value
    public static float depth = 0.3f;                       // Depth that arrow move in target
    public static bool s_shooting = false;                  // Global flag, show bow is shooting now
}

public class BowNotchGrabInteractable : XRGrabInteractable
{
    [SerializeField] private XRGrabInteractable bowGrabInteractable;      // Bow Grab Interactable component

    [SerializeField] private Transform bowBody;                           // Bow main body transform component
    [SerializeField] private Transform bowString;                         // String start position

    private Vector3 stringStart;                        // String start position
    private Vector3 stringEnd;                          // String end position

    private float stringPower;                          // String power, depends on string length

    private bool bowFire;                               // There is a difference between "s_shooting" and "bowFire
                                                        // First show that weapon is working, second - release the string flag

    private bool stringInertia = false;                 // Need to return string in start position after inertia

    public AudioClip shootAudio;                        // Bow shoot sound

    void Start()
    {
        // Get string start coordinates
        stringStart = bowString.localPosition;
        stringEnd = stringStart + new Vector3(0, 0, -0.0355f);

        // Add listener for drop bow trigger
        bowGrabInteractable.selectExited.AddListener(DropStringAndArrow);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Get the notch in hand
        if (!bowFire && isSelected)
        {
            // Move string with hand
            Vector3 newPosition = new Vector3(stringStart.x, stringStart.y, transform.localPosition.z);
            if (newPosition.z >= stringEnd.z && newPosition.z <= stringStart.z)
                bowString.localPosition = newPosition;

            base.ProcessInteractable(updatePhase);
        }
    }

    void Update()
    {
        if (bowFire)
            StringForward();

        if (stringInertia)
            StringInertia();
    }

    // Return string to start position + add inertia, then shoot arrow
    private void StringForward()
    {
        if (bowString.localPosition.z <= stringStart.z + stringPower / Bow.inertia)
            bowString.localPosition += new Vector3(0, 0, 0.1f) * Time.deltaTime * stringPower;
        else
        {
            bowFire = false;
            ArrowShoot();
        }
    }

    // Return string to start position after inertia
    private void StringInertia()
    {
        if (bowString.localPosition.z > stringStart.z)
            bowString.localPosition += new Vector3(0, 0, -0.01f) * Time.deltaTime * stringPower;
        else
        {
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
        float stringPosition = bowString.localPosition.z;
        stringPower = (Mathf.Abs(stringPosition) - Mathf.Abs(stringStart.z)) * Bow.shootSpeed;

        // Release string flag
        bowFire = true;

        // Shoot sound
        GetComponent<AudioSource>().PlayOneShot(shootAudio);

        // Return notch to start position
        transform.localPosition = stringStart;

        base.OnSelectExited(args);
    }

    private void ArrowShoot()
    {
        // Arrow inside the notch socket
        if (bowString.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            // Select arrow
            IXRSelectInteractable interactable = bowString.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            Transform arrow = interactable.transform;

            // Start shooting flag (before arrow release to avoid socketing again)
            Bow.s_shooting = true;

            // Release arrow
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);

            // Give the force to arrow
            arrow.GetComponent<Rigidbody>().AddForce(arrow.forward * Bow.shootSpeed * stringPower / Bow.forceReducing);
        }

        // Signal to return string in start position
        stringInertia = true;
    }

    // Listener. Bow drop trigger
    private void DropStringAndArrow(SelectExitEventArgs arg0)
    {
        // Notch in the hand. Release the notch
        if (isSelected)
            interactionManager.SelectExit(interactorsSelecting[0], this);

        // Arrow inside the notch socket. Release the arrow
        if (bowString.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            IXRSelectInteractable interactable = bowString.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);
        }
    }
}
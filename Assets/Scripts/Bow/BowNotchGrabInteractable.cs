using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BowNotchGrabInteractable : XRGrabInteractable
{
    [SerializeField] private Transform _bowString;

    private Bow _bow;

    private Vector3 _stringStart;                        // String start position
    private Vector3 _stringEnd;                          // String end position

    private float _stringPower;                          // String power, depends on string length
    private bool _stringForward = false;                 // Start move string forward
    private bool _stringInertia = false;                 // Return string in start position after inertia

    void Start()
    {
        _bow = transform.parent.GetComponent<Bow>();

        // Get string start coordinates
        _stringStart = _bowString.localPosition;
        _stringEnd = _stringStart + new Vector3(0, 0, -0.0355f);

        // Add listener for drop bow trigger
        XRGrabInteractable bowGrab = _bow.transform.parent.parent.GetComponent<XRGrabInteractable>();
        bowGrab.selectExited.AddListener(DropStringAndArrow);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        // Get the notch in hand
        if (isSelected)
        {
            // Move string with hand
            Vector3 newPosition = new Vector3(_stringStart.x, _stringStart.y, transform.localPosition.z);
            if (newPosition.z >= _stringEnd.z && newPosition.z <= _stringStart.z)
                _bowString.localPosition = newPosition;

            base.ProcessInteractable(updatePhase);
        }
    }

    void Update()
    {
        if (_stringForward)
            StringForward();

        if (_stringInertia)
            StringInertia();
    }

    // Return string to start position + add inertia, then shoot arrow
    private void StringForward()
    {
        if (_bowString.localPosition.z <= _stringStart.z + _stringPower / _bow.Inertia)
            _bowString.localPosition += new Vector3(0, 0, 0.1f) * Time.deltaTime * _stringPower;
        else
        {
            _stringForward = false;
            ArrowShoot();
        }
    }

    // Return string to start position after inertia
    private void StringInertia()
    {
        if (_bowString.localPosition.z > _stringStart.z)
            _bowString.localPosition += new Vector3(0, 0, -0.01f) * Time.deltaTime * _stringPower;
        else
        {
            _stringInertia = false;
        }
    }

    // Get the notch in hand
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Reparent notch back to bow (there is no parenting when grab)
        transform.parent = _bow.transform;

        base.OnSelectEntered(args);
    }

    // Release the notch
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Calculate string power. Shoot speed multiplicates with string length
        float stringPosition = _bowString.localPosition.z;
        _stringPower = (Mathf.Abs(stringPosition) - Mathf.Abs(_stringStart.z)) * _bow.ShootSpeed;

        _stringForward = true;

        // Shoot sound
        _bow.PlayAudio(_bow.ShotAudio);

        // Return notch to start position
        transform.localPosition = _stringStart;

        base.OnSelectExited(args);
    }

    private void ArrowShoot()
    {
        // Arrow inside the notch socket
        if (_bowString.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            // Select arrow
            IXRSelectInteractable interactable = _bowString.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            Transform arrow = interactable.transform;

            // Release arrow
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);

            // Give the force to arrow
            arrow.GetComponent<Rigidbody>().AddForce(arrow.forward * _bow.ShootSpeed * _stringPower / _bow.ForceReducing);
        }

        // Signal to return string in start position
        _stringInertia = true;
    }

    // Listener. Bow drop trigger
    private void DropStringAndArrow(SelectExitEventArgs arg0)
    {
        // Notch in the hand. Release the notch
        if (isSelected)
            interactionManager.SelectExit(interactorsSelecting[0], this);

        // Arrow inside the notch socket. Release the arrow
        if (_bowString.GetComponent<XRSocketInteractorBow>().hasSelection)
        {
            IXRSelectInteractable interactable = _bowString.GetComponent<XRSocketInteractorBow>().interactablesSelected[0];
            interactionManager.SelectExit(interactable.interactorsSelecting[0], interactable);
        }
    }
}
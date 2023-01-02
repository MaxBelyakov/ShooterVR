using UnityEngine;

[System.Serializable]

public class MapTransform
{
    public Transform vrTarget;
    public Transform ikTarget;

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void VRMapping()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class AvatarController : MonoBehaviour
{
    private MapTransform head;
    private MapTransform leftHand;
    private MapTransform rightHand;

    private GameObject xrOrigin;
    private Transform ikHead;

    private float turnSmoothness = 2f;
    private float moveValue = 3f;               // Animator parametr for moving
    private float angleValue = 0.2f;            // Animator parametr for rotating
    private float speedValue = 8f;              // Animator parametr for speed

    public MapTransform Head { get => head; set => head = value; }
    public Transform IkHead { get => ikHead; set => ikHead = value; }
    public MapTransform LeftHand { get => leftHand; set => leftHand = value; }
    public MapTransform RightHand { get => rightHand; set => rightHand = value; }
    public GameObject XROrigin { get => xrOrigin; set => xrOrigin = value; }

    private void LateUpdate()
    {
        if (XROrigin == null)
            return;

        // Define the way of moving and animation speed
        float x;
        float y;

        if (transform.position.x - IkHead.position.x < 0f)
            x = -moveValue;
        else
            x = moveValue;

        if (transform.position.z - IkHead.position.z > 0f)
            y = -moveValue;
        else
            y = moveValue;

        Vector2 dir = new Vector2(x, y);
        float speed = (Mathf.Abs(transform.position.x - IkHead.position.x) + Mathf.Abs(transform.position.z - IkHead.position.z)) * speedValue;

        // Avatar follow camera with animation. Works when XRRig has no velocity (no move by controllers)
        //if (transform.CurrentSpeed == 0)
        //{
            //visitor.OnBodyMoveByCamera(dir, speed);
            transform.position = Vector3.Lerp(transform.position, new Vector3(IkHead.position.x, transform.position.y, IkHead.position.z), Time.deltaTime * turnSmoothness);

            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IkHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
        //}

        head.VRMapping();
        leftHand.VRMapping();
        rightHand.VRMapping();
    }
}

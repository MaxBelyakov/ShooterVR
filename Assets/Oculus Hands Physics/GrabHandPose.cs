#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public GhostHands GhostHands;
    public HandData leftHandPose;
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    XRGrabInteractable grabInteractable;
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(GrabWeapon);
        grabInteractable.selectExited.AddListener(DropWeapon);
        grabInteractable.hoverEntered.AddListener(HoverWeapon);

        leftHandPose.gameObject.SetActive(false);
        rightHandPose.gameObject.SetActive(false);
    }

    private void HoverWeapon(HoverEnterEventArgs arg0)
    {
        if (arg0.interactorObject.transform.CompareTag("left hand"))
            grabInteractable.attachTransform = leftAttachTransform;
        else if (arg0.interactorObject.transform.CompareTag("right hand"))
            grabInteractable.attachTransform = rightAttachTransform;
    }

    private void DropWeapon(SelectExitEventArgs arg0)
    {
        UnsetPose(GhostHands.ghostHandDirectInteractor);
    }

    private void GrabWeapon(SelectEnterEventArgs arg0)
    {
        SetupPose(GhostHands.ghostHandDirectInteractor);
    }

    public void SetupPose(XRDirectInteractor arg)
    {
        if (arg is XRDirectInteractor)
        {
            HandData handData = arg.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if (handData.handType == HandData.HandModelType.Left)
                SetHandDataValues(handData, leftHandPose);
            else
                SetHandDataValues(handData, rightHandPose);

            SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
        }
    }

    public void UnsetPose(XRDirectInteractor arg)
    {
        if (arg is XRDirectInteractor)
        {
            HandData handData = arg.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Mirror Selected Left Grab Pose")]
    public static void MirrorLeftPose()
    {
        Debug.Log("Mirror left pose");
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        handPose.MirrorPose(handPose.rightHandPose, handPose.leftHandPose);
    }
#endif

    public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;

        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
        mirroredQuaternion.y *= -1;
        mirroredQuaternion.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredQuaternion;

        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }
    }
}

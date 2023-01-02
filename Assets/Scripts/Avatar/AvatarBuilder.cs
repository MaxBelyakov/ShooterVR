using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AvatarBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _XROrigin;
    [SerializeField] private GameObject _VRRigForAvatar;

    private Transform mainCamera;
    private Transform leftHand;
    private Transform rightHand;

    private Transform rigHead;
    private Transform rigLeftHandTarget;
    private Transform rigRightHandTarget;

    private Transform avatarHead;
    private Transform avatarLeftArm;
    private Transform avatarRightArm;

    private void Start()
    {
        // Get VR Player components by name/tag
        foreach (Transform item in _XROrigin.GetComponentsInChildren<Transform>())
        {
            if (item.name == "Main Camera")
                mainCamera = item;
            if (item.name == "LeftHand interactor")
                leftHand = item;
            if (item.name == "RightHand interactor")
                rightHand = item;
        }

        // Load VRRig
        /*var VRRig = Resources.Load<GameObject>("VRRigForAvatar");
        var VRRigForAvatar = GameObject.Instantiate(VRRig);
        VRRigForAvatar.transform.parent = transform;
        VRRigForAvatar.transform.position = transform.position;
        VRRigForAvatar.transform.rotation = transform.rotation;*/

        // Get VRRig components by name/tag
        foreach (Transform item in _VRRigForAvatar.GetComponentsInChildren<Transform>())
        {
            if (item.name == "IKHead")
                rigHead = item;
            if (item.name == "LeftHand target")
                rigLeftHandTarget = item;
            if (item.name == "RightHand target")
                rigRightHandTarget = item;
        }

        // Add Rig Builder components to Avatar
        var rigBuilder = transform.AddComponent<RigBuilder>();
#if UNITY_EDITOR
        var boneRenderer = transform.AddComponent<BoneRenderer>();
#endif
        var avatarController = transform.AddComponent<AvatarController>();

        // Fill the Rig Builder component
        var rigLayer = new List<RigLayer>();
        rigLayer.Add(new RigLayer(_VRRigForAvatar.GetComponent<Rig>()));
        rigBuilder.layers = rigLayer;

        foreach (Transform bone in transform.GetComponentsInChildren<Transform>())
        {
#if UNITY_EDITOR
            if (bone.name == "Armature")
            {
                var transformsBody = new HashSet<Transform>(bone.GetComponentsInChildren<Transform>());
                transformsBody.Remove(bone);
                boneRenderer.transforms = transformsBody.ToArray();
            }
#endif
            if (bone.name == "Head")
                avatarHead = bone;
            if (bone.name == "LeftArm")
                avatarLeftArm = bone;
            if (bone.name == "RightArm")
                avatarRightArm = bone;
        }

        // Connect to arms and head here
        avatarController.Head = new MapTransform()
        {
            vrTarget = mainCamera,
            ikTarget = rigHead,
            trackingPositionOffset = new Vector3(0, -0.1f, -0.15f),
            trackingRotationOffset = new Vector3(0, 0, 0)
        };

        avatarController.LeftHand = new MapTransform
        {
            vrTarget = leftHand,
            ikTarget = rigLeftHandTarget,
            trackingPositionOffset = new Vector3(-0.05f, -0.03f, -0.12f),
            trackingRotationOffset = new Vector3(0, 90f, 110f)
        };

        avatarController.RightHand = new MapTransform
        {
            vrTarget = rightHand,
            ikTarget = rigRightHandTarget,
            trackingPositionOffset = new Vector3(0.05f, -0.03f, -0.12f),
            trackingRotationOffset = new Vector3(0, -90f, -110f)
        };

        avatarController.IkHead = rigHead;
        avatarController.XROrigin = _XROrigin;

        // Setting Rig with Avatar armature
        var headBoneConstraints = rigHead.GetComponent<MultiParentConstraint>();
        headBoneConstraints.data.constrainedObject = avatarHead;

        var leftBoneConstraints = rigLeftHandTarget.parent.GetComponent<TwoBoneIKConstraint>();
        leftBoneConstraints.data.root = avatarLeftArm;
        leftBoneConstraints.data.mid = avatarLeftArm.GetChild(0);
        leftBoneConstraints.data.tip = avatarLeftArm.GetChild(0).GetChild(0);

        var rightBoneConstraints = rigRightHandTarget.parent.GetComponent<TwoBoneIKConstraint>();
        rightBoneConstraints.data.root = avatarRightArm;
        rightBoneConstraints.data.mid = avatarRightArm.GetChild(0);
        rightBoneConstraints.data.tip = avatarRightArm.GetChild(0).GetChild(0);

        // Fix: restart RigBuilder component to activate hands 
        rigBuilder.enabled = false;
        rigBuilder.enabled = true;

        // Fix: pixel hands and body blinking
        var avatarMaterial = transform.GetComponentInChildren<Renderer>().material;
        avatarMaterial.SetFloat("_BumpScale", 0.1f);
        avatarMaterial.SetFloat("_Smoothness", 0.1f);

        var teleportationProvider = GameObject.FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.TeleportationProvider>();
        teleportationProvider.system = _XROrigin.GetComponent<UnityEngine.XR.Interaction.Toolkit.LocomotionSystem>();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GhostHands : MonoBehaviour
{
    public GameObject ghostHand;
    public Renderer ghostHandRenderer;
    public XRDirectInteractor ghostHandDirectInteractor;
    public GrabHandPose ghostHandPose;

    public XRDirectInteractor avatarHandDirectInteractor;
    public GameObject avatarHand;
    public GameObject avatar;

    public bool IsGhostHands { get; private set; }

    void Update()
    {
        // Show ghost hands
        if (Vector3.Distance(avatarHand.transform.position, ghostHand.transform.position) > 0.15f)
        {
            if (!IsGhostHands)
            {
                if (avatarHandDirectInteractor.hasSelection)
                {
                    IXRSelectInteractable obj = avatarHandDirectInteractor.interactablesSelected[0];
                    avatarHandDirectInteractor.interactionManager.SelectExit(avatarHandDirectInteractor, obj);
                    ghostHandDirectInteractor.interactionManager.SelectEnter(ghostHandDirectInteractor, obj);

                    ghostHandPose.SetupPose(ghostHandDirectInteractor);
                }

                avatarHandDirectInteractor.enabled = false;
                ghostHandDirectInteractor.enabled = true;
                ghostHandRenderer.enabled = true;

                avatar.SetLayerRecursively(14);
                ghostHand.SetLayerRecursively(7);

                IsGhostHands = true;
            }
        }
        // Hide ghost hands
        else if (IsGhostHands)
        {
            if (ghostHandDirectInteractor.hasSelection)
            {
                IXRSelectInteractable obj = ghostHandDirectInteractor.interactablesSelected[0];
                ghostHandDirectInteractor.interactionManager.SelectExit(ghostHandDirectInteractor, obj);
                avatarHandDirectInteractor.interactionManager.SelectEnter(avatarHandDirectInteractor, obj);

                ghostHandPose.UnsetPose(ghostHandDirectInteractor);
            }

            ghostHandDirectInteractor.enabled = false;
            avatarHandDirectInteractor.enabled = true;
            ghostHandRenderer.enabled = false;

            avatar.SetLayerRecursively(7);
            ghostHand.SetLayerRecursively(14);

            IsGhostHands = false;
        }

    }
}

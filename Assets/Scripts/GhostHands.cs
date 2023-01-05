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

    public XRDirectInteractor avatarHandDirectInteractor;
    public GameObject avatarHand;
    public GameObject avatar;

    private bool _returnAvatarHand = true;

    void Update()
    {
        if (Vector3.Distance(avatarHand.transform.position, ghostHand.transform.position) > 0.2f)
        {
            if (avatarHandDirectInteractor.hasSelection)
            {
                IXRSelectInteractable obj = avatarHandDirectInteractor.interactablesSelected[0];
                avatarHandDirectInteractor.interactionManager.SelectExit(avatarHandDirectInteractor, obj);
                ghostHandDirectInteractor.interactionManager.SelectEnter(ghostHandDirectInteractor, obj);
            }

            avatarHandDirectInteractor.enabled = false;
            ghostHandDirectInteractor.enabled = true;
            ghostHandRenderer.enabled = true;

            avatar.SetLayerRecursively(14);
            ghostHand.SetLayerRecursively(7);

            _returnAvatarHand = false;
        }
        else if (!_returnAvatarHand)
        {
            if (ghostHandDirectInteractor.hasSelection)
            {
                IXRSelectInteractable obj = ghostHandDirectInteractor.interactablesSelected[0];
                ghostHandDirectInteractor.interactionManager.SelectExit(ghostHandDirectInteractor, obj);
                avatarHandDirectInteractor.interactionManager.SelectEnter(avatarHandDirectInteractor, obj);
            }

            ghostHandDirectInteractor.enabled = false;
            avatarHandDirectInteractor.enabled = true;
            ghostHandRenderer.enabled = false;

            avatar.SetLayerRecursively(7);
            ghostHand.SetLayerRecursively(14);

            _returnAvatarHand = true;
        }

    }

    private void ProcessInteraction()
    {

    }
}

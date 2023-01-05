using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSitting : MonoBehaviour
{
    // h=1.2 - 0.76
    // -0.439
    // -0.9139999  -1.43
    [SerializeField] private GameObject _additionalOffset;
    [SerializeField] private GameObject _avatar;

    void Update()
    {
        if (transform.localPosition.y > 0.56f && transform.localPosition.y < 1.2f)
        {
            var newPosY = -0.91f * 1.2f / transform.localPosition.y;
            _avatar.transform.localPosition = new Vector3(_avatar.transform.localPosition.x, newPosY, _avatar.transform.localPosition.z);

            float k = -0.78125f * transform.localPosition.y + 1.9375f;
            newPosY = -0.439f * 1.2f / transform.localPosition.y;
            _additionalOffset.transform.localPosition = new Vector3(_additionalOffset.transform.localPosition.x, newPosY * k, _additionalOffset.transform.localPosition.z);
        }
    }
}

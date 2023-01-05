using UnityEngine;

public class AvatarFootController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField][Range(0, 1)] private float leftFootPosWeight;
    [SerializeField][Range(0, 1)] private float rightFootPosWeight;

    [SerializeField][Range(0, 1)] private float leftFootRotWeight;
    [SerializeField][Range(0, 1)] private float rightFootRotWeight;

    [SerializeField] private Vector3 footOffset;
    [SerializeField] private Vector3 raycastOffsetLeft;
    [SerializeField] private Vector3 raycastOffsRight;

    RaycastHit hitLeftFoot;
    RaycastHit hitRightFoot;

    private float _leftKneePositionWeight;
    private Vector3 _leftKneePosition;
    private float _rightKneePositionWeight;
    private Vector3 _rightKneePosition;

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 leftFootPos = this.animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPos = this.animator.GetIKPosition(AvatarIKGoal.RightFoot);

        print("foot: " + rightFootPos);

        bool isLeftFootDown;
        bool isRightFootDown;

        if (leftFootPos.y > 0)
            isLeftFootDown = Physics.Raycast(leftFootPos + this.raycastOffsetLeft, Vector3.down, out hitLeftFoot);
        else
            isLeftFootDown = true;

        if (rightFootPos.y > 0)
            isRightFootDown = Physics.Raycast(rightFootPos + this.raycastOffsRight, Vector3.down, out hitRightFoot);
        else
            isRightFootDown = true;

        if (isLeftFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, this.leftFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitLeftFoot.point + this.footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitLeftFoot.normal), hitLeftFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, this.leftFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }

        if (isRightFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, this.rightFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.RightFoot, hitRightFoot.point + this.footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitRightFoot.normal), hitRightFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, this.rightFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }

        // Knee
        print("left knee: " + this.animator.GetIKHintPosition(AvatarIKHint.LeftKnee));

        Vector3 leftKneePos = this.animator.GetIKHintPosition(AvatarIKHint.LeftKnee);
        Vector3 rightKneePos = this.animator.GetIKHintPosition(AvatarIKHint.RightKnee);

        if (leftKneePos.y <= 0)
        {
            print("setting knee: " + _leftKneePosition);
            this.animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
            this.animator.SetIKHintPosition(AvatarIKHint.LeftKnee, _leftKneePosition);
        }
        else
        {
            _leftKneePositionWeight = this.animator.GetIKHintPositionWeight(AvatarIKHint.LeftKnee);
            _leftKneePosition = leftKneePos;
        }

        if (rightKneePos.y <= 0)
        {
            this.animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
            this.animator.SetIKHintPosition(AvatarIKHint.RightKnee, _rightKneePosition);
        }
        else
        {
            _rightKneePositionWeight = this.animator.GetIKHintPositionWeight(AvatarIKHint.RightKnee);
            _rightKneePosition = rightKneePos;
        }
    }
}
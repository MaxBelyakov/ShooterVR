using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.Events;

public class BowStringMeasurer : XRBaseInteractable
{
    public class PullEvent : UnityEvent<Vector3, float> { }
    public PullEvent Pulled = new PullEvent();

    public Transform stringStart;
    public Transform stringEnd;

    public float stringAmount = 0.0f;

    private IXRSelectInteractor stringInteractor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        print("select");
        base.OnSelectEntered(args);

        // Set interactor for measuring
        stringInteractor = args.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        print("exit");
        base.OnSelectExited(args);

        // Clear interactor and reset string amount for animation
        stringInteractor = null;

        // Reset everything
        SetStringValues(stringStart.position, 0.0f);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected)
        {
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                Vector3 interactorPosition = stringInteractor.transform.position;

                float newStringAmount = CalculateString(interactorPosition);
                Vector3 newStringPosition = Vector3.Lerp(stringStart.position, stringEnd.position, newStringAmount);

                SetStringValues(newStringPosition, newStringAmount);
            }
        }
    }

    private float CalculateString(Vector3 stringPosition)
    {
        // Direction and length
        Vector3 stringDirection = stringPosition - stringStart.position;
        Vector3 targetDirection = stringEnd.position - stringStart.position;

        float maxLength = targetDirection.magnitude;
        targetDirection.Normalize();

        // Calculate actual distance
        float stringValue = Vector3.Dot(stringDirection, targetDirection) / maxLength;
        stringValue = Mathf.Clamp(stringValue, 0.0f, 1.0f);

        return stringValue;
    }

    private void SetStringValues(Vector3 newStringPosition, float newStringAmount)
    {
        // If it's a new value
        if (newStringAmount != stringAmount)
        {
            stringAmount = newStringAmount;
            Pulled?.Invoke(newStringPosition, newStringAmount);
        }
    }
}
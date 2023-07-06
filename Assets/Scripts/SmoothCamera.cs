
using UnityEngine;



[RequireComponent(typeof(Camera))]
public class SmoothCamera : CameraAbstract {

    [Header("Camera Roll Damp")]
    [Range(0, 1)]
    public float rollDamp;

    private Vector3 lastObservedObjectNormal;

    protected override void Awake() {
        base.Awake();
        lastObservedObjectNormal = observedObject.up;
        camTarget.parent = observedObject;
    }

    protected override void Update() {
        base.Update();
        if (rollDamp != 0) {
            float angle = Vector3.SignedAngle(lastObservedObjectNormal, observedObject.up, camTarget.right);
            RotateCameraVertical(rollDamp * -angle);
            lastObservedObjectNormal = observedObject.up;
        }
    }

    protected override Vector3 getHorizontalRotationAxis() {
        return observedObject.transform.up;
    }
    protected override Vector3 getVerticalRotationAxis() {
        return camTarget.right;
    }
}

using UnityEngine;

/// <summary>
/// Script that forces whatever canvas/UI this is attached to,
/// to look at the player at all times.
/// </summary>
public class FaceTowardsPlayer : MonoBehaviour {
    private Camera mainCam;

    private void Awake() {
        mainCam = Camera.main;
    }

    private void LateUpdate() {
        if (mainCam == null) {
            return;
        }

        Vector3 forward = mainCam.transform.forward;
        forward.y = 0f;

        transform.forward = forward;
    }

}

using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    /// <summary>
    /// Makes sure the callback is only triggered once.
    /// </summary>
    private bool isFirstUpdate = true;

    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }

}

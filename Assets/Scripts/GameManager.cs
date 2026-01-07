using UnityEngine;

/// <summary>
/// Attached to parent of all game controllers/managers.
/// Allows parent/child to not be destroyed when changing scenes.
/// </summary>
public class GameManager : MonoBehaviour {
    public static GameManager instance {get; private set;}

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

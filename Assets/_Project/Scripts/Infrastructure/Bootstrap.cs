using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private const string MainScemeName = "Main";

    private void Start() =>
        SceneManager.LoadScene(MainScemeName);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene() {
        yield return new WaitForSecondsRealtime(3.0f);
        USENSceneManager.Instance.LoadScene("GameEntries");
    }
}

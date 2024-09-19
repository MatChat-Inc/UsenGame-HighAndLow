using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class USENSceneManager : MonoBehaviourSingletonTemplate<USENSceneManager>
{
    public string m_sceneName;
    void OnEnable()
    {
    	// 添加场景加载委托
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name)); 
        Resources.UnloadUnusedAssets();
    }

    void OnSceneUnloaded(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
        Resources.UnloadUnusedAssets();
    }

    void OnDisable()
    {
    	// 移除场景委托
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}



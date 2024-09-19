using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IResource
{
    public void Retain();
    public void Release();
    public void Unload();
}

public class Resource<T> : IResource where T : UnityEngine.Object
{
    private T m_asset;
    public Resource(T asset)
    {
        m_asset = asset;
        m_retainCount = 0;
    }

    public T GetAsset()
    {
        Retain();
        return m_asset;
    }

    private int m_retainCount;
    public void Retain()
    {
        m_retainCount++;
    }

    public void Release()
    {
        m_retainCount--;
        if (m_retainCount == 0)
        {
            Unload();
        }
    }

    public void Unload()
    {
        Resources.UnloadAsset(m_asset);
    }
}

public class AbstractView
{
    protected GameObject m_mainViewGameObject;
    protected List<GameObject> m_viewGameObjects;
    private List<IResource> m_resources = new();
    private List<GameObject> m_gameObjects = new();
    
    public GameObject MainGameObject => m_mainViewGameObject;

    protected GameObject LoadViewGameObject(string assetPath, Transform parent)
    {
        var obj = Resources.Load<GameObject>(assetPath);
        var go = GameObject.Instantiate(obj, parent, false);
        m_gameObjects.Add(go);
        return go;
    }

    protected T Load<T>(string assetPath) where T : UnityEngine.Object
    {
        var obj = Resources.Load<T>(assetPath);
        var res = new Resource<T>(obj);
        m_resources.Add(res);
        return res.GetAsset();
    }

    protected async Task<GameObject> LoadViewGameObjectAsync(string assetPath, Transform parent)
    {
        var obj = await ResourceLoader.LoadResourcesAsync<GameObject>(assetPath);
        var go = GameObject.Instantiate(obj, parent, false);
        m_gameObjects.Add(go);
        return go;
    }

    protected async Task<T> LoadAsync<T>(string assetPath) where T : UnityEngine.Object
    {
        var obj = await ResourceLoader.LoadResourcesAsync<T>(assetPath);
        var res = new Resource<T>(obj);
        m_resources.Add(res);
        return res.GetAsset();
    }

    public void Destroy()
    {
        OnDestroy();
        // foreach (var res in m_resources) {
        //     res.Release();
        // }
        // m_resources.Clear();

        foreach (var go in m_gameObjects)
        {
            UnityEngine.Object.DestroyImmediate(go, true);
        }
        m_gameObjects.Clear();

        // Resources.UnloadUnusedAssets();
        // GC.Collect();
    }

    public virtual void OnDestroy() {}
}
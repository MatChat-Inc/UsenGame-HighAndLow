using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System;
 
 
public static class ResourceLoader
{
    public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest request) => new ResourceRequestAwaiter(request);
 
    public static async Task<T> LoadResourcesAsync<T>(string path) where T : UnityEngine.Object
    {
        var gres = Resources.LoadAsync<T>(path);
        await gres;
        return gres.asset as T;
    }
}
 
public class ResourceRequestAwaiter : INotifyCompletion
{
    public Action Continuation;
    public ResourceRequest resourceRequest;
    public bool IsCompleted => resourceRequest.isDone;
    public ResourceRequestAwaiter(ResourceRequest resourceRequest)
    {
        this.resourceRequest = resourceRequest;
 
        this.resourceRequest.completed += Accomplish;
    }
 
    public void OnCompleted(Action continuation) => this.Continuation = continuation;
 
    public void Accomplish(AsyncOperation asyncOperation) => Continuation?.Invoke();
 
    public void GetResult() {}
}
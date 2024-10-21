// Created by LunarEclipse on 2024-8-3 4:19.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Luna.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace USEN
{
    public class AssetUtils
    {
        private static readonly Dictionary<Type, AsyncOperationHandle<GameObject>> _typeHandlers = new ();
        private static readonly Dictionary<string, AsyncOperationHandle<IList<GameObject>>> _labelHandlers = new ();
        
        public static GameObject Load<T>() where T : Widget
        {
            if (_typeHandlers.TryGetValue(typeof(T), out var cachedHandler))
                return cachedHandler.WaitForCompletion();
            
            var key = GetAddressableKey<T>();
            var handler = Addressables.LoadAssetAsync<GameObject>(key);
            var prefab = handler.WaitForCompletion();
            _typeHandlers.Add(typeof(T), handler);
            return prefab;
        }
        
        public static Task<GameObject> LoadAsync<T>() where T : Widget
        {
            if (_typeHandlers.TryGetValue(typeof(T), out var cachedHandler))
                return cachedHandler.Task;
            
            var key = GetAddressableKey<T>();
            var handler = Addressables.LoadAssetAsync<GameObject>(key);
            _typeHandlers.Add(typeof(T), handler);
            return handler.Task;
        }
        
        public static Task<IList<GameObject>> Load(string label)
        {
            if (_labelHandlers.TryGetValue(label, out var cachedHandler))
                return cachedHandler.Task;
            
            var handler = Addressables.LoadAssetsAsync<GameObject>(label, null);
            handler.Completed += op =>
            {
                foreach (var obj in op.Result)
                    Debug.Log($"[Widget] Loaded asset: {obj.name}");
            };
            _labelHandlers.Add(label, handler);
            return handler.Task;
        }

        public static Task<IList<GameObject>> Load(params string[] labels)
        {
            var key = string.Join(",", labels);
            if (_labelHandlers.TryGetValue(key, out var cachedHandler))
                return cachedHandler.Task;
            
            var handler = Addressables.LoadAssetsAsync<GameObject>(new List<string>(labels), null, Addressables.MergeMode.Intersection);
            var locations = Addressables.LoadResourceLocationsAsync(labels);
            handler.Completed += op =>
            {
                foreach (var obj in op.Result)
                    Debug.Log($"[Widget] Loaded asset: {obj.name}");
            };
            _labelHandlers.Add(key, handler);
            return handler.Task;
        }

        public static void Unload<T>() where T : Widget
        {
            var key = typeof(T);
            if (_typeHandlers.ContainsKey(key))
            {
                Addressables.Release(_typeHandlers[key]);
                _typeHandlers.Remove(key);
            }
            else Debug.LogWarning($"[Widget] Key not found: {key}");
        }
        
        public static void Unload(string label)
        {
            if (_labelHandlers.ContainsKey(label))
            {
                Addressables.Release(_labelHandlers[label]);
                _labelHandlers.Remove(label);
            }
            else Debug.LogWarning($"[Widget] Label not found: {label}");
        }
        
        public static void Unload(params string[] labels)
        {
            var key = string.Join(",", labels);
            if (_labelHandlers.ContainsKey(key))
            {
                Addressables.Release(_labelHandlers[key]);
                _labelHandlers.Remove(key);
            }
            else Debug.LogWarning($"[Widget] Label not found: {key}");
        }
        
        public static string GetAddressableKey<T>() where T : Widget
        {
            return (typeof(T).Namespace ?? "global") + "." + typeof(T).Name;
        }
    }
}
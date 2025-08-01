// Created by LunarEclipse on 2024-09-26 19:09.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Luna;
using Newtonsoft.Json;
using UnityEngine;
using USEN.Games.Roulette;

namespace USEN
{
    public class Request
    {
        public static HttpClient Client { get; }

        static Request()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            var ssid = USEN.AndroidPreferences.Ssid ?? "01HA1S5FCXKDB78KGBZ0QP3HYQ";
            var tvIdentifier = USEN.AndroidPreferences.TVIdentifier ?? "N00000000000000065760";
#else
            var ssid = "01HA1S5FCXKDB78KGBZ0QP3HYQ";
            var tvIdentifier = "N00000000000000065760";
#endif
            
            Debug.Log($"Request with ssid={ssid}, tvIdentifier={tvIdentifier}");
            
            Client = new()
            {
#if DEBUG
                BaseAddress = new Uri("https://api-stg.tvsignage.usen.com/"),
#else
                BaseAddress = new Uri("https://api.tvsignage.usen.com/"),
#endif
                DefaultRequestHeaders = {
                    Accept = { new("application/json") },
                    Authorization = new("750900428"),
                },
            };
            
            Client.DefaultRequestHeaders.Add("x-umid", ssid);
            Client.DefaultRequestHeaders.Add("neosContractCd", tvIdentifier);
        }
        
        public static Task<T> Get<T>(string path) where T : class
        {
            var response = Client.GetAsync(path);
            return response.ContinueWith(task =>
            {
                var result = task.Result;
                var content = result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                    return content.ContinueWith(task => {
                        Debug.Log($"GET {path} result: {task.Result}");
                        return JsonConvert.DeserializeObject<T>(task.Result);
                    }).Result;
                return null;
            });
        }
        
        public static Task<Response> Post(string path, object data)
        {
            return Post<Response>(path, data);
        }
        
        public static Task<T> Post<T>(string path, object data)
        {
            var json = JsonConvert.SerializeObject(data, new IgnoreJsonProperties("id"));
            var content = new StringContent(json);
            Debug.Log($"POST {path} with json: {json}");
            content.Headers.ContentType = new("application/json");
            
            var postTask = Client.PostAsync(path, content);
            return postTask.ContinueWith(task => {
                var content = task.Result.Content.ReadAsStringAsync();
                return content.ContinueWith(task => {
                    Debug.Log($"POST {path} result: {task.Result}");
                    return JsonConvert.DeserializeObject<T>(task.Result);
                }).Result;
            });
        }
        
        public static Task<Response> Put(string path, object data)
        {
            return Put<Response>(path, data);
        }
        
        public static Task<T> Put<T>(string path, object data)
        {
            var json = JsonConvert.SerializeObject(data, new IgnoreJsonProperties("id"));
            var content = new StringContent(json);
            Debug.Log($"PUT {path} with json: {json}");
            content.Headers.ContentType = new("application/json");
            
            var response = Client.PutAsync(path, content);
            return response.ContinueWith(task => {
                var content = task.Result.Content.ReadAsStringAsync();
                return content.ContinueWith(task =>
                {
                    Debug.Log($"PUT {path} result: {task.Result}");
                    return JsonConvert.DeserializeObject<T>(task.Result);
                }).Result;
            });
        }
        
        public static Task<Response> Delete(string path)
        {
            return Delete<Response>(path);
        }
        
        public static Task<T> Delete<T>(string path)
        {
            var response = Client.DeleteAsync(path);
            return response.ContinueWith(task => {
                var content = task.Result.Content.ReadAsStringAsync();
                return content.ContinueWith(task => JsonConvert.DeserializeObject<T>(task.Result)).Result;
            });
        }
    }
}
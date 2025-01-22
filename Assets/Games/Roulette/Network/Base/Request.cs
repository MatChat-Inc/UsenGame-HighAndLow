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
            Client = new()
            {
                BaseAddress = new Uri("https://api-stg.tvsignage.usen.com/"),
                // BaseAddress = new Uri("https://api.tvsignage.usen.com/"),
                DefaultRequestHeaders = {
                    Accept = { new("application/json") },
                    Authorization = new("750900428"),
                },
            };
            
#if !DEBUG && UNITY_ANDROID
                var tvIdentifier = USEN.AndroidPreferences.TVIdentifier ?? "N00000000000000065760";
#else
            var tvIdentifier = "N00000000000000065760";
#endif
            
            Client.DefaultRequestHeaders.Add("x-umid", "01HA1S5FCXKDB78KGBZ0QP3HYQ");
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
                    return content.ContinueWith(task => JsonConvert.DeserializeObject<T>(task.Result)).Result;
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
            content.Headers.ContentType = new("application/json");
            
            var postTask = Client.PostAsync(path, content);
            return postTask.ContinueWith(task => {
                var content = task.Result.Content.ReadAsStringAsync();
                return content.ContinueWith(task => {
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
            content.Headers.ContentType = new("application/json");
            
            var response = Client.PutAsync(path, content);
            return response.ContinueWith(task => {
                var content = task.Result.Content.ReadAsStringAsync();
                return content.ContinueWith(task => JsonConvert.DeserializeObject<T>(task.Result)).Result;
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
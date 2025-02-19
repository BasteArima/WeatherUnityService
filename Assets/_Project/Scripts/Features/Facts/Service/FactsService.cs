using System;
using _Project.Scripts.Features.Facts.Domain;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

namespace _Project.Scripts.Features.Facts.Service
{
    public class FactsService : IFactsService
    {
        private const string FACTS_URL = "https://dogapi.dog/api/v2/breeds";

        public async UniTask<FactInfo[]> GetFactsAsync()
        {
            using (var request = UnityWebRequest.Get(FACTS_URL))
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await UniTask.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        string json = request.downloadHandler.text;
                        
                        JObject root = JObject.Parse(json);

                        JArray data = root["data"] as JArray;
                        if (data == null)
                            throw new Exception("Field 'data' not found or not an array in the breeds response.");

                        int count = Math.Min(10, data.Count);
                        FactInfo[] facts = new FactInfo[count];

                        for (int i = 0; i < count; i++)
                        {
                            JObject item = data[i] as JObject;
                            if (item == null) continue;

                            string id = item["id"]?.ToString();

                            JObject attributes = item["attributes"] as JObject;
                            string name = attributes?["name"]?.ToString();

                            facts[i] = new FactInfo
                            {
                                Id = id,
                                Name = name
                            };
                        }

                        return facts;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"JSON parse error (facts): {e.Message}");
                        throw;
                    }
                }
                else
                {
                    throw new Exception($"Facts request error: {request.error}");
                }
            }
        }

        public async UniTask<FactDetail> GetFactDetailAsync(string factId)
        {
            string url = $"{FACTS_URL}/{factId}";
            using (var request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await UniTask.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        string json = request.downloadHandler.text;
                        JObject root = JObject.Parse(json);
                        
                        JObject data = root["data"] as JObject;
                        if (data == null)
                            throw new Exception("Field 'data' not found or not an object in breed detail response.");

                        JObject attributes = data["attributes"] as JObject;
                        if (attributes == null)
                            throw new Exception("Field 'attributes' not found in breed detail response.");

                        string name = attributes["name"]?.ToString();
                        string description = attributes["description"]?.ToString();

                        return new FactDetail
                        {
                            Name = name,
                            Description = description
                        };
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Fact detail parse error: {e.Message}");
                        throw;
                    }
                }
                else
                {
                    throw new Exception($"Fact detail request error: {request.error}");
                }
            }
        }
    }
}

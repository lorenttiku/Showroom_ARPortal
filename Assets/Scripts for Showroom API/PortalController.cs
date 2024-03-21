using Assets.Course.Core.RestHttp;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Assets.Course.Models;
using System.Collections.Generic;
using Assets.Course.Core;
using System.Collections;
using Unity.VisualScripting;

namespace Assets.Course
{
    public class MergedImageTargets : MonoBehaviour
    {
        public string baseUrl = "http://localhost:3000/portalusers";

        private string currentDataHash;
        public Material[] cubeMaterials;
      

        void Start()
        {
            RequestHeader reqHeader = new RequestHeader
            {
                Key = "Content-Type",
                Value = "application/json"
            };

            // Initial data load
            StartCoroutine(RestApiClient.Instance.HttpGet(baseUrl, (r) => OnRequestCompleted(r)));

            InvokeRepeating("AutoRefreshData", 1f, 2f);
        }

        void OnRequestCompleted(Response response)
        {
            Debug.Log("Succesful HTTP Request: " + response.StatusCode);
            Debug.Log("Data: " + response.Data);
            Debug.Log("Error: " + response.Error);

            AllImageTargets allImageTargets = JsonUtility.FromJson<AllImageTargets>(response.Data);

            if (currentDataHash != GetHash(response.Data))
            {
                currentDataHash = GetHash(response.Data);
                RefreshUI(allImageTargets);
            }
        }

        void RefreshUI(AllImageTargets allImageTargets)
        {
            int count = Mathf.Min(allImageTargets.data.Length, cubeMaterials.Length);

            for (int i = 0; i < count; i++)
            {

          

                int index = i; 

                StartCoroutine(GetTexture(allImageTargets.data[i].PictureLink, tex =>
                {
                   
                    cubeMaterials[index].SetTexture("_MainTex", tex);
                }));
            }
        }
        
        IEnumerator GetTexture(string url, System.Action<Texture> callback)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
              Texture tex = DownloadHandlerTexture.GetContent(www);
                callback(tex);
            }
        }

        private void AutoRefreshData()
        {
            StartCoroutine(RestApiClient.Instance.HttpGet(baseUrl, (r) => OnRequestCompleted(r)));
        }

        private string GetHash(string input)
        {
            if (input == null)
            {
                Debug.LogWarning("Input string is null. Unable to generate hash.");
                return null;
            }
            else
            {
                return input.GetHashCode().ToString();
            }
        }
    }
}

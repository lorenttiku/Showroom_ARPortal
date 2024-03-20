using UnityEngine;
using UnityEngine.Networking;
using Assets.Course.Models;
using System.Collections;
using Assets.Course.Core;
using Assets.Course.Core.RestHttp;

namespace Assets.Course
{
    public class PortalController : MonoBehaviour
    {
        public string baseUrl = "http://localhost:3000/portalusers";
        public Renderer[] cubeRenderers;

        void Start()
        {
            StartCoroutine(RestApiClient.Instance.HttpGet(baseUrl, OnRequestCompleted));
            InvokeRepeating("AutoRefreshData", 1f, 2f);
        }

        private void OnRequestCompleted(Response response)
        {
            if (response.Error != null)
            {
                Debug.LogError("Error in HTTP Request: " + response.Error);
                return;
            }

            Debug.Log("Successful HTTP Request: " + response.StatusCode);
            Debug.Log("Data: " + response.Data);

            AllImageTargets allImageTargets = JsonUtility.FromJson<AllImageTargets>(response.Data);
            RefreshUI(allImageTargets);
        }

        void RefreshUI(AllImageTargets allImageTargets)
        {
            ClearImages();

            // Determine the maximum number of iterations based on the smaller array length
            int maxIterations = Mathf.Min(allImageTargets.data.Length, cubeRenderers.Length);

            for (int i = 0; i < maxIterations; i++)
            {
                ImageTarget targetData = allImageTargets.data[i];
                StartCoroutine(GetTexture(targetData.PictureLink, tex =>
                {
                    // Assign the texture to the cube renderer's material
                    cubeRenderers[i].material.mainTexture = tex;

                    // Enable the cube renderer to make it visible
                    cubeRenderers[i].enabled = true;
                }));

                // Disable the cube renderer initially to prevent it from being rendered without a texture
                cubeRenderers[i].enabled = false;
            }
        }

        void ClearImages()
        {
            foreach (var renderer in cubeRenderers)
            {
                renderer.material.mainTexture = null;
            }
        }

        IEnumerator GetTexture(string url, System.Action<Texture> callback)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading texture from URL: " + url);
                Debug.LogError(www.error);
            }
            else
            {
                Texture tex = DownloadHandlerTexture.GetContent(www);
                Debug.Log("Texture loaded successfully from URL: " + url);
                callback(tex);
            }
        }

        private void AutoRefreshData()
        {
            StartCoroutine(RestApiClient.Instance.HttpGet(baseUrl, OnRequestCompleted));
        }
    }
}

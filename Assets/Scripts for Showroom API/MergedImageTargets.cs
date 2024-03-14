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

        private GameObject imgTargetCard;
        private string currentDataHash;
        private List<ImageTargetButton> imageTargetButtons = new List<ImageTargetButton>();
        public GameObject detailsPanel;
        public TextMeshProUGUI detailsDescription;
        public TextMeshProUGUI detailsAutorName;
        public RawImage detailsImage;
        public GameObject imgTargetCardPrefab;
        public Transform PicturePosition;


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

            if (response.Data != null && currentDataHash != GetHash(response.Data))
            {
                currentDataHash = GetHash(response.Data);
                AllImageTargets allImageTargets = JsonUtility.FromJson<AllImageTargets>(response.Data);
                RefreshUI(allImageTargets);
            }
        }


        void RefreshUI(AllImageTargets allImageTargets)
        {
            ClearUI();
            imageTargetButtons.Clear();

            int count = allImageTargets.data.Length;
            int index = 0;

            for (int i = 0; i < count; i++)
            {
                imgTargetCard = Instantiate(imgTargetCardPrefab) as GameObject;
                imgTargetCard.SetActive(true);

                ImageTargetButton imgTarget = imgTargetCard.GetComponent<ImageTargetButton>();
                imageTargetButtons.Add(imgTarget);

                imgTarget.id.text = allImageTargets.data[i].id.ToString();
                imgTarget.Description.text = allImageTargets.data[i].Description;

                StartCoroutine(GetTexture(allImageTargets.data[i].PictureLink, tex =>
                {
                    imgTarget.PictureLink.texture = tex;
                }));

                // Find detailsAutorName within imgTargetCard
                TextMeshProUGUI detailsAutorName = imgTargetCard.GetComponentInChildren<TextMeshProUGUI>();
                if (detailsAutorName != null)
                {
                    detailsAutorName.text = allImageTargets.data[i].AutorName;
                }
                else
                {
                    Debug.LogError("detailsAutorName not found in imgTargetCard hierarchy.");
                }

                imgTargetCard.transform.SetParent(PicturePosition, false);

                int newIndex = index;
                imgTargetCard.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                    () =>
                    {
                        imageTargetDetails(newIndex, allImageTargets);
                        detailsPanel.SetActive(true);
                    });
                index++;
            }
        }


        private void imageTargetDetails(int index, AllImageTargets imgTargetObject)
        {
            detailsDescription.text = imgTargetObject.data[index].Description;

            StartCoroutine(GetTexture(imgTargetObject.data[index].PictureLink, tex =>
            {
                detailsImage.texture = tex;
            }));

            detailsAutorName.text = imgTargetObject.data[index].AutorName;
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

        void ClearUI()
        {
            foreach (Transform child in PicturePosition)
            {
                Destroy(child.gameObject);
            }
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

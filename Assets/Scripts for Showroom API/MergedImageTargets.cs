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
      
        public TextMeshProUGUI detailsDescription;
        public TextMeshProUGUI detailsAutorName;
        public RawImage detailsImage;
        public GameObject imgTargetCardPrefab;
        public Transform[] instantiatePositions;
        [SerializeField] private Portal portal;
        List<GameObject> _images = new List<GameObject>();


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
        private void OnEnable()
        {
            Portal.OnGetInPortal += Portal_OnGetInPortal;
          
        }
        private void OnDisable()
        {
            Portal.OnGetInPortal -= Portal_OnGetInPortal;
        }
        private void Portal_OnGetInPortal(bool obj)
        {
            if (obj)
            {
                TurnOn(_images);
            }
            else
            {
                TurnOff(_images);
            }
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

        void TurnOn(List<GameObject> imagePrefab)
        {
            foreach (var image in imagePrefab)
            {
                image.SetActive(true);
            }
         
        }
        void TurnOff(List<GameObject> imagePrefab)
        {
            foreach (var image in imagePrefab)
            {
                image.SetActive(false);
            }
  

        }
        void RefreshUI(AllImageTargets allImageTargets)
        {
          
            //imageTargetButtons.Clear();

            int count = allImageTargets.data.Length;
            int positionIndex = 0; // Initialize position index outside the loop

            for (int i = 0; i < count; i++)
            {
                // Instantiate the prefab at different positions
                 imgTargetCard = Instantiate(imgTargetCardPrefab, instantiatePositions[positionIndex].position, Quaternion.identity) as GameObject;

                imgTargetCard.SetActive(false);
                _images.Add(imgTargetCard);

                // Populate data for the instantiated prefab
                ImageTargetButton imgTarget = imgTargetCard.GetComponent<ImageTargetButton>();
                imageTargetButtons.Add(imgTarget);
                imgTarget.id.text = allImageTargets.data[i].id.ToString();
                imgTarget.Description.text = allImageTargets.data[i].Description;

                StartCoroutine(GetTexture(allImageTargets.data[i].PictureLink, tex =>
                {
                    imgTarget.PictureLink.texture = tex;
                }));

                // Find detailsAutorName within instantiated prefab
                TextMeshProUGUI detailsAutorName = imgTargetCard.GetComponentInChildren<TextMeshProUGUI>();
                if (detailsAutorName != null)
                {
                    detailsAutorName.text = allImageTargets.data[i].AutorName;
                }
                else
                {
                    Debug.LogError("detailsAutorName not found in imgTargetCard hierarchy.");
                }

                imgTargetCard.transform.SetParent(instantiatePositions[positionIndex], false);

                int newIndex = positionIndex;
                //imgTargetCard.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(
                //    () =>
                //    {
                //        imageTargetDetails(newIndex, allImageTargets);
                      
                //    });

                // Increment position index
                positionIndex = (positionIndex + 1) % instantiatePositions.Length;
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

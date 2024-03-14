using Assets.Course.Core;
using Assets.Course.Core.RestHttp;
using Assets.Course.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Course
{
    public class  PostImageTarget : MonoBehaviour
    {


        private string baseUrl = "http://localhost:3000/portalusers";
        private void Start()
        {
            RequestHeader header = new RequestHeader
            {
                Key = "Content-Type",
                Value = "application/json"
            };
            ImageTarget newImageTarget = new ImageTarget();
            newImageTarget.AutorName = "Ford7";
            newImageTarget.Description = "A amazing car that will never stop!!";
          
            newImageTarget.PictureLink = "https://shorturl.at/byBRU";
            string newImageTargetJson = JsonUtility.ToJson(newImageTarget);
            StartCoroutine(RestApiClient.Instance.HttpPost(baseUrl, newImageTargetJson, (r) => OnRequestComplete(r), new List<RequestHeader> { header }));
        }
        void OnRequestComplete(Response response)
        {
            Debug.Log("Status Code :" + response.StatusCode);
            Debug.Log("Message : " + response.Message);
            Debug.Log("Error : " + response.Error);
        }

    }



}
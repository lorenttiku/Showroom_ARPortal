using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class getdatafromAPI : MonoBehaviour
{
    public string url;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(getImageTarget());
        //StartCoroutine(getSingleImageTarget(2));
        //StartCoroutine(createImageTarget());
        //StartCoroutine(updateImageTarget(2));
        StartCoroutine(deleteImageTarget(5));
    }

    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator getImageTarget()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            //www.SetRequestHeader("Accept-Language", "en");
            //www.SendWebRequest("Authorization", "Bearer" + authorizationToken);

            AsyncOperation request = www.SendWebRequest();

            while (!request.isDone)
            {
                // show loading UI while getting data 
                yield return null;
            }


            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // show network error popup if theres no network
            }
            else
            {
                string imageTargets = www.downloadHandler.text;

                //if (imageTargets != "")
                //{

                //}
                Debug.Log(imageTargets);
                AllImageTargets allImageTargets = JsonUtility.FromJson<AllImageTargets>(imageTargets);
                Debug.Log(allImageTargets);
                Debug.Log(allImageTargets.data[0].AutorName);
                Debug.Log(allImageTargets.data[1].AutorName);
                Debug.Log(allImageTargets.data[2].AutorName);

                //get meta dta 
                Debug.Log(allImageTargets.meta.page);

            }
        }

    }

    private IEnumerator getSingleImageTarget(int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url + "/" + id))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            AsyncOperation request = www.SendWebRequest();

            while (!request.isDone)
            {
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // show network error popup if theres no network
            }
            else
            {
                string imageTargets = www.downloadHandler.text;
                Debug.Log(imageTargets);



            }
        }
    }

    private IEnumerator createImageTarget()
    {
        ImageTarget newImageTarget = new ImageTarget();
        newImageTarget.AutorName = "Ford";
        newImageTarget.Description = "An amazing car that will never stop!!";
     
        newImageTarget.PictureLink = "ford.png";

        string newImageTargetJson = JsonUtility.ToJson(newImageTarget);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, newImageTargetJson))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(newImageTargetJson);

            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-type", "application/json");

            AsyncOperation request = www.SendWebRequest();

            while (!request.isDone)
            {
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // show network error popup if theres no network
            }
            else
            {
                string imageTarget = www.downloadHandler.text;
                Debug.Log(imageTarget);
            }
        }
    }

    private IEnumerator updateImageTarget(int id)
    {
        ImageTarget updateImageTarget = new ImageTarget();
        updateImageTarget.AutorName = "Ford1";
        updateImageTarget.Description = "New Car";
        updateImageTarget.PictureLink = "ford1.png";

        string updateImageTargetJson = JsonUtility.ToJson(updateImageTarget);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(updateImageTargetJson);

        using (UnityWebRequest www = UnityWebRequest.Put(url + "/" + id, jsonToSend))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            AsyncOperation request = www.SendWebRequest();

            while (!request.isDone)
            {
                //show a message that data is loading
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // show network error
            }
            else
            {
                string imageTarget = www.downloadHandler.text;

                Debug.Log(imageTarget);
            }
        }
    }


    private IEnumerator deleteImageTarget(int id)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(url + "/" + id))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            AsyncOperation request = www.SendWebRequest();

            while (!request.isDone)
            {
                //show a message that data is loading
                yield return null;
            }

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // show network error
            }
            else
            {
                string imageTarget = www.downloadHandler.text;

                Debug.Log(imageTarget);
            }

        }
    }


}


[System.Serializable]



public class AllImageTargets
{
    public ImageTarget[] data;
    public Meta meta;

    public static implicit operator AllImageTargets(ImageTarget v)
    {
        throw new NotImplementedException();
    }
}
[System.Serializable]
public class ImageTarget
{
    public int id;
    public string AutorName;
    public string Description;
    public string PictureLink;


}

[System.Serializable] 
public class Meta
{
    public string page;
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class DisplayDataFromDatabase : MonoBehaviour
{
    public string baseUrl = "http://localhost:3000/portalusers"; // Change the URL according to your database endpoint

    public GameObject canvasPrefab;
    public Transform canvasSpawnPoint;

    void Start()
    {
        StartCoroutine(GetDataFromDatabase(baseUrl));
    }

    IEnumerator GetDataFromDatabase(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            // Parse JSON response to get texture URL and text data
            string jsonResponse = www.downloadHandler.text;
            TextureData textureData = JsonUtility.FromJson<TextureData>(jsonResponse);

            // Fetch texture
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(textureData.textureUrl);
            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.ConnectionError || textureRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching texture: " + textureRequest.error);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(textureRequest);

            // Instantiate the canvas prefab
            GameObject canvasObject = Instantiate(canvasPrefab, canvasSpawnPoint.position, Quaternion.identity);

            // Get references to the UI elements on the canvas
            RawImage textureDisplay = canvasObject.GetComponentInChildren<RawImage>();
            TextMeshProUGUI text1 = canvasObject.transform.Find("AutorName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI text2 = canvasObject.transform.Find("Description").GetComponent<TextMeshProUGUI>();

            // Display the texture on the RawImage
            textureDisplay.texture = texture;

            // Set the texts
            text1.text = textureData.AutorName;
            text2.text = textureData.Description;
        }
    }

    [System.Serializable]
    public class TextureData
    {
        public string textureUrl;
        public string AutorName;
        public string Description;
    }
}

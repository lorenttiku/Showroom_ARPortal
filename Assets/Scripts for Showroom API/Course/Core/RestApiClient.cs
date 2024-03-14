using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Course.Core.Singletons;
using Assets.Course.Core.RestHttp;
using UnityEngine.Networking;

namespace Assets.Course.Core
{
    public class RestApiClient : Singleton<RestApiClient>
    {
        private const string defContentType = "application/json";

        public IEnumerator HttpGet(string url, System.Action<Response> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                AsyncOperation request = www.SendWebRequest();
                //yield return www.SendWebRequest();
                while (!request.isDone)
                {
                    // show an UI loader
                    // show a message that data is being processed 
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                    }); 

                }
                else if (www.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                        Data = data
                    });
                }
            }
        }

        public IEnumerator HttpPost(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, body))
            {
                if (headers != null) 
                {
                    foreach (RequestHeader header in headers)
                    {
                        www.SetRequestHeader(header.Key, header.Value);
                    }
                }
                www.uploadHandler.contentType = defContentType;
                www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                AsyncOperation request = www.SendWebRequest();

                while (!request.isDone)
                {
                    // show an UI loader
                    // show a message that data is being processed 
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                    });

                }
                else if (www.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                        Message = data
                    }); 
                }


            }
        }
        public IEnumerator HttpPut(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, body))
            {
                if (headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        www.SetRequestHeader(header.Key, header.Value);
                    }
                }
                www.uploadHandler.contentType = defContentType;
                www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                AsyncOperation request = www.SendWebRequest();

                while (!request.isDone)
                {
                    // show an UI loader
                    // show a message that data is being processed 
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                    });

                }
                else if (www.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                        Data = data
                    });
                }


            }
        }


        public IEnumerator HttpDelete(string url, System.Action<Response> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Delete(url))
            {
                AsyncOperation request = www.SendWebRequest();

                while (!request.isDone)
                {
                    // show an UI loader
                    // show a message that data is being processed 
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Error = www.error,
                    });

                }
                else if (www.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                    callback(new Response
                    {
                        StatusCode = www.responseCode,
                        Message = data
                    }) ;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Course.Core.RestHttp
{
    public class Response
    {
        public long StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }



}

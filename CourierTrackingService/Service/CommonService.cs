using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CourierTrackingService.Service
{
    public class CommonService
    {
        /// <summary>
        /// SendApiRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string SendApiRequest(string url, string Request)
        {
            string strresponse = "";
            try
            {
                var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json";

                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    if (!string.IsNullOrEmpty(Request))
                        streamWriter.Write(Request);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strresponse = streamReader.ReadToEnd();
                }
            }
            catch 
            {
                
            }

            return strresponse;

        }
    }
}

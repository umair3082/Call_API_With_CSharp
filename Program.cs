using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApp_CallAnAPI
{
    class Program
    {
        public class Root
        {
            public int Count { get; set; }
            public List<Entry> entries { get; set; }
        }
        public class Entry
        {
            public string API { get; set; }
            public string Description { get; set; }
            public string Auth { get; set; }
            public string HTTPS { get; set; }
            public string Cors { get; set; }
            public string Link { get; set; }
            public string Category { get; set; }
        }
        static void Main(string[] args)
        {
            //https://apipheny.io/free-api/
            try
            {
                string URL = "https://api.publicapis.org/";
                string urlParameters = "entries";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    var ParsedJSONData = JsonConvert.DeserializeObject<Root>(dataObjects);

                    foreach (var d in ParsedJSONData.entries)
                    {
                        Console.WriteLine("{0}", d.Link);
                    }
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                // Make any other calls using HttpClient here.

                // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
                client.Dispose();
                Console.ReadLine();
            }
            catch (Exception)
            {

                //throw;
            }

        }
    }
}

using System.IO;
using System.Net;
using System;

namespace FreelanceBot.Parsers
{
    public class Requests
    {

        public static string HttpGetListITJobs(string url, string auth)
        {
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("api_key", auth);

                using (var data = client.OpenRead(url))
                {
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    data.Close();
                    reader.Close();

                    return s;
                }
            }
        }
        public static string HttpGetSearchITJobs(string url, string auth, string q, string p)
        {
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("api_key", auth);
                client.QueryString.Add("q", q);
                if (p != null  && p != "")
                {
                    client.QueryString.Add("page", p);
                }
                using (var data = client.OpenRead(url))
                {
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    data.Close();
                    reader.Close();

                    return s;
                }
            }
        }
   
    }
}

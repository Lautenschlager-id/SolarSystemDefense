using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SolarSystemDefense
{
	static class Internet
	{
		static HttpClient client = new HttpClient();
		static Internet()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
		}

		// Internet support @ https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request
		public static bool InternetConnection()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://g.cn/generate_204");
			request.UserAgent = "MonoGame's SolarSystemDefense Project";
			request.KeepAlive = false;
			request.Timeout = 3000;

			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					return response.ContentLength == 0 && response.StatusCode == HttpStatusCode.NoContent;
			}
			catch
			{
				return false;
			}
		}

		public static string HTTPGet(string url)
		{
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Add("User-Agent", "MonoGame's SolarSystemDefense Project");

			return client.GetStringAsync(url).Result;
		}
				
		public static string HTTPPost(string url, Info.WebServerJSONPut content)
		{
			client.DefaultRequestHeaders.Clear();
		
			string data = Encoding.UTF8.GetString(Utils.toJSON(content.GetType(), content));
			HttpContent body = new StringContent(data, Encoding.UTF8, "application/json");

			int tentative = 0;
			do
			{
				try
				{
					HttpResponseMessage result = client.PostAsync(url, body).GetAwaiter().GetResult();
					result.Content.Headers.ContentLength = result.Content.ToString().Length;
					return result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				}
				catch (Exception)
				{
					tentative++;
				}
			} while (tentative < 5);
			return "failed";
		}
	}
}

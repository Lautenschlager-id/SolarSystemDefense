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
			request.UserAgent = "XNA";
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

		public static string HTTPGet(string url, Dictionary<string, string> headers = null)
		{
			client.DefaultRequestHeaders.Clear();

			if (headers != null)
			{
				if (headers.ContainsKey("User-Agent"))
					client.DefaultRequestHeaders.Add("User-Agent", headers["User-Agent"]);

				if (headers.ContainsKey("Token"))
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", headers["Token"]);
			}

			return client.GetStringAsync(url).Result;
		}
		public static string HTTPPut(string url, string data, Dictionary<string, string> headers)
		{
			client.DefaultRequestHeaders.Clear();

			HttpContent body = new StringContent(data, Encoding.UTF8, "application/json");

			if (headers.ContainsKey("User-Agent"))
				client.DefaultRequestHeaders.Add("User-Agent", "SolarSystemDefense");

			if (headers.ContainsKey("Token"))
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", headers["Token"]);

			HttpResponseMessage result = client.PutAsync(url, body).GetAwaiter().GetResult();
			result.Content.Headers.ContentLength = result.Content.ToString().Length;

			string resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

			return resultContent;
		}
	}
}

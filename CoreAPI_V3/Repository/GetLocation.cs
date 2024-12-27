using Newtonsoft.Json.Linq;

namespace CoreAPI_V3.Repository
{
    public class GetLocation
    {

        public async Task<bool> IsVpnIpAddress(string ipAddress)
        {
            string apiKey = "kRbwM9x31oWj1On0swZHc5mpyTVuZ3Og";  //------------> change this API KEY through "ipqs" website.
            string apiUrl = $"https://ipqualityscore.com/api/json/ip/{apiKey}/{ipAddress}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);

                    bool isVpn = json["vpn"]?.ToObject<bool>() ?? false;

                    return isVpn;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return false;
                }
            }
        }

        public  async Task<string> GetPublicIPAddressAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://api.ipify.org");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public  async Task<string> GetLocationFromIpAsync(string ipAddress)
        {
            bool isVpn = await IsVpnIpAddress(ipAddress);

            if (isVpn)
            {
                return "VPN use";
            }

            string apiUrl = $"http://ip-api.com/json/{ipAddress}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);

                    if (json["status"]?.ToString() == "fail")
                    {
                        return $"Error: {json["message"]}";
                    }

                    string country = Convert.ToString(json["country"]);
                    string region = json["regionName"]?.ToString();
                    string city = json["city"]?.ToString();

                    return $"{city}({country})";
                }
                catch (HttpRequestException e)
                {
                    return $"Request error: {e.Message}";
                }
            }
        }

    }
}

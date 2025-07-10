using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Arcane_Launcher.Services
{
    public class Authentication
    {
        public static async Task<JObject> GetAccessToken(string AuthorizationCode)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token";

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", AuthorizationCode),
                    new KeyValuePair<string, string>("token_type", "eg1"),
                });

                client.DefaultRequestHeaders.Add("Authorization", "Basic ZWM2ODRiOGM2ODdmNDc5ZmFkZWEzY2IyYWQ4M2Y1YzY6ZTFmMzFjMjExZjI4NDEzMTg2MjYyZDM3YTEzZmM4NGQ=");

                HttpResponseMessage response = await client.PostAsync(url, requestData);
                string responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);
                if (json.ContainsKey("access_token"))
                {
                    Properties.Settings.Default.access_token = json["access_token"]?.ToString();
                    if (json.ContainsKey("refresh_token"))
                    {
                        Properties.Settings.Default.refresh_token = json["refresh_token"]?.ToString();
                    }
                    else
                    {
                        Utils.Logger.error("Could not get refresh token, Launching will still proceed but automatic login might not work!");
                    }
                    Properties.Settings.Default.displayName = json["displayName"]?.ToString();
                    Properties.Settings.Default.account_id = json["account_id"]?.ToString();
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Utils.Logger.error("Could not get access token!");
                }

                Utils.Logger.good($"Successfully logged into {json["displayName"]?.ToString()}");

                return json;
            }
        }

        public static async Task<bool> VerifyAccessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/verify";

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Properties.Settings.Default.access_token}");

                HttpResponseMessage response = await client.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);
                if (json.ContainsKey("account_id"))
                {
                    Utils.Logger.good($"Successfully logged into {json["displayName"]?.ToString()}");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<JObject> GetExchangeCode(string AccessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/exchange";

                client.DefaultRequestHeaders.Add("Authorization", $"bearer {AccessToken}");

                HttpResponseMessage response = await client.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);
                if (json.ContainsKey("code"))
                {
                    Utils.Logger.good("Got exchange code: " +  json["code"]);
                }
                else
                {
                    Utils.Logger.error("Failed to get exchange code!");
                }

                return json;
            }
        }

        public static async Task<JObject> RefreshToken(string refresh_token)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token";

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refresh_token),
                    new KeyValuePair<string, string>("token_type", "eg1"),
                });

                client.DefaultRequestHeaders.Add("Authorization", "Basic ZWM2ODRiOGM2ODdmNDc5ZmFkZWEzY2IyYWQ4M2Y1YzY6ZTFmMzFjMjExZjI4NDEzMTg2MjYyZDM3YTEzZmM4NGQ=");

                HttpResponseMessage response = await client.PostAsync(url, requestData);
                string responseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseString);
                if (json.ContainsKey("access_token"))
                {
                    Properties.Settings.Default.access_token = json["access_token"]?.ToString();
                    Properties.Settings.Default.displayName = json["displayName"]?.ToString();
                    Properties.Settings.Default.account_id = json["account_id"]?.ToString();
                    Properties.Settings.Default.Save();

                    Utils.Logger.good($"Successfully logged into {json["displayName"]?.ToString()}");
                }

                return json;
            }
        }
    }
}

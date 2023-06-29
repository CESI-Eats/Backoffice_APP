using Backoffice_APP.Models.Requests;
using Backoffice_APP.Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backoffice_APP.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;

        public BaseService()
        {
            _httpClient = new HttpClient();
        }

        private async Task RefreshTokens()
        {
            string json = JsonConvert.SerializeObject(new RefreshRequest() { refreshToken = AppUser.RefreshToken });
            LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await Post(ConfigurationManager.AppSettings["refresh_url"], json, false));
            AppUser.Token = loginResponse.Token;
            AppUser.RefreshToken = loginResponse.NewRefreshToken;
        }

        protected async Task<string> Post(string url, string json, bool handleUnauthorized = true)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized && handleUnauthorized)
            {
                await RefreshTokens();
                return await Post(url, json, false);
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new Exception(error.Message);
            }
            return responseContent;
        }

        protected async Task<string> Get(string url, bool handleUnauthorized = true)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AppUser.Token);
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized && handleUnauthorized)
            {
                await RefreshTokens();
                return await Get(url, false);
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                throw new Exception(error.Message);
            }
            return responseContent;
        }
    }
}

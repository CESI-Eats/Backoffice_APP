using Backoffice_APP.Models.Requests;
using Backoffice_APP.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backoffice_APP.Services
{
    public class LoginService
    {
        private readonly HttpClient httpClient;

        public LoginService()
        {
            httpClient = new HttpClient();
        }

        public async Task<bool> Login(string mail, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Mail = mail,
                    Password = password
                };

                string json = JsonConvert.SerializeObject(loginRequest);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(ConfigurationManager.AppSettings["login_url"], content);

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    AppUser.Mail = mail;
                    AppUser.Token = loginResponse?.Token;
                    AppUser.RefreshToken = loginResponse?.RefreshToken;

                    return true;
                }
                else
                {
                    throw new Exception(loginResponse?.Message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

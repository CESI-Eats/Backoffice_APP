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
    public class LoginService : BaseService
    {

        public async Task Login(string mail, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Mail = mail,
                    Password = password
                };

                string json = JsonConvert.SerializeObject(loginRequest);

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await MakeRequest(json));

                AppUser.Mail = mail;
                AppUser.Token = loginResponse?.Token;
                AppUser.RefreshToken = loginResponse?.RefreshToken;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

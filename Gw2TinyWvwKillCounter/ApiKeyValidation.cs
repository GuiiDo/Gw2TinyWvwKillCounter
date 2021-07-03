using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Gw2Sharp;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;

namespace Gw2TinyWvwKillCounter
{
    public class ApiKeyValidation
    {
        public static async Task<bool> ApiKeyIsInvalid(string apiKey)
        {
            if(string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("API key is missing. Set API key in settings.");
                return true;
            }

            var connection = new Connection(apiKey);
            var gw2Client  = new Gw2Client(connection);
            TokenInfo tokenInfo;


            try
            {
                tokenInfo = await gw2Client.WebApi.V2.TokenInfo.GetAsync();
            }
            catch (InvalidAccessTokenException)
            {
                MessageBox.Show("Invalid API key.");
                return true;
            }

            if (ApiKeyIsMissingNecessaryPermissions(tokenInfo))
            {
                MessageBox.Show("API key is missing permissions. This app needs account, progression and characters permissions.");
                return true;
            }

            return false;
        }

        private static bool ApiKeyIsMissingNecessaryPermissions(TokenInfo tokenInfo)
        {
            var tokenPermissions = tokenInfo.Permissions.List.Select(a => a.Value).ToList();

            var apiKeyHasNecessaryPermissions = tokenPermissions.Contains(TokenPermission.Account)
                                                && tokenPermissions.Contains(TokenPermission.Progression)
                                                && tokenPermissions.Contains(TokenPermission.Characters);

            return apiKeyHasNecessaryPermissions == false;
        }
    }
}
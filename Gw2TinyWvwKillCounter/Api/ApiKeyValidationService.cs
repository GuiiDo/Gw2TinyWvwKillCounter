using System.Linq;
using System.Threading.Tasks;
using Gw2Sharp;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using Gw2TinyWvwKillCounter.ViewAndViewModels;

namespace Gw2TinyWvwKillCounter.Api
{
    public class ApiKeyValidationService
    {
        public static async Task<bool> ApiKeyIsInvalid(string apiKey)
        {
            if(string.IsNullOrWhiteSpace(apiKey))
            {
                new ErrorDialogView("API key is missing. Set API key in settings.").ShowDialog();
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
                new ErrorDialogView("Invalid API key.").ShowDialog();
                return true;
            }

            if (ApiKeyIsMissingNecessaryPermissions(tokenInfo))
            {
                new ErrorDialogView("API key is missing permissions. This app needs account, progression and characters permissions.").ShowDialog();
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
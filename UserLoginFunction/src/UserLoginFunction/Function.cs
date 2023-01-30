using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using UserLoginFunction.ElastiCache;
using UserLoginFunction.Login;
using UserLoginFunction.SecretsManager;
using UserLoginFunction.SecretsManager.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserLoginFunction;

public class Function
{
    private readonly string? _matchUpSiteAppSecretName = Environment.GetEnvironmentVariable("UserPoolAppSecret");
    private ILambdaContext _lambdaContext = null!;

    public async Task FunctionHandler(LoginModel loginModel, ILambdaContext context)
    {
        _lambdaContext = context;

        if (string.IsNullOrWhiteSpace(_matchUpSiteAppSecretName))
        {
            context.Logger.LogCritical("Failed to retrieve secret name from environment variables.");

            return;
        }

        var userPoolCredentials = await GetUserPoolCredentials();

        if (userPoolCredentials is null)
        {
            return;
        }

        var loginHandler = new LoginHandler();

        var loginResult = await loginHandler.LoginUser(loginModel, userPoolCredentials);

        if (string.IsNullOrWhiteSpace(loginResult))
        {
            context.Logger.LogWarning($"Failed to log in user {loginModel.UserName}. Access token is null or empty.");
                
            return;
        }

        var cacheHandler = new CacheHandler();
        var saveToCacheResult = await cacheHandler.SaveStringToCache(loginModel.UserName!, loginResult);

        if (!saveToCacheResult)
        {
            context.Logger.LogCritical($"Failed to log in user {loginModel.UserName}. Failed to save token to cache.");
        }
    }

    private async Task<UserPoolAppCredentials?> GetUserPoolCredentials()
    {
        var secretsManagerHandler = new SecretsManagerHandler();

        var secretResponse = await secretsManagerHandler.RetrieveSecret(_matchUpSiteAppSecretName!);

        if (string.IsNullOrWhiteSpace(secretResponse))
        {
            _lambdaContext.Logger.LogCritical("Failed to retrieve secret. Secret is null or empty.");
        }

        var deserializedSecret = JsonConvert.DeserializeObject<UserPoolAppCredentials>(secretResponse);

        if (!string.IsNullOrWhiteSpace(deserializedSecret?.AppClientId))
        {
            return deserializedSecret;
        }

        _lambdaContext.Logger.LogCritical(
            "Failed to deserialize secret. AppClientId is null or empty.");

        return null!;
    }
}
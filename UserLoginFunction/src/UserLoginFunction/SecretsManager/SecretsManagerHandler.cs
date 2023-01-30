using System.Threading.Tasks;
using Amazon.SecretsManager.Extensions.Caching;

namespace UserLoginFunction.SecretsManager;

public class SecretsManagerHandler : ISecretsManagerHandler
{
    public async Task<string> RetrieveSecret(string secretName)
    {
        var secretsManagerCache = new SecretsManagerCache();

        var secret = await secretsManagerCache.GetSecretString(secretName);

        return secret;
    }
}
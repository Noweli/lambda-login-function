using System.Threading.Tasks;

namespace UserLoginFunction.SecretsManager;

public interface ISecretsManagerHandler
{
    public Task<string> RetrieveSecret(string secretName);
}
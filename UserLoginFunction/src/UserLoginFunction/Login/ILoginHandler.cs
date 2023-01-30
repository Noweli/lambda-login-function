using System.Threading.Tasks;
using UserLoginFunction.SecretsManager.Models;

namespace UserLoginFunction.Login;

public interface ILoginHandler
{
    Task<string> LoginUser(LoginModel loginModel, UserPoolAppCredentials userPoolAppCredentials);
}
using System.Net;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using UserLoginFunction.SecretsManager.Models;

namespace UserLoginFunction.Login;

public class LoginHandler : ILoginHandler
{
    public async Task<string> LoginUser(LoginModel loginModel, UserPoolAppCredentials userPoolAppCredentials)
    {
        var identityProviderClient = new AmazonCognitoIdentityProviderClient();

        var authRequest = new AdminInitiateAuthRequest
        {
            ClientId = userPoolAppCredentials.AppClientId,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH
        };

        authRequest.AuthParameters.Add("USERNAME", loginModel.UserName);
        authRequest.AuthParameters.Add("PASSWORD", loginModel.Password);

        var authResponse = await identityProviderClient.AdminInitiateAuthAsync(authRequest);

        return authResponse.HttpStatusCode != HttpStatusCode.OK
            ? string.Empty
            : authResponse.AuthenticationResult.AccessToken;
    }
}
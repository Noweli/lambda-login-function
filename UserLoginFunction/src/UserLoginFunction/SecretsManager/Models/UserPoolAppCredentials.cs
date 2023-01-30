using Newtonsoft.Json;

namespace UserLoginFunction.SecretsManager.Models;

public class UserPoolAppCredentials
{
    [JsonProperty("appClientId")] public string? AppClientId { get; set; }
}
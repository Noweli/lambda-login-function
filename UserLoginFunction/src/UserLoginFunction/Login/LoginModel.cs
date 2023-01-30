using Newtonsoft.Json;

namespace UserLoginFunction.Login;

public class LoginModel
{
    [JsonProperty("userName")] public string? UserName { get; set; }
    [JsonProperty("password")] public string? Password { get; set; }
}
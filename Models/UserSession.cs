namespace Web_JbcHrms.Models;

public class UserSession
{
    public string UserName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Uid { get; set; } = string.Empty;
}

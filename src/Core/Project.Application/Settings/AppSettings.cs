namespace Project.Application.Settings;
public class AppSettings : IAppSettings
{
    public string Secret { get; set; }
    public string PasswordHashSecret { get; set; }
}

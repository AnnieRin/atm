namespace Project.Application.Settings;
public interface IAppSettings
{
    string Secret { get; set; }
    string PasswordHashSecret { get; set; }
}

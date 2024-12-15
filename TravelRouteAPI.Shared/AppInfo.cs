using TravelRouteAPI.Shared.Enums;

namespace TravelRouteAPI.Shared;

/// <summary>
/// Модель данных информации о приложении.
/// </summary>
public class AppInfo
{
    #region Props

    private static AppInfo? _current;

    /// <summary>
    /// Текущее приложение.
    /// </summary>
    public static AppInfo Current => _current ?? throw new InvalidOperationException("AppInfo not initialized");

    /// <summary>
    /// Идентификатор инстанса приложения. Меняется при перезапуске приложения, не изменяется во время жизни приложения.
    /// </summary>
    public Guid AppId { get; private set; }

    /// <summary>
    /// Текущее имя хоста приложения.
    /// </summary>
    public string Host => System.Environment.MachineName.ToLower();

    /// <summary>
    /// Название приложения.
    /// </summary>
    public string AppName { get; private set; } = "unknown";

    /// <summary>
    /// Окружение.
    /// </summary>
    public AppEnvironment Environment { get; private set; }

    #endregion Props

    private AppInfo() { }

    public static void InitializeApp(string appName, string environment)
    {
        if (_current is not null)
        {
            return;
        }

        var appHelper = new AppInfo
        {
            AppId = Guid.NewGuid(),
            AppName = appName,
            Environment = Enum.TryParse<AppEnvironment>(environment, out var appEnv) ? appEnv : AppEnvironment.Unknown
        };

        _current = appHelper;
    }
}

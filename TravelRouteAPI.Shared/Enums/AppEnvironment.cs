namespace TravelRouteAPI.Shared.Enums;

/// <summary>
/// Перечисление окружений.
/// </summary>
public enum AppEnvironment
{
    /// <summary>
    /// Неизвестное окружение.
    /// </summary>
    Unknown,

    /// <summary>
    /// Окружение разработки.
    /// </summary>
    Development,

    /// <summary>
    /// Тестовое окружение.
    /// </summary>
    Test,

    /// <summary>
    /// Прод окружение.
    /// </summary>
    Production,

    /// <summary>
    /// Промежуточное окружение.
    /// </summary>
    Staging
}

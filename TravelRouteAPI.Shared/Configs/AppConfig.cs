using System.Text.Json.Serialization;

namespace TravelRouteAPI.Shared;

/// <summary>
/// Модель описывающая конфигурацию приложений.
/// </summary>
public class AppsConfig
{
    [JsonPropertyName("consumer_configs")]
    public List<AppConfig> AppConfigs { get; set; } = [];
}

/// <summary>
/// Модель описывающая конфигурацию приложения.
/// </summary>
public class AppConfig : BaseConfig
{
    /// <summary>
    /// Именование хоста, на котором крутится приложение.
    /// </summary>
    [JsonPropertyName("server_name")]
    public string HostName { get; set; } = "unknown";

    /// <summary>
    /// Конфигурация потребителя сообщений из кафки.
    /// </summary>
    [JsonPropertyName("consumer_name")]
    public ConsumerConfig? ConsumerConfig { get; set; }

    /// <summary>
    /// Конфигурация отправки событий в кролик.
    /// </summary>
    [JsonPropertyName("publish_config")]
    public PublishEventsConfig? PublishEventsConfig { get; set; }

    /// <summary>
    /// Конфигурация логирования.
    /// </summary>
    [JsonPropertyName("logging_config")]
    public LoggingConfig? LoggingConfig { get; set; }

    public override BaseConfig GetDefault()
    {
        return new AppConfig()
        {
            ConsumerConfig = new ConsumerConfig().GetDefault() as ConsumerConfig,
            PublishEventsConfig = new PublishEventsConfig().GetDefault() as PublishEventsConfig,
            LoggingConfig = new LoggingConfig().GetDefault() as LoggingConfig
        };
    }
}

/// <summary>
/// Модель описывающая конфигурацию потребителя сообщений из топика кафки.
/// </summary>
public class ConsumerConfig : BaseConfig
{
    [JsonPropertyName("prefetch_count")]
    public int PrefetchCount { get; set; }

    [JsonPropertyName("concurrent_message_limit")]
    public int ConcurrentMessageLimit { get; set; }

    [JsonPropertyName("concurrent_delivery_limit")]
    public int ConcurrentDeliveryLimit { get; set; }

    [JsonPropertyName("concurrent_consumer_limit")]
    public int ConcurrentConsumerLimit { get; set; }

    [JsonPropertyName("queued_max_messages_kbytes")]
    public int QueuedMaxMessagesKbytes { get; set; }

    [JsonPropertyName("min_bytes")]
    public int MinBytes { get; set; }

    [JsonPropertyName("max_bytes")]
    public int MaxBytes { get; set; }

    [JsonPropertyName("wait_max_interval_seconds")]
    public int WaitMaxIntervalSeconds { get; set; }

    public override BaseConfig GetDefault()
    {
        return new ConsumerConfig()
        {
            PrefetchCount = 10,
            ConcurrentMessageLimit = 40,
            ConcurrentDeliveryLimit = 2,
            ConcurrentConsumerLimit = 1,
            QueuedMaxMessagesKbytes = 500000,
            MaxBytes = 15000000,
            MinBytes = 10000000,
            WaitMaxIntervalSeconds = 5
        };
    }
}

/// <summary>
/// Модель описывающая конфигурацию отправки событий в кролик.
/// </summary>
public class PublishEventsConfig : BaseConfig
{
    /// <summary>
    /// Возвращает <see langword="true"/> если публикация событий разрешена; иначе <see langword="false"/>.
    /// </summary>
    [JsonPropertyName("can_publish")]
    public bool IsCanPublish { get; set; }

    /// <summary>
    /// Размер пакета событий, которые будут отправлены.
    /// </summary>
    [JsonPropertyName("chunk_size")]
    public int BatchSize { get; set; }

    /// <summary>
    /// Конфигурация пропускной способности отправки событий.
    /// </summary>
    [JsonPropertyName("bucket_config")]
    public BucketConfig? BucketConfig { get; set; }

    public override BaseConfig GetDefault()
    {
        return new PublishEventsConfig()
        {
            IsCanPublish = true,
            BatchSize = 500,
            BucketConfig = new BucketConfig().GetDefault() as BucketConfig
        };
    }
}

/// <summary>
/// Модель описывающая конфигурацию логирования.
/// </summary>
public class LoggingConfig : BaseConfig
{
    /// <summary>
    /// Возвращает <see langword="true"/> если логирование разрешено; иначе <see langword="false"/>.
    /// </summary>
    [JsonPropertyName("can_logging")]
    public bool IsCanLogging { get; set; }

    /// <summary>
    /// Конфигурация пропускной способности логирования в highload части приложения.
    /// </summary>
    [JsonPropertyName("bucket_config")]
    public BucketConfig? BucketConfig { get; set; }

    public override BaseConfig GetDefault()
    {
        return new LoggingConfig()
        {
            IsCanLogging = true,
            BucketConfig = new BucketConfig()
            {
                Name = "app_logger",
                Capacity = 100,
                RefillTokens = 100,
                RefillPeriodSecond = 60
            }
        };
    }
}

/// <summary>
/// Модель описывающая конфигурацию пропускной способности.
/// </summary>
public class BucketConfig : BaseConfig
{
    /// <summary>
    /// Именование.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Общая ёмкость.
    /// </summary>
    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }

    /// <summary>
    /// Кол-во токенов, который будут освобождены.
    /// </summary>
    [JsonPropertyName("refill_tokens")]
    public int RefillTokens { get; set; }

    /// <summary>
    /// Период времени в сек., когда токены будут освобождены.
    /// </summary>
    [JsonPropertyName("refill_period_second")]
    public int RefillPeriodSecond { get; set; }

    public override BaseConfig GetDefault()
    {
        return new BucketConfig()
        {
            Name = "app_bucket",
            Capacity = 100,
            RefillTokens = 100,
            RefillPeriodSecond = 60
        };
    }
}

/// <summary>
/// Базовая модель конфигурации.
/// </summary>
public abstract class BaseConfig
{
    /// <summary>
    /// Возвращает конфигурациб со значениями по умолчанию.
    /// </summary>
    public abstract BaseConfig GetDefault();
}

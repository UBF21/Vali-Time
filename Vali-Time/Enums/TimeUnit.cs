namespace Vali_Time.Enums;

/// <summary>
/// Represents the supported units of time for conversions and calculations in the Vali-Time library.
/// These units define the scale of time values, ranging from milliseconds to hours.
/// </summary>
public enum TimeUnit
{
    /// <summary>
    /// Represents time in milliseconds (1/1000 of a second). Suitable for high-precision measurements.
    /// </summary>
    Milliseconds,

    /// <summary>
    /// Represents time in seconds. A base unit for many time-related calculations.
    /// </summary>
    Seconds,

    /// <summary>
    /// Represents time in minutes (60 seconds). Commonly used for short durations.
    /// </summary>
    Minutes,

    /// <summary>
    /// Represents time in hours (3600 seconds). Used for longer time spans.
    /// </summary>
    Hours
}
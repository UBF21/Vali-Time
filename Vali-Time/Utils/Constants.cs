namespace Vali_Time.Utils;

/// <summary>
/// Provides a collection of constant values used throughout the Vali-Time library for time unit conversions and formatting.
/// These constants ensure consistency and precision in calculations and string representations.
/// </summary>
public  static class Constants
{
   /// <summary>
    /// Represents the integer value one (1). Used as a multiplier or base value in calculations.
    /// </summary>
    public const int One = 1;

    /// <summary>
    /// Represents the decimal value one (1.0). Used as a multiplier or base value in precise decimal calculations.
    /// </summary>
    public const decimal OneDecimal = 1m;

    /// <summary>
    /// Represents the default number of decimal places (2) for formatting time values.
    /// </summary>
    public const int Two = 2;

    /// <summary>
    /// Represents the integer value zero (0). Used as a default or initial value in calculations.
    /// </summary>
    public const int Zero = 0;

    /// <summary>
    /// Represents the decimal value zero (0.0). Used as a default or initial value in precise decimal calculations.
    /// </summary>
    public const decimal ZeroDecimal = 0m;

    /// <summary>
    /// Represents the number of seconds in a minute (60). Used for converting between seconds and minutes.
    /// </summary>
    public const int SecondsInMinute = 60;

    /// <summary>
    /// Represents the number of milliseconds in a second (1000). Used for converting between milliseconds and seconds.
    /// </summary>
    public const decimal MillisecondsInSecond = 1000m;

    /// <summary>
    /// Represents the number of seconds in an hour (3600). Used for converting between seconds and hours.
    /// </summary>
    public const int SecondsInHour = 3600;

    /// <summary>
    /// Represents the string prefix for milliseconds ("ms"). Used in time formatting.
    /// </summary>
    public const string PrefixMilliseconds = "ms";

    /// <summary>
    /// Represents the string prefix for seconds ("s"). Used in time formatting.
    /// </summary>
    public const string PrefixSeconds = "s";

    /// <summary>
    /// Represents the string prefix for minutes ("min"). Used in time formatting.
    /// </summary>
    public const string PrefixMinutes = "min";

    /// <summary>
    /// Represents the string prefix for hours ("h"). Used in time formatting.
    /// </summary>
    public const string PrefixHours = "h";

}
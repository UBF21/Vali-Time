using System.Globalization;
using Vali_Time.Enums;
using Vali_Time.Utils;

namespace Vali_Time.Core;

/// <summary>
/// Helper class for converting time units between seconds, minutes, and hours with maximum precision.
/// </summary>
public class ValiTime
{
    private const decimal MillisecondsInSecond = 1000m;
    private const int SecondsInMinute = 60;
    private const int SecondsInHour = 3600;
    
    /// <summary>
    /// Converts a time value from one unit to another with full precision, optionally applying rounding.
    /// </summary>
    /// <param name="time">The time value to convert.</param>
    /// <param name="fromUnit">The source unit of the time.</param>
    /// <param name="toUnit">The target unit to convert the time to.</param>
    /// <param name="decimalPlaces">The number of decimal places to round to; if null, no rounding is applied.</param>
    /// <param name="rounding">The rounding strategy to apply if rounding is requested (default is ToEven).</param>
    /// <returns>The converted time in the target unit with full precision unless rounding is specified.</returns>
    /// <exception cref="ArgumentException">Thrown if the time is negative or decimalPlaces is negative.</exception>
    /// <exception cref="NotSupportedException">Thrown if an unsupported unit is provided.</exception>
    public decimal Convert(decimal time, TimeUnit fromUnit, TimeUnit toUnit, int? decimalPlaces = null, MidpointRounding rounding = MidpointRounding.ToEven)
    {
        if (time < Constants.Zero) throw new ArgumentException("Time cannot be negative.", nameof(time));
        if (decimalPlaces is < Constants.Zero) throw new ArgumentException("Decimal places cannot be negative.", nameof(decimalPlaces));

        decimal timeInSeconds = fromUnit switch
        {
            TimeUnit.Milliseconds => MillisecondsToSeconds(time),
            TimeUnit.Seconds => time,
            TimeUnit.Minutes => MinutesToSeconds(time),
            TimeUnit.Hours => HoursToSeconds(time),
            _ => throw new NotSupportedException("TimeUnit not supported.")
        };

        decimal result = toUnit switch
        {
            TimeUnit.Milliseconds => SecondsToMilliseconds(timeInSeconds),
            TimeUnit.Seconds => timeInSeconds,
            TimeUnit.Minutes => SecondsToMinutes(timeInSeconds),
            TimeUnit.Hours => SecondsToHours(timeInSeconds),
            _ => throw new NotSupportedException("TimeUnit not supported.")
        };

        return decimalPlaces.HasValue ? decimal.Round(result, decimalPlaces.Value, rounding) : result;
    }
    
    /// <summary>
    /// Adds multiple time values in different units and returns the result in a specified unit with full precision, optionally applying rounding.
    /// </summary>
    /// <param name="resultUnit">The unit for the result.</param>
    /// <param name="times">List of tuples containing time values and their units to add.</param>
    /// <param name="decimalPlaces">The number of decimal places to round the result to; if null, no rounding is applied.</param>
    /// <param name="rounding">The rounding strategy to apply if rounding is requested (default is ToEven).</param>
    /// <returns>The sum of all times converted to the specified unit with full precision unless rounding is specified.</returns>
    /// <exception cref="ArgumentException">Thrown if the times array is null or empty, or if decimalPlaces is negative.</exception>
    /// <exception cref="NotSupportedException">Thrown if an unsupported unit is provided.</exception>
    public decimal SumTimes(TimeUnit resultUnit, List<(decimal time, TimeUnit unit)> times, int? decimalPlaces = null, MidpointRounding rounding = MidpointRounding.ToEven)
    {
        if (times == null || !times.Any())
            throw new ArgumentException("At least one time value must be provided.", nameof(times));
        if (decimalPlaces is < Constants.Zero)
            throw new ArgumentException("Decimal places cannot be negative.", nameof(decimalPlaces));

        decimal totalSeconds = times.Sum(t => Convert(t.time, t.unit, TimeUnit.Seconds));
        return Convert(totalSeconds, TimeUnit.Seconds, resultUnit, decimalPlaces, rounding);
    }
    
    /// <summary>
    /// Formats a time value into a human-readable string with customizable precision.
    /// </summary>
    /// <param name="time">The time value to format.</param>
    /// <param name="unit">The unit in which to express the time.</param>
    /// <param name="decimalPlaces">Number of decimal places to display (default is 2).</param>
    /// <param name="culture">Culture for numeric formatting (optional, defaults to current culture).</param>
    /// <returns>A formatted string representing the time (e.g., "1.25 h").</returns>
    /// <exception cref="NotSupportedException">Thrown if an unsupported unit is provided.</exception>
    public string FormatTime(decimal time, TimeUnit unit, int decimalPlaces = 2, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture; 
        string suffix = unit switch
        {
            TimeUnit.Milliseconds => Constants.PrefixMilliseconds,
            TimeUnit.Seconds => Constants.PrefixSeconds,
            TimeUnit.Minutes => Constants.PrefixMinutes,
            TimeUnit.Hours => Constants.PrefixHours,
            _ => throw new NotSupportedException("TimeUnit not supported.")
        };
        return $"{time.ToString($"F{decimalPlaces}", culture)} {suffix}";
    }
    
    /// <summary>
    /// Determines the most appropriate unit for a given time in seconds with full precision.
    /// </summary>
    /// <param name="seconds">The time in seconds.</param>
    /// <returns>A tuple with the converted time and the best unit, preserving full precision.</returns>
    /// <exception cref="ArgumentException">Thrown if the time is negative.</exception>
    public (decimal time, TimeUnit unit) GetBestUnit(decimal seconds)
    {
        if (seconds < Constants.Zero) throw new ArgumentException("Time cannot be negative.", nameof(seconds));
        if (seconds >= SecondsInHour) return (SecondsToHours(seconds), TimeUnit.Hours);
        if (seconds >= SecondsInMinute) return (SecondsToMinutes(seconds), TimeUnit.Minutes);
        if (seconds >= Constants.OneDecimal) return (seconds, TimeUnit.Seconds);
        return (SecondsToMilliseconds(seconds), TimeUnit.Milliseconds);
    }
    
    /// <summary>
    /// Converts a time value to a TimeSpan object with full precision.
    /// </summary>
    /// <param name="time">The time value to convert.</param>
    /// <param name="unit">The unit of the time value.</param>
    /// <returns>A TimeSpan representing the time.</returns>
    /// <exception cref="ArgumentException">Thrown if the time is negative.</exception>
    /// <exception cref="NotSupportedException">Thrown if an unsupported unit is provided.</exception>
    public TimeSpan ToTimeSpan(decimal time, TimeUnit unit)
    {
        if (time < Constants.Zero) throw new ArgumentException("Time cannot be negative.", nameof(time));
        decimal seconds = Convert(time, unit, TimeUnit.Seconds);
        return TimeSpan.FromSeconds((double)seconds);
    }
    
    /// <summary>
    /// Breaks down a time in seconds into a dictionary of units with full precision.
    /// </summary>
    /// <param name="seconds">The time in seconds to break down.</param>
    /// <returns>A dictionary with the time distributed across units, preserving full precision.</returns>
    /// <exception cref="ArgumentException">Thrown if the time is negative.</exception>
    public Dictionary<TimeUnit, decimal> Breakdown(decimal seconds)
    {
        if (seconds < Constants.Zero) throw new ArgumentException("Time cannot be negative.", nameof(seconds));

        var breakdown = new Dictionary<TimeUnit, decimal>
        {
            [TimeUnit.Hours] = SecondsToHours(seconds),
            [TimeUnit.Minutes] = Constants.ZeroDecimal,
            [TimeUnit.Seconds] = Constants.ZeroDecimal,
            [TimeUnit.Milliseconds] = Constants.ZeroDecimal
        };

        decimal hoursInteger = decimal.Floor(breakdown[TimeUnit.Hours]);
        decimal remainingSeconds = seconds - HoursToSeconds(hoursInteger);
        breakdown[TimeUnit.Hours] = hoursInteger;

        breakdown[TimeUnit.Minutes] = SecondsToMinutes(remainingSeconds);
        decimal minutesInteger = decimal.Floor(breakdown[TimeUnit.Minutes]);
        remainingSeconds -= MinutesToSeconds(minutesInteger);
        breakdown[TimeUnit.Minutes] = minutesInteger;

        breakdown[TimeUnit.Seconds] = remainingSeconds;
        decimal secondsInteger = decimal.Floor(breakdown[TimeUnit.Seconds]);
        decimal remainingMilliseconds = (remainingSeconds - secondsInteger) * MillisecondsInSecond;
        breakdown[TimeUnit.Seconds] = secondsInteger;

        breakdown[TimeUnit.Milliseconds] = remainingMilliseconds;

        return breakdown;
    }
    
    private decimal MillisecondsToSeconds(decimal milliseconds) => milliseconds / MillisecondsInSecond;
    private decimal SecondsToMilliseconds(decimal seconds) => seconds * MillisecondsInSecond;
    private decimal MinutesToSeconds(decimal minutes) => minutes * SecondsInMinute;
    private decimal HoursToSeconds(decimal hours) => hours * SecondsInHour;
    private decimal SecondsToMinutes(decimal seconds) => seconds / SecondsInMinute;
    private decimal SecondsToHours(decimal seconds) => seconds / SecondsInHour;
}
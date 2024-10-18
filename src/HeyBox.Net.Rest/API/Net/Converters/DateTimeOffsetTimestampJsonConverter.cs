using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class DateTimeOffsetTimestampJsonConverter : JsonConverter<DateTimeOffset>
{
    public TimestampUnit Unit { get; set; } = TimestampUnit.Milliseconds;

    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        DateTimeOffset time = DateTimeOffsetTimestampHelper.Read(ref reader, Unit);
        if (time == DateTimeOffset.UnixEpoch)
            throw new JsonException("Invalid timestamp");
        return time;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
        DateTimeOffsetTimestampHelper.Write(writer, value, Unit);
}

internal class NullableDateTimeOffsetTimestampJsonConverter : JsonConverter<DateTimeOffset?>
{
    /// <inheritdoc />
    public override bool HandleNull => true;

    public TimestampUnit Unit { get; set; } = TimestampUnit.Milliseconds;

    /// <inheritdoc />
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return null;
        DateTimeOffset time = DateTimeOffsetTimestampHelper.Read(ref reader, Unit);
        if (time == DateTimeOffset.UnixEpoch) return null;
        return DateTimeOffsetTimestampHelper.Read(ref reader, Unit);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteNullValue();
            return;
        }
        DateTimeOffsetTimestampHelper.Write(writer, value.Value, Unit);
    }
}

static file class DateTimeOffsetTimestampHelper
{
    public static DateTimeOffset Read(ref Utf8JsonReader reader, TimestampUnit unit)
    {
        switch (unit)
        {
            case TimestampUnit.Milliseconds:
                if (reader.TokenType is JsonTokenType.Number)
                    return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64());
                if (reader.TokenType is JsonTokenType.String
                    && reader.GetString() is { } millisecondStr)
                    return DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(millisecondStr));
                throw new JsonException();
            case TimestampUnit.Seconds:
                if (reader.TokenType is JsonTokenType.Number)
                    return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64());
                if (reader.TokenType is JsonTokenType.String
                    && reader.GetString() is { } secondStr)
                    return DateTimeOffset.FromUnixTimeSeconds(long.Parse(secondStr));
                throw new JsonException();
            default:
                throw new NotSupportedException($"Unsupported format: {unit}");
        }
    }

    public static void Write(Utf8JsonWriter writer, DateTimeOffset value, TimestampUnit unit)
    {
        switch (unit)
        {
            case TimestampUnit.Milliseconds:
                writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
                break;
            case TimestampUnit.Seconds:
                writer.WriteNumberValue(value.ToUnixTimeSeconds());
                break;
            default:
                throw new NotSupportedException($"Unsupported format: {unit}");
        }
    }
}

internal class DateTimeOffsetTimestampJsonConverterAttribute : JsonConverterAttribute
{
    public TimestampUnit Unit { get; set; } = TimestampUnit.Milliseconds;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>))
            return new NullableDateTimeOffsetTimestampJsonConverter { Unit = Unit };
        if (typeToConvert == typeof(DateTimeOffset))
            return new DateTimeOffsetTimestampJsonConverter { Unit = Unit };
        throw new NotSupportedException($"Unsupported type: {typeToConvert}");
    }
}

internal enum TimestampUnit
{
    Milliseconds,
    Seconds
}

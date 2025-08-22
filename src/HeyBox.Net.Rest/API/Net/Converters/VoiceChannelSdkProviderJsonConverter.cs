using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBox.Net.Converters;

internal class VoiceChannelSdkProviderJsonConverter : JsonConverter<VoiceChannelSdkProvider>
{
    /// <inheritdoc />
    public override VoiceChannelSdkProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            return VoiceChannelSdkProvider.Unknown;
        string? value = reader.GetString();
        return value switch
        {
            "trtc" => VoiceChannelSdkProvider.Tencent,
            "volc" => VoiceChannelSdkProvider.Volcengine,
            _ => VoiceChannelSdkProvider.Unknown
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, VoiceChannelSdkProvider value, JsonSerializerOptions options)
    {
        string? result = value switch
        {
            VoiceChannelSdkProvider.Tencent => "trtc",
            VoiceChannelSdkProvider.Volcengine => "volc",
            _ => null
        };
        if (result is not null)
            writer.WriteStringValue(result);
        else
            throw new JsonException($"Unsupported {nameof(VoiceChannelSdkProvider)} value: {value}");
    }
}

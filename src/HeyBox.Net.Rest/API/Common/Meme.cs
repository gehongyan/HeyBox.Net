﻿using System.Text.Json.Serialization;
using HeyBox.Net.Converters;

namespace HeyBox.API;

internal class Meme
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("path")]
    public required ulong Path { get; set; }

    [JsonPropertyName("ext")]
    public required string Extension { get; set; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter]
    public DateTimeOffset? CreateTime { get; set; }

    [JsonPropertyName("mtype")]
    public int MemeType { get; set; }
}

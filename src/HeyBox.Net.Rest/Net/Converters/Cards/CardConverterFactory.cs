﻿using System.Text.Json;
using System.Text.Json.Serialization;
using HeyBox.API;

namespace HeyBox.Net.Converters;

internal class CardConverterFactory : JsonConverterFactory
{
    public static readonly CardConverterFactory Instance = new();

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert == typeof(CardBase)
        || typeToConvert == typeof(ModuleBase)
        || typeToConvert == typeof(NodeBase);

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (typeToConvert == typeof(CardBase))
            return CardConverter.Instance;
        if (typeToConvert == typeof(ModuleBase))
            return ModuleConverter.Instance;
        if (typeToConvert == typeof(NodeBase))
            return NodeConverter.Instance;
        return null;
    }
}

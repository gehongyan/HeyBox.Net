using System.Text.Json.Serialization;

namespace HeyBox.API.Rest;

internal interface IPageInfoProvider
{
    int TotalCount { get; }
}

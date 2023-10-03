using System.Text.Json.Serialization;

namespace dotnet_oktober.Models
{
    public enum RpgClass
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        Knight = 1,
        Mage = 2,
        Cleric = 3
    }
}
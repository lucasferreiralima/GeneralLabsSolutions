using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GeneralLabSolutions.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        [EnumMember(Value = "alta")]
        alta,

        [EnumMember(Value = "media")]
        media,

        [EnumMember(Value = "baixa")]
        baixa
    }

}
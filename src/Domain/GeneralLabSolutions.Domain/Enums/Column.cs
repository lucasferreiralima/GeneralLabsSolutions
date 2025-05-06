using GeneralLabSolutions.Domain.Extensions;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GeneralLabSolutions.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Column
    {
        [EnumMember(Value = "todo")]
        todo,

        [EnumMember(Value = "review")]
        review,

        [EnumMember(Value = "progress")]
        progress,

        [EnumMember(Value = "done")]
        done
    }

}
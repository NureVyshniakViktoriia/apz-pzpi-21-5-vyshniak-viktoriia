using System.Runtime.Serialization;

namespace Common.Enums;
public enum CommandType
{
    [EnumMember(Value = "Configure")]
    ConfigurePetDevice,
    [EnumMember(Value = "GetCurrentTemperature")]
    GetCurrentTemperature,
    [EnumMember(Value = "GetAverageTemperature")]
    GetAverageTemperature,
    [EnumMember(Value = "GetCurrentLocation")]
    GetCurrentLocation,
    [EnumMember(Value = "ConfigureRFIDReader")]
    ConfigureRFIDReader,
}

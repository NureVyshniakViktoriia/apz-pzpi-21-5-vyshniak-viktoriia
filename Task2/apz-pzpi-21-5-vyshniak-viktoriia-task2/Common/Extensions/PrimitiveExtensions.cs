using System.ComponentModel;

namespace Common.Extensions;
public static class PrimitivesExtensions
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            return attribute.Description;

        throw new ArgumentException(string.Format(Resources.Resources.Get("ENUM_NOT_FOUND"), nameof(enumValue)));
    }
}

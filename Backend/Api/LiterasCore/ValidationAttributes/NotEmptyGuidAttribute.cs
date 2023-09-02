using System.ComponentModel.DataAnnotations;

namespace LiterasCore.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NotEmptyGuidAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is Guid guidValue)
        {
            return guidValue != Guid.Empty;
        }

        return false;
    }
}

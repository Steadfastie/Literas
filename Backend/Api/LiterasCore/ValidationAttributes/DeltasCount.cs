using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiterasCore.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DeltasCountAttribute : ValidationAttribute
{
    public DeltasCountAttribute(int minDeltasAmount)
    {
        MinDeltasAmount = minDeltasAmount;
    }

    private int MinDeltasAmount { get; }
    public int MaxDeltasAmount { get; set; } = int.MaxValue;

    public override bool IsValid(object? value)
    {
        switch (value)
        {
            case null:
                return false;
            case JsonDocument deltas:
                return CheckDeltasAmount(deltas.RootElement.Clone());
            default:
                ErrorMessage = "Quill deltas wrong type";
                return false;
        }
    }

    private bool CheckDeltasAmount(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                var ops = element.GetProperty("ops");
                var opsLength = ops.GetArrayLength();
                if (opsLength < MinDeltasAmount)
                {
                    ErrorMessage = $"Deltas amount is less than {MinDeltasAmount}";
                    return false;
                }

                if (opsLength <= MaxDeltasAmount)
                {
                    return true;
                }

                ErrorMessage = $"Deltas amount is more than {MaxDeltasAmount}";
                return false;

            }
            default:
                ErrorMessage = "Quill deltas structure is corrupt";
                return false;
        }
    }
}

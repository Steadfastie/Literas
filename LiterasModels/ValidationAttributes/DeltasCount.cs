using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LiterasModels.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
sealed class DeltasCountAttribute : ValidationAttribute
{
    public int MinDeltasAmount { get; }
    public int MaxDeltasAmount { get; set; } = int.MaxValue;
    public DeltasCountAttribute(int minDeltasAmount)
    {
        MinDeltasAmount = minDeltasAmount;
    }
    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        if (value is not JsonDocument deltas)
        {
            ErrorMessage = "Quill deltas wrong type";
            return false;
        }
        return CheckDeltasAmount(deltas.RootElement.Clone());
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

                if (opsLength > MaxDeltasAmount)
                {
                    ErrorMessage = $"Deltas amount is more than {MaxDeltasAmount}";
                    return false;
                }

                return true;
            }
            default:
                ErrorMessage = "Quill deltas structure is corrupt";
                return false;
        }
    }
}


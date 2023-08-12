using System.Reflection;
using LiterasCore.System;
using LiterasData.CQS;
using LiterasData.DTO;

namespace LiterasCore;

public static class PatchModelCreator<T> where T : IBaseDto
{
    public static List<PatchModel> Generate(T source, T changed, PropertyInfo[]? ignoreList = null)
    {
        if (source.GetType() != changed.GetType())
        {
            throw new ArgumentException("Type difference exception", nameof(changed));
        }

        var patchList = new List<PatchModel>();

        foreach (var property in source.GetType().GetProperties())
        {
            if (ignoreList?.Contains(property) == true)
            {
                continue;
            }

            if (property.GetValue(changed) != null &&
                !property.GetValue(changed)!.Equals(property.GetValue(source)))
            {
                patchList.Add(new PatchModel()
                {
                    PropertyName = property.Name,
                    PropertyValue = property.GetValue(changed)!
                });
            }
        }

        return patchList;
    }
}
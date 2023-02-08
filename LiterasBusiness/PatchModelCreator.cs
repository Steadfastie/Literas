using LiterasModels.Abstractions;
using LiterasModels.System;
using System.Reflection;

namespace LiterasBusiness;

public static class PatchModelCreator<T> where T : IBaseDto
{
    public static List<PatchModel> Generate(T source, T changed)
    {
        if (source.GetType() != changed.GetType())
        {
            throw new ArgumentException("Type difference exception", nameof(changed));
        }

        var patchList = new List<PatchModel>();

        foreach (PropertyInfo property in source.GetType().GetProperties())
        {
            if (property.Name == "Id")
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
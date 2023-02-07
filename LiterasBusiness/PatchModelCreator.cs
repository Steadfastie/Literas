using LiterasModels.Abstractions;
using LiterasModels.System;
using System.Reflection;

namespace LiterasBusiness;

public static class PatchModelCreator<T> where T : IBaseDto
{
    public static List<PatchModel> Generate(T source, T changed)
    {
        var patchList = new List<PatchModel>();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            if (property.GetValue(changed) != null &&
                property.GetValue(source) != null &&
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
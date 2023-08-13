using System.Reflection;
using LiterasData.CQS;
using LiterasData.DTO;
using LiterasData.Entities;

namespace LiterasData;

public static class PatchModelCreator<T> where T : IBaseEntity
{
    public static List<PatchModel> Generate(T source, T changed, PropertyInfo[]? ignoreList = null)
    {
        if (source.GetType() != changed.GetType())
        {
            throw new ArgumentException("Patching of different types is forbidden", nameof(changed));
        }

        var patchList = source.GetType().GetProperties()
            .Where(property => ignoreList?.Contains(property) != true)
            .Where(property =>
            {
                var changedValue = property.GetValue(changed);
                var sourceValue = property.GetValue(source);
                return changedValue != null && !changedValue.Equals(sourceValue);
            })
            .Select(property => new PatchModel
            {
                PropertyName = property.Name,
                PropertyValue = property.GetValue(changed)!
            })
            .ToList();

        return patchList;
    }
}

public static class PatchModelCreatorDto<T> where T : IBaseDto
{
    public static List<PatchModel> Generate(T source, T changed, PropertyInfo[]? ignoreList = null)
    {
        if (source.GetType() != changed.GetType())
        {
            throw new ArgumentException("Patching of different types is forbidden", nameof(changed));
        }

        var patchList = source.GetType().GetProperties()
            .Where(property => ignoreList?.Contains(property) != true)
            .Where(property =>
            {
                var changedValue = property.GetValue(changed);
                var sourceValue = property.GetValue(source);
                return changedValue != null && !changedValue.Equals(sourceValue);
            })
            .Select(property => new PatchModel
            {
                PropertyName = property.Name,
                PropertyValue = property.GetValue(changed)!
            })
            .ToList();

        return patchList;
    }
}

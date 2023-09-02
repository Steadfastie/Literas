using LiterasCore.System;
using LiterasData.DTO;
using LiterasData.Entities;

namespace LiterasCore.Abstractions;

public interface IEditorsService
{
    Task<bool> CanUserDo(Guid docId, List<EditorScope> scopes);
}

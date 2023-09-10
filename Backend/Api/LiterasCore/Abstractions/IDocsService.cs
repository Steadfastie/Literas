using LiterasData.DTO;
using LiterasData.Entities;

namespace LiterasCore.Abstractions;

public interface IDocsService
{
    Task<List<(DocDto doc, List<EditorScope> scopes, EditorStatus status)>> GetDocsAsync(
        CancellationToken cancellationToken = default);

    Task<(DocDto doc, List<EditorScope> scopes, EditorStatus status)> GetDocAsync(Guid docId,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateDocAsync(DocDto newDoc, CancellationToken cancellationToken = default);

    Task PatchDocAsync(DocDto changedDocDto, CancellationToken cancellationToken = default);

    Task DeleteDocAsync(Guid docId, CancellationToken cancellationToken = default);
}

using LiterasDataTransfer.Dto;
using LiterasModels.System;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IDocumentsService
{
    Task<int> CreateDocumentAsync(DocumentDto documentDto);

    Task<int> DeleteDocumentAsync(DocumentDto documentDto);

    Task<DocumentDto> GetDocumentByIdAsync(Guid documentId);

    Task<DocumentDto> GetDocumentByTitleAsync(string title);

    Task<int> PatchDocumentAsync(DocumentDto documentDto, List<PatchModel> patchlist);
}
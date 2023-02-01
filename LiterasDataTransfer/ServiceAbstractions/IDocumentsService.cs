using LiterasDataTransfer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IDocumentsService
{
    Task<int> PatchDocumentAsync(DocumentDTO documentDTO, List<PatchModel> patchlist);
}

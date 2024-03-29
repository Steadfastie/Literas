﻿using AutoMapper;
using LiterasCore.Abstractions;
using LiterasData.DTO;
using LiterasData.Exceptions;
using LiterasWebAPI.Auth;
using LiterasWebAPI.Models.Requests;
using LiterasWebAPI.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasWebAPI.Controllers;

[Route("docs")]
[ApiController]
[Authorize]
public class DocsController : ControllerBase
{
    private readonly IDocsService _docsService;
    private readonly IMapper _mapper;

    public DocsController(IDocsService docsService, IMapper mapper)
    {
        _docsService = docsService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get simplified doc representation
    /// </summary>
    [HttpGet(Name = nameof(GetDocs))]
    [Authorize(Policy = Policies.LiterasRead)]
    [ProducesResponseType(typeof(DocsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocs(CancellationToken cancellationToken = default)
    {
        var docData = await _docsService.GetDocsAsync(cancellationToken);

        var response = docData.ConvertAll(dataFound => new DocsResponse
        {
            Id = dataFound.doc.Id,
            Title = dataFound.doc.Title,
            Permissions = dataFound.scopes,
            Status = dataFound.status
        });

        return Ok(response);
    }

    /// <summary>
    /// Get one exact doc
    /// </summary>
    [HttpGet("{docId}", Name = nameof(Details))]
    [Authorize(Policy = Policies.LiterasRead)]
    [ProducesResponseType(typeof(DocResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Details([FromRoute] Guid docId, CancellationToken cancellationToken = default)
    {
        if (docId == Guid.Empty)
        {
            return BadRequest();
        }

        var (doc, scopes, status) = await _docsService.GetDocAsync(docId, cancellationToken);

        var response = new DocsResponse
        {
            Id = doc.Id, Title = doc.Title, Permissions = scopes, Status = status
        };

        return Ok(response);
    }

    /// <summary>
    /// Add new doc
    /// </summary>
    [HttpPost(Name = nameof(Create))]
    [Authorize(Policy = Policies.LiterasWrite)]
    [ProducesResponseType(typeof(DocResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] DocRequestModel docModel,
        CancellationToken cancellationToken = default)
    {
        var docDto = _mapper.Map<DocDto>(docModel);

        var docId = await _docsService.CreateDocAsync(docDto, cancellationToken);

        return Created(docId.ToString(), null);
    }

    /// <summary>
    /// Update doc
    /// </summary>
    [HttpPatch("{docId}", Name = nameof(Patch))]
    [Authorize(Policy = Policies.LiterasWrite)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public async Task<IActionResult> Patch(Guid docId, [FromBody] DocRequestModel docModel,
        CancellationToken cancellationToken = default)
    {
        if (docId != docModel.Id)
        {
            throw new GeneralException("Url ID does not correspond to model's one");
        }

        var docDto = _mapper.Map<DocDto>(docModel);

        await _docsService.PatchDocAsync(docDto, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// Remove doc
    /// </summary>
    [HttpDelete("{docId}", Name = nameof(Delete))]
    [Authorize(Policy = Policies.LiterasDelete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid docId, CancellationToken cancellationToken = default)
    {
        await _docsService.DeleteDocAsync(docId, cancellationToken);

        return Ok();
    }
}

using System.Security.Claims;
using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using LiterasWebAPI.Models.Requests;
using LiterasWebAPI.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasWebAPI.Controllers;

[Route("docs")]
[ApiController]
[Authorize(Policy = "literas")]
public class DocsController : ControllerBase
{
    private readonly IDocsService _docsService;
    private readonly IEditorsService _editorsService;
    private readonly IMapper _mapper;

    public DocsController(IDocsService docsService, IMapper mapper, IEditorsService editorsService)
    {
        _docsService = docsService;
        _mapper = mapper;
        _editorsService = editorsService;
    }

    [HttpGet("thumbnails")]
    [ProducesResponseType(typeof(DocThumbnailResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDocTumbnails()
    {
        var docDto = await _docsService.GetDocThumbnailsAsync();

        var responseModel = _mapper.Map<IEnumerable<DocThumbnailResponseModel>>(docDto.Results);

        return Ok(responseModel);
    }

    [HttpGet("{docId}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Details(Guid docId)
    {
        if (docId == Guid.Empty)
        {
            return BadRequest();
        }

        var docDto = await _docsService.GetDocByIdAsync(docId);

        if (docDto.Result == null || docDto.ResultStatus == OperationResult.Failure) return NotFound();

        var responseModel = _mapper.Map<DocResponseModel>(docDto.Result);

        return Ok(responseModel);
    }

    [HttpGet("{id}/editors")]
    [ProducesResponseType(typeof(List<DocResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllEditors(Guid docId)
    {
        //if (docId == Guid.Empty)
        //{
        //    return BadRequest();
        //}

        //var docDtos = await _editorsService.GetUsersByDocIdAsync(docId);

        //if (docDtos.Results == null || docDtos.ResultStatus == OperationResult.Failure) return NotFound();

        //var responseModels = _mapper.Map<List<UserResponseModel>>(docDtos.Results.ToList());

        //return Ok(responseModels);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DocRequestModel docModel)
    {
        var docDto = _mapper.Map<DocDto>(docModel);

        var docId = await _docsService.CreateDocAsync(docDto);
        
        return Created(docId.ToString(), null);
    }

    [HttpPatch("{docId}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DocRequestModel), StatusCodes.Status304NotModified)]
    public async Task<IActionResult> Patch(Guid docId, [FromBody] DocRequestModel docModel)
    {
        if (docId != docModel.Id) throw new GeneralException();

        var docDto = _mapper.Map<DocDto>(docModel);

        await _docsService.PatchDocAsync(docDto);

        return Ok();
    }

    [HttpDelete("{docId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid docId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        await _docsService.DeleteDocAsync(docId, Guid.TryParse(userId, out var creatorId) ? creatorId : Guid.Empty);

        return Ok();
    }
}

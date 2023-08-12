using System.Security.Claims;
using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.DTO;
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
        try
        {
            var docDto = await _docsService.GetDocThumbnailsAsync();

            if (docDto.Results == null || docDto.ResultStatus == OperationResult.Failure) return NotFound();

            var responseModel = _mapper.Map<IEnumerable<DocThumbnailResponseModel>>(docDto.Results);

            return Ok(responseModel);
        }
        catch (Exception ex)
        { 
            Log.Error($"!--- {ex.Message} ---! " + 
                $"{Environment.NewLine} {Environment.NewLine}" +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not find doc",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
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
        if (docId == Guid.Empty)
        {
            return BadRequest();
        }

        var docDtos = await _editorsService.GetUsersByDocIdAsync(docId);

        if (docDtos.Results == null || docDtos.ResultStatus == OperationResult.Failure) return NotFound();

        var responseModels = _mapper.Map<List<UserResponseModel>>(docDtos.Results.ToList());

        return Ok(responseModels);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DocRequestModel docModel)
    {
        var docDto = _mapper.Map<DocDto>(docModel);

        if (docDto.CreatorId == Guid.Empty)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            docDto.CreatorId = Guid.TryParse(userId, out var creatorId) ? creatorId : Guid.Empty;
        }

        var creationResult = await _docsService.CreateDocAsync(docDto);

        if (creationResult.ResultStatus == OperationResult.Failure)
        {
            return BadRequest("Could not save your doc");
        }

        var responseModel = _mapper.Map<DocResponseModel>(creationResult.Result);
        return Ok(responseModel);
    }

    [HttpPatch("{docId}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DocRequestModel), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(Guid docId, [FromBody] DocRequestModel docModel)
    {
        if (docId == Guid.Empty || docId != docModel.Id) return BadRequest();

        var docDto = _mapper.Map<DocDto>(docModel);

        var patchedResult = await _docsService.PatchDocAsync(docId, docDto);

        if (patchedResult.ResultStatus != OperationResult.Success)
        {
            return StatusCode(StatusCodes.Status304NotModified, docModel);
        }

        var responseModel = _mapper.Map<DocResponseModel>(patchedResult.Result);
        return Ok(responseModel);
    }

    [HttpDelete("{docId}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid docId)
    {
        if (docId == Guid.Empty) return BadRequest();

        var deleteResult = await _docsService.DeleteDocAsync(docId);

        if (deleteResult.ResultStatus == OperationResult.Failure)
        {
            return BadRequest("Operation unsuccessful");
        }

        var responseModel = _mapper.Map<DocResponseModel>(deleteResult.Result);
        return Ok(responseModel);
    }
}
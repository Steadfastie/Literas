using AutoMapper;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Requests;
using LiterasModels.Responses;
using LiterasModels.System;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasWebAPI.Controllers;

[Route("docs")]
public class DocsController : ControllerBase
{
    private readonly IDocsService _docsService;
    private readonly IEditorsService _contributorsService;
    private readonly IMapper _mapper;

    public DocsController(IDocsService docsService, IMapper mapper, IEditorsService contributorsService)
    {
        _docsService = docsService;
        _mapper = mapper;
        _contributorsService = contributorsService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid docId)
    {
        try
        {
            if (docId == Guid.Empty)
            {
                return BadRequest();
            }

            var docDto = await _docsService.GetDocByIdAsync(docId);

            if (docDto.Result == null) return NotFound();

            var responseModel = _mapper.Map<DocResponseModel>(docDto);

            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
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

    [HttpGet("{id}/editors")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEditors(Guid docId)
    {
        try
        {
            if (docId == Guid.Empty)
            {
                return BadRequest();
            }

            var docDto = await _contributorsService.GetUsersByDocIdAsync(docId);

            if (docDto == null) return NotFound();

            var responseModel = _mapper.Map<DocResponseModel>(docDto);

            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                      $"{Environment.NewLine} {Environment.NewLine} " +
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

    [HttpPost]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] DocRequestModel docModel)
    {
        try
        {
            var docDto = _mapper.Map<DocDto>(docModel);
            var creationResult = await _docsService.CreateDocAsync(docDto);

            if (creationResult.ResultStatus == OperationResult.Success)
            {
                var responseModel = _mapper.Map<DocResponseModel>(creationResult.Result);
                return Ok(responseModel);
            }
            else
            {
                return BadRequest("Could not register new doc");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not register new doc",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DocRequestModel), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(Guid docId, [FromBody] DocRequestModel docModel)
    {
        try
        {
            if (docId == Guid.Empty)
            {
                return BadRequest();
            }

            var docDto = _mapper.Map<DocDto>(docModel);
            var patchedResult = await _docsService.PatchDocAsync(docId, docDto);

            if (patchedResult.ResultStatus == OperationResult.Success)
            {
                var responseModel = _mapper.Map<DocResponseModel>(patchedResult.Result);
                return Ok(responseModel);
            }
            else
            {
                return StatusCode(StatusCodes.Status304NotModified, docModel);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not register new doc",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DocResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid docId)
    {
        try
        {
            if (docId == Guid.Empty)
            {
                return BadRequest();
            }

            var deleteResult = await _docsService.DeleteDocAsync(docId);

            if (deleteResult.ResultStatus == OperationResult.Success)
            {
                var responseModel = _mapper.Map<DocResponseModel>(deleteResult.Result);
                return Ok(responseModel);
            }
            else
            {
                return BadRequest("Operation unsuccessful");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not register new doc",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}
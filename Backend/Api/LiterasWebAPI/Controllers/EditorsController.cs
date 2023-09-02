using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.DTO;
using LiterasWebAPI.Models.Requests;
using LiterasWebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasWebAPI.Controllers;

[Route("editors")]
public class EditorsController : ControllerBase
{
    private readonly IEditorsService _editorsService;
    private readonly IMapper _mapper;

    public EditorsController(IEditorsService editorsService, IMapper mapper, IEditorsService contributorsService)
    {
        _editorsService = editorsService;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EditorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid editorId)
    {
        try
        {
            if (editorId == Guid.Empty)
            {
                return BadRequest();
            }

            var editorDto = await _editorsService.GetEditorByIdAsync(editorId);

            if (editorDto.Result == null || editorDto.ResultStatus == OperationResult.Failure) return NotFound();

            var responseModel = _mapper.Map<EditorResponse>(editorDto);

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
                Message = "Could not find editor",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(EditorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] EditorRequestModel editorModel)
    {
        try
        {
            var editorDto = _mapper.Map<EditorDto>(editorModel);

            if (editorDto.Id == Guid.Empty)
            {
                editorDto.Id = Guid.NewGuid();
            }

            var creationResult = await _editorsService.CreateEditorAsync(editorDto);

            if (creationResult.ResultStatus == OperationResult.Failure)
            {
                return BadRequest("Could not create new editor");
            }

            var responseModel = _mapper.Map<EditorResponse>(creationResult.Result);
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
                Message = "Could not create new editor",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(EditorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid editorId)
    {
        try
        {
            if (editorId == Guid.Empty) return BadRequest();

            var deleteResult = await _editorsService.DeleteEditorAsync(editorId);

            if (deleteResult.ResultStatus == OperationResult.Failure)
            {
                return BadRequest("Operation unsuccessful");
            }

            var responseModel = _mapper.Map<EditorResponse>(deleteResult.Result);
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
                Message = "Could not delete editor",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}
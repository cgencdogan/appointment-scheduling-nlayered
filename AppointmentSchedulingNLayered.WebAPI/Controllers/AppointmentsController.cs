using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Common.Results;
using AppointmentSchedulingNLayered.Common.DataTransferObjects;

namespace AppointmentSchedulingNLayered.WebAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase {
    private IAppointmentService _appointmentsService;
    private readonly IMapper _mapper;

    public AppointmentsController(IAppointmentService appointmentService, IMapper mapper) {
        _appointmentsService = appointmentService;
        _mapper = mapper;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<List<AppointmentDto>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> GetAll() {
        var result = await _appointmentsService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("GetById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<AppointmentDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> GetById(Guid appointmentId) {
        var result = await _appointmentsService.GetByIdAsync(appointmentId);
        return Ok(result);
    }

    [HttpGet("GetAllByPersonnelId")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<List<AppointmentDto>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> GetAllByPersonnelId(Guid personnelId) {
        var result = await _appointmentsService.GetAllByPersonelIdAsync(personnelId);
        return Ok(result);
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IDataResult<List<AppointmentDto>>))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> Add(AddAppointmentDto appointmentDto) {
        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _appointmentsService.AddAsync(appointmentDto);
        return CreatedAtAction(nameof(GetById), new { appointmentId = result.Value.Id }, result);
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDataResult<List<AppointmentDto>>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> Update(UpdateAppointmentDto appointmentDto) {
        var result = await _appointmentsService.UpdateAsync(appointmentDto);
        return Ok(result);
    }

    [HttpDelete("Delete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Common.Results.IResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
    public async Task<IActionResult> Delete(Guid appointmentId) {
        await _appointmentsService.DeleteAsync(appointmentId);
        return Ok();
    }
}

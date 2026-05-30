using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s32640.DTOs;
using PJATK_APBD_Cw8_s32640.Exceptions;
using PJATK_APBD_Cw8_s32640.Filters;
using PJATK_APBD_Cw8_s32640.Models;
using PJATK_APBD_Cw8_s32640.Services;

namespace PJATK_APBD_Cw8_s32640.Controllers;
[ApiController]
[Route("apo/[controller]")]
public class PatientsController(IHospitalService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PatientFilters filters, CancellationToken cancellationToken)
    {
        return Ok(await service.GetPatientsAsync(filters, cancellationToken));
    }

    [HttpPost("{pesel}/bedAssignments")]
    public async Task<IActionResult> AddBedAssignment(string pesel, BedAssignmentRequest bedAssignmentRequest, CancellationToken cancellationToken)
    {
        try
        {
            await service.AddBedAssignment(pesel,bedAssignmentRequest,cancellationToken);
            return Created();
        }catch(NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s32640.Filters;
using PJATK_APBD_Cw8_s32640.Models;
using PJATK_APBD_Cw8_s32640.Services;

namespace PJATK_APBD_Cw8_s32640.Controllers;
[ApiController]
[Route("apo/[controller]")]
public class HospitalController(IHospitalService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PatientFilters filters, CancellationToken cancellationToken)
    {
        return Ok(await service.GetPatientsAsync(filters, cancellationToken));
    }
}
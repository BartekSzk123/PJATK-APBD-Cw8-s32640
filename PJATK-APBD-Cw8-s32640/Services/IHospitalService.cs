using PJATK_APBD_Cw8_s32640.DTOs;
using PJATK_APBD_Cw8_s32640.Filters;
using PJATK_APBD_Cw8_s32640.Models;

namespace PJATK_APBD_Cw8_s32640.Services;

public interface IHospitalService
{
    Task<IEnumerable<PatientResponse>> GetPatientsAsync(PatientFilters patientFilters, CancellationToken cancellationToken);
    Task AddBedAssignment(string pesel,BedAssignmentRequest bedAssigmentRequest, CancellationToken cancellationToken);
}
using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s32640.DTOs;
using PJATK_APBD_Cw8_s32640.Filters;
using PJATK_APBD_Cw8_s32640.Infrastructure;
using PJATK_APBD_Cw8_s32640.Models;

namespace PJATK_APBD_Cw8_s32640.Services;

public class HospitalService(HospitalDbContext context) : IHospitalService
{
    public async Task<IEnumerable<PatientResponse>> GetPatientsAsync(PatientFilters patientFilters,
        CancellationToken cancellationToken)
    {
        return await context.Patients
            .Where(p => (patientFilters.Search == null 
                         || p.FirstName.Contains(patientFilters.Search) || p.LastName.Contains(patientFilters.Search)))
            .Select(patient => new PatientResponse(
                patient.Pesel,
                patient.FirstName,
                patient.LastName,
                patient.Age,
                patient.Sex,
                patient.Admissions.Select(admission => new AdmissionResponse(
                    admission.Id,
                    admission.AdmissionDate,
                    admission.DischargeDate,
                    new WardResponse(
                        admission.Ward.Id,
                        admission.Ward.Name,
                        admission.Ward.Description)
                )),
                patient.BedAssignments.Select(bedAssignment => new BedAssignmentResponse(
                    bedAssignment.Id,
                    bedAssignment.From,
                    bedAssignment.To,
                    new BedResponse(
                        bedAssignment.Bed.Id,
                        new BedTypeResponse(
                            bedAssignment.Bed.BedType.Id,
                            bedAssignment.Bed.BedType.Name,
                            bedAssignment.Bed.BedType.Description
                            ),
                        new RoomResponse(
                            bedAssignment.Bed.Room.Id,
                            bedAssignment.Bed.Room.HasTv,
                            new WardResponse(
                                bedAssignment.Bed.Room.Ward.Id,
                                bedAssignment.Bed.Room.Ward.Name,
                                bedAssignment.Bed.Room.Ward.Description)
                            ) 
                        )
                ))
            )).ToListAsync(cancellationToken);
    }
}
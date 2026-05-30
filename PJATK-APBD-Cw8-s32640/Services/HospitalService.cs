using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s32640.DTOs;
using PJATK_APBD_Cw8_s32640.Exceptions;
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

    public async Task AddBedAssignment(string pesel, BedAssignmentRequest bedAssigmentRequest,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var patient = await context.Patients.FirstOrDefaultAsync(p => p.Pesel == pesel, cancellationToken);
            if (patient == null)
            {
                throw new NotFoundException($"Patient {pesel} not found");
            }

            var bedType =
                await context.BedTypes.FirstOrDefaultAsync(b => b.Name == bedAssigmentRequest.BedType,
                    cancellationToken);
            if (bedType == null)
            {
                throw new NotFoundException($"BedType {bedAssigmentRequest.BedType} not found");
            }

            var ward = await context.Wards.FirstOrDefaultAsync(w => w.Name == bedAssigmentRequest.Ward,
                cancellationToken);
            if (ward == null)
            {
                throw new NotFoundException($"Ward {bedAssigmentRequest.Ward} not found");
            }

            var freeBed = await context.Beds
                .Include(b => b.Room)
                .Include(b => b.BedAssignments)
                .Where(b => b.BedTypeId == bedType.Id && b.Room.WardId == ward.Id)
                .FirstOrDefaultAsync(b => !b.BedAssignments
                    .Any(ba => ba.From < bedAssigmentRequest.To && ba.To > bedAssigmentRequest.From), cancellationToken);

            if (freeBed == null)
            {
                throw new NotFoundException( $"No available bed of type {bedAssigmentRequest.BedType} in ward {bedAssigmentRequest.Ward}");
            }

            var assignment = new BedAssignment
            {
                PatientPesel = patient.Pesel,
                BedId = freeBed.Id,
                From = bedAssigmentRequest.From,
                To = bedAssigmentRequest.To
            };
            
            context.BedAssignments.Add(assignment);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
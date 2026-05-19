namespace PJATK_APBD_Cw8_s32640.DTOs;

public record PatientResponse(
    string Pesel,
    string FirstName,
    string LastName,
    int Age,
    bool Sex,
    IEnumerable<AdmissionResponse> Admissions,
    IEnumerable<BedAssignmentResponse> BedAssignments
);

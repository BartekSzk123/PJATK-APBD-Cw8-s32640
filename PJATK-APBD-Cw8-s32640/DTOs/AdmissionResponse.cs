namespace PJATK_APBD_Cw8_s32640.DTOs;

public record AdmissionResponse(
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardResponse Ward
);
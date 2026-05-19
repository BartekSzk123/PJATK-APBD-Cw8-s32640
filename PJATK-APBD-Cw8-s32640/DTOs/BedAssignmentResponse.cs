namespace PJATK_APBD_Cw8_s32640.DTOs;

public record BedAssignmentResponse(
    int Id,
    DateTime From,
    DateTime? To,
    BedResponse Bed
);

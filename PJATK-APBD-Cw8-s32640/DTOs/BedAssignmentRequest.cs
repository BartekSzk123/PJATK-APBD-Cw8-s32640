using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Cw8_s32640.DTOs;

public record BedAssignmentRequest(
    [Required]
    DateTime From,
    DateTime? To,
    [Required, MinLength(1)]
    string BedType,
    [Required, MinLength(1)]
    string Ward
);

using PJATK_APBD_Cw8_s32640.Models;

namespace PJATK_APBD_Cw8_s32640.DTOs;

public record BedResponse(
    int Id,
    BedTypeResponse BedType,
    RoomResponse Room
);

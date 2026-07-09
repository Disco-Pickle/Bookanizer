using System.ComponentModel.DataAnnotations;
using Bookanizer.REST.Enums;

namespace Bookanizer.REST.API.DTOs;

public sealed record InteractionUpsertDto(
    [property: Required, Range(0, int.MaxValue)] int BookId,
    bool? IsRead,
    [property: Range(0, double.MaxValue)] double? Rating,
    DateTimeOffset? ReadAt,
    DateTimeOffset? StartedAt,
    ReadLocationEnum? ReadLocation);

using Bookanizer.REST.Enums;

namespace Bookanizer.REST.API.DTOs;

public sealed record InteractionUpsertDto(
    int BookId,
    bool? IsRead,
    double? Rating,
    DateTimeOffset? ReadAt,
    DateTimeOffset? StartedAt,
    ReadLocationEnum? ReadLocation);

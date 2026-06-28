using Bookanizer.REST.Enums;

namespace Bookanizer.REST.API.DTOs;

public sealed record InteractionReadDto(
    BookReadDto Book,
    bool? IsRead,
    double? Rating,
    DateTimeOffset? DateAdded,
    DateTimeOffset? DateUpdated,
    DateTimeOffset? ReadAt,
    DateTimeOffset? StartedAt,
    ReadLocationEnum? ReadLocation);

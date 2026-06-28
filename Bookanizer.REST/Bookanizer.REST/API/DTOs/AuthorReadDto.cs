namespace Bookanizer.REST.API.DTOs;

public sealed record AuthorReadDto(
    int AuthorId,
    string? Name);

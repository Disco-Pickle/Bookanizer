using System.ComponentModel.DataAnnotations;

namespace Bookanizer.REST.API.DTOs;

public sealed record RegisterRequestDto(
    [property: Required, StringLength(64, MinimumLength = 6)] string Username,
    [property: Required, StringLength(64, MinimumLength = 8)] string Password);

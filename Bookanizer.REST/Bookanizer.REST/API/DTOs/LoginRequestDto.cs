using System.ComponentModel.DataAnnotations;

namespace Bookanizer.REST.API.DTOs;

public sealed record LoginRequestDto(
    [property: Required] string Username,
    [property: Required] string Password);

namespace Bookanizer.REST.API.DTOs;

public sealed record LoginRequestDto(
    string Username,
    string Password);

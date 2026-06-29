namespace Bookanizer.REST.API.DTOs;

public sealed record RegisterRequestDto(
    string Username,
    string Password);

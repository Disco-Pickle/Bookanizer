namespace Bookanizer.REST.API.DTOs;

public sealed record BookReadDto(
    int BookId,
    string? Isbn,
    string? Isbn13,
    string? CountryCode,
    string? LanguageCode,
    double? AverageRating,
    int? RatingsCount,
    AuthorReadDto Author,
    int? NumPages,
    DateOnly? PublicationDate,
    string Title);

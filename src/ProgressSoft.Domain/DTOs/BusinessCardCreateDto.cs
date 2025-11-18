using System;

namespace ProgressSoft.Domain.DTOs;


/// <summary>
/// A pure Data Transfer Object (DTO) that represents the data required 
/// to create a new business card. This is the common contract
/// used by the API, Parsers, and Application Commands.
/// It lives in the Domain layer so all other layers can reference it.
/// </summary>
public sealed record BusinessCardCreateDto(
    string Name,
    string Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    string Address,
    string? PhotoBase64
);

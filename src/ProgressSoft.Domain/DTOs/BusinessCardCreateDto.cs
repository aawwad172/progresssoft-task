namespace ProgressSoft.Domain.DTOs;


/// <summary>
/// A pure Data Transfer Object (DTO) that represents the data required 
/// to create a new business card. This is the common contract
/// used by the API, Parsers, and Application Commands.
/// It lives in the Domain layer so all other layers can reference it.
/// </summary>
public sealed class BusinessCardCreateDto
{
    public BusinessCardCreateDto()
    {
    }

    public BusinessCardCreateDto(string name, string gender, DateTime dateOfBirth, string email, string phone, string address, string? photoBase64)
    {
        Name = name;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Email = email;
        Phone = phone;
        Address = address;
        PhotoBase64 = photoBase64;
    }

    public required string Name { get; set; }
    public required string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public string? PhotoBase64 { get; set; }
}

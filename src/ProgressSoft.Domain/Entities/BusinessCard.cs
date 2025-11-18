using ProgressSoft.Domain.Interfaces.Domain.Auditing;

namespace ProgressSoft.Domain.Entities;

public class BusinessCard : IBaseEntity
{
    public Guid Id { get; init; }

    // Required fields
    public required string Name { get; set; }
    public required string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public string? PhotoBase64 { get; set; }
    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

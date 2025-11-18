using System;

using FluentValidation;

using ProgressSoft.Presentation.API.Models;

namespace ProgressSoft.Presentation.API.Validators.Commands.BusinessCards;

public class FileImportCommandValidator : AbstractValidator<FileImportCommand>
{
    private readonly string[] AllowedExtensions = [".csv", ".xml", ".png", ".jpg", ".jpeg"];

    public FileImportCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
            .Must(f => f.Length <= 10 * 1024 * 1024) // 10 MB
                .WithMessage("File size cannot exceed 10 MB.")
            .Must(f => AllowedExtensions.Contains(Path.GetExtension(f.FileName).ToLowerInvariant()))
                .WithMessage($"Only {string.Join(", ", AllowedExtensions)} files are allowed.");
    }
}

using MediatR;

using ProgressSoft.Domain.Enums;

namespace ProgressSoft.Application.CQRS.Queries;

public sealed record ExportBusinessCardQuery(Guid Id, FileFormatEnum Format) : IRequest<ExportBusinessCardQueryResult>;

public sealed record ExportBusinessCardQueryResult(
    byte[] FileContents,
    string ContentType,
    string FileName
);
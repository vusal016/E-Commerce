namespace Ordering.Application.Common.Dtos;
public sealed record ReorderResultDto(bool Success, IEnumerable<string> Warnings);

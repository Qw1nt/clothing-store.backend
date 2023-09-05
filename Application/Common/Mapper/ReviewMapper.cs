using Application.Reviews.Commands;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mapper;

[Mapper]
public static partial class ReviewMapper
{
    public static partial Review ToReview(this CreateReviewCommand command);
}
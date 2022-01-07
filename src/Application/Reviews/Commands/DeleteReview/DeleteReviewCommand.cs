using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest<Review>
{
    public int Id { get; set; }
}

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Review>
{
    private readonly IApplicationDbContext _context;

    public DeleteReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Review> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Reviews.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }

        _context.Reviews.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
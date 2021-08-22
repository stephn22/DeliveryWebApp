using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Reviews.Queries.GetReviews;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Extensions
{
    public static class RestaurateurExtensions
    {
        public static async Task<Restaurateur> GetRestaurateurAsync(this IApplicationDbContext context,
            int? restaurateurId)
        {
            try
            {
                if (restaurateurId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Restaurateurs.Where(r => r.Id == restaurateurId).FirstAsync();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static async Task<Restaurateur> GetRestaurateurByCustomerIdAsync(this IApplicationDbContext context,
            int? customerId)
        {
            try
            {
                if (customerId == null)
                {
                    throw new NullReferenceException();
                }

                return await context.Restaurateurs.Where(r => r.Customer.Id == customerId).FirstAsync();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static async Task<Restaurateur> GetRestaurateurByApplicationUserFkAsync(
            this IApplicationDbContext context, string applicationUserFk)
        {
            try
            {
                var customer = await context.Customers.FirstAsync(c => c.ApplicationUserFk == applicationUserFk);
                var restaurateur = await context.Restaurateurs.FirstAsync(r => r.CustomerId == customer.Id);

                return restaurateur;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public static async Task<double> GetRestaurateurAverageRating(this Restaurateur restaurateur, IMediator mediator)
        {
            var avg = 0.0;
            var sum = 0.0;

            try
            {
                var reviews = await mediator.Send(new GetReviewsQuery
                {
                    RestaurateurId = restaurateur.Id
                });

                if (reviews is not { Count: > 0 })
                {
                    return avg;
                }

                sum = reviews.Aggregate(sum, (current, review) => current + review.Rating);

                avg = sum / reviews.Count;

                return avg;
            }
            catch (InvalidOperationException)
            {
                return avg;
            }
        }
    }
}

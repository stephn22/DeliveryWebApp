using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Reviews.Commands.CreateReview;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Reviews.Commands;

using static Testing;

public class CreateReviewTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateReviewCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateReviewTestAsync()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateCustomerCommand
        {
            ApplicationUserFk = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@gmail.com"
        };

        var customer = await SendAsync(command);

        var user2 = await RunAsUserAsync("mariorossi@gmail.com", "Qwerty12!", Array.Empty<string>());

        var customer2 = await SendAsync(new CreateCustomerCommand
        {
            ApplicationUserFk = user2,
            FirstName = "Mario",
            LastName = "Rossi",
            Email = "mariorossi@gmail.com"
        });

        var restaurateur = await SendAsync(new CreateRestaurateurCommand
        {
            Customer = customer2
        });

        var addressCommand = new CreateAddressCommand
        {
            Latitude = 48.5472M,
            Longitude = 72.1804M,
            CustomerId = customer.Id
        };

        var reviewCommand = new CreateReviewCommand
        {
            Customer = customer,
            Restaurateur = restaurateur,
            Rating = 4,
            Title = "Lorem Ipsum",
            Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                   "Praesent eleifend metus id justo dignissim imperdiet."
        };

        var review = await SendAsync(reviewCommand);

        review.Should().NotBeNull();
        review.Id.Should().BeGreaterThan(0);
        review.CustomerId.Should().Be(customer.Id);
        review.RestaurateurId.Should().Be(restaurateur.Id);
        review.Title.Should().Be(reviewCommand.Title);
        review.Text.Should().Be(reviewCommand.Text);
    }
}
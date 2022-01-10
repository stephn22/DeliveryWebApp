using DeliveryWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class EmailSenderController : ControllerBase
{
    private readonly EmailSender _emailSender;

    public EmailSenderController(EmailSender emailSender)
    {
        _emailSender = emailSender;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task SendEmail(string email)
    {
        await _emailSender.SendEmailAsync(
            email,
            "Confirm your email",
            "Please confirm yout account" // TODO: callback url
        );
    }
}
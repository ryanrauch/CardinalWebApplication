using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Extensions;
using CardinalWebApplication.Models.DbContext;
using CardinalWebApplication.Services;
using CardinalWebApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Registration")]
    public class RegistrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public RegistrationController(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        // POST: api/Registration
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostRegistration([FromBody] UserInfoContract info)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser
            {
                UserName = info.UserName,
                Email = info.Email,
                DateOfBirth = info.DateOfBirth,
                FirstName = info.FirstName,
                LastName = info.LastName,
                PhoneNumber = info.PhoneNumber.RemoveNonNumeric(),
                Gender = info.Gender,
                AccountType = AccountType.Regular
            };
            //TODO: remove duplication between this and
            //      AccountController::Register(...)
            var result = await _userManager.CreateAsync(user, info.Password);
            if(result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password from RegistrationController.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);

                return Ok(true);
            }
            return Ok(false);
        }
    }
}
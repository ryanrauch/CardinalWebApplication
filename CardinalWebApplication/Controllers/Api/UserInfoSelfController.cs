using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using CardinalWebApplication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/UserInfoSelf")]
    [Authorize]
    public class UserInfoSelfController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoSelfController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET api/UserInfoSelf
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var target = _httpContextAccessor.CurrentUserId();
            var user = await _context.ApplicationUsers
                                     .SingleOrDefaultAsync(s => s.Id.Equals(target));
            if (user == null)
            {
                return NotFound();
            }
            var contract = new UserInfoContract()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                AccountType = user.AccountType,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return Ok(contract);
        }
    }
}
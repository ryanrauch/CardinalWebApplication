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
    [Route("api/UserInfo")]
    [Authorize]
    public class UserInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET api/UserInfo
        [HttpGet]
        public async Task<IActionResult> GetAllUsersInfo()
        {
            var target = _httpContextAccessor.CurrentUserId();
            var users = await _context.FriendRequests
                                      .Where(f => f.TargetId.Equals(target))
                                      .Join(
                                            inner: _context.ApplicationUsers,
                                            outerKeySelector: f => f.InitiatorId,
                                            innerKeySelector: a => a.Id,
                                            resultSelector: (f, a) => new UserInfoContract
                                            {
                                                Id = a.Id,
                                                UserName = a.UserName,
                                                FirstName = a.FirstName,
                                                LastName = a.LastName,
                                                DateOfBirth = a.DateOfBirth,
                                                Gender = a.Gender,
                                                AccountType = a.AccountType,
                                                PhoneNumber  = a.PhoneNumber,
                                                Email = a.Email
                                            })
                                            .ToListAsync();
            if(users == null || users.Count == 0)
            {
                return NoContent();
            }
            return Ok(users);
        }

        // GET api/UserInfo/5abc-d4ef-...
        [HttpGet("id")]
        public async Task<IActionResult> GetUserInfo([FromRoute] string id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var target = _httpContextAccessor.CurrentUserId();
            var friendPermission = await _context.FriendRequests
                                                 .SingleOrDefaultAsync(s => s.TargetId.Equals(target)
                                                                            && s.InitiatorId.Equals(id));
            if (friendPermission == null)
            {
                return NotFound();
            }
            var user = await _context.ApplicationUsers
                                     .SingleOrDefaultAsync(s => s.Id.Equals(id));
            if(user == null)
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
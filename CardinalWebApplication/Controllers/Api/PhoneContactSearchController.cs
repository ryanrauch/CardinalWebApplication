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
    [Route("api/PhoneContactSearch")]
    [Authorize]
    public class PhoneContactSearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PhoneContactSearchController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/PhoneContactSearch/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoneContactRequest([FromRoute] string id)
        {
            //this takes a 10-digit phone number
            //numeric values only (512)555-1234
            //would be sent as "5125551234"
            //result is either a boolean value of false, and null username
            //or a value of true, and the username associated with phone number
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id.Length != 10)
            {
                return BadRequest(id);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            var userExists = await _context.ApplicationUsers
                               .AnyAsync(a => a.Id.Equals(userId));
            if(!userExists)
            {
                return BadRequest();
            }
            var phoneExists = await _context.ApplicationUsers
                                            .AnyAsync(a => a.PhoneNumber.Equals(id));
            if(!phoneExists)
            {
                return Ok(new PhoneContactSearchContract()
                                {
                                    Found = false,
                                    UserName = String.Empty
                                });
            }
            var phoneUsername = await _context.ApplicationUsers.FirstAsync(a => a.PhoneNumber.Equals(id));
            var res = new PhoneContactSearchContract()
                            {
                                Found = true,
                                UserName = phoneUsername.UserName
                            };
            return Ok(res);
        }
    }
}
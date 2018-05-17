using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary;
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
    [Route("api/MockData")]
    [Authorize]
    public class MockDataController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public MockDataController(
            IServiceProvider serviceProvider,
            ApplicationDbContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        // POST: api/MockData
        [HttpPost]
        public async Task<IActionResult> PostLocationHistory([FromBody] MockDataInitializeContract data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(a => a.Email.Equals(data.Email, StringComparison.OrdinalIgnoreCase));
            if(user == null)
            {
                return NotFound(data.Email);
            }
            if(!user.Id.Equals(userId))
            {
                return Unauthorized();
            }
            if(user.AccountType != AccountType.MockedData)
            {
                return Unauthorized();
            }
            var dataseed = new MockDataInitializer();
            await dataseed.InitializeMockUsers(_serviceProvider, data);
            return Ok();
        }

    }
}
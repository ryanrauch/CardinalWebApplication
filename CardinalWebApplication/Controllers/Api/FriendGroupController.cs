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
    [Route("api/FriendGroup")]
    [Authorize]
    public class FriendGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendGroupController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/FriendGroup
        [HttpGet]
        public async Task<IActionResult> GetAllFriendGroups()
        {
            var userId = _httpContextAccessor.CurrentUserId();
            var groups = await _context.FriendGroups
                                       .GroupJoin(
                                            inner: _context.FriendGroupUsers,
                                            outerKeySelector: g => g.ID,
                                            innerKeySelector: u => u.GroupID,
                                            resultSelector: (g, u) => new FriendGroupContract()
                                            {
                                                ID = g.ID.ToString(),
                                                Description = g.Description,
                                                UserID = g.UserID,
                                                Users = u.Select(s => new FriendGroupUserContract()
                                                {
                                                    ID = s.ID.ToString(),
                                                    FriendID = s.FriendID,
                                                    GroupID = s.GroupID.ToString()
                                                })
                                            }).ToListAsync();
            if(groups == null || groups.Count == 0)
            {
                return NoContent();
            }
            return Ok(groups);
        }
    }
}
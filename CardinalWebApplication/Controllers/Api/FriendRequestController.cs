using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using CardinalWebApplication.Extensions;
using CardinalWebApplication.Models.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/FriendRequest")]
    [Authorize]
    public class FriendRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendRequestController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/FriendRequest
        [HttpGet]
        public async Task<IActionResult> GetAllFriendRequests()
        {
            var userId = _httpContextAccessor.CurrentUserId();
            var friends = await _context.FriendRequests
                                        .Where(f => (f.InitiatorId.Equals(userId)
                                                     || f.TargetId.Equals(userId))
                                                 && !(f.TargetId.Equals(userId)
                                                      && f.Type.HasValue
                                                      && f.Type.Value.Equals(FriendRequestType.Blocked)))
                                        .Select(s => new FriendRequestContract()
                                        {
                                            InitiatorId = s.InitiatorId,
                                            TargetId = s.TargetId,
                                            TimeStamp = s.TimeStamp,
                                            Type = s.Type
                                        })
                                        .ToListAsync();
            if (friends == null || friends.Count == 0)
            {
                return NoContent();
            }
            return Ok(friends);
        }

        // GET: api/FriendRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriendRequest([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            if (userId.Equals(id))
            {
                return BadRequest(id);
            }
            var friendRequest = await _context.FriendRequests
                                              .Where(f => (f.InitiatorId.Equals(id)
                                                           && f.TargetId.Equals(userId)
                                                           && !(f.Type.HasValue && f.Type.Value.Equals(FriendRequestType.Blocked)))
                                                       || (f.TargetId.Equals(id)
                                                           && f.InitiatorId.Equals(userId)))
                                              .Select(s => new FriendRequestContract()
                                              {
                                                  InitiatorId = s.InitiatorId,
                                                  TargetId = s.TargetId,
                                                  TimeStamp = s.TimeStamp,
                                                  Type = s.Type
                                              })
                                              .ToListAsync();
            if (friendRequest == null)
            {
                return NoContent();
            }
            return Ok(friendRequest);
        }

        // POST: api/FriendRequest
        [HttpPost]
        public async Task<IActionResult> PostFriendRequest([FromBody] FriendRequest friendRequest)
        {
            //TODO: create DataContract
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            if (!friendRequest.InitiatorId.Equals(userId))
            {
                return Unauthorized();
            }
            if (friendRequest.InitiatorId.Equals(friendRequest.TargetId))
            {
                return BadRequest(friendRequest.TargetId);
            }
            var userExists = await _context.ApplicationUsers
                                           .AnyAsync(a => a.Id.Equals(friendRequest.TargetId));
            var blocked = await _context.FriendRequests
                                        .AnyAsync(f => f.InitiatorId.Equals(friendRequest.TargetId)
                                                       && f.TargetId.Equals(userId)
                                                       && f.Type.HasValue
                                                       && f.Type.Value.Equals(FriendRequestType.Blocked));
            if (!userExists || blocked)
            {
                return BadRequest(friendRequest.TargetId);
            }
            var friend = await _context.FriendRequests
                                       .SingleOrDefaultAsync(f => f.InitiatorId.Equals(userId)
                                                                  && f.TargetId.Equals(friendRequest.TargetId));
            DateTime timeStamp = DateTime.Now.ToUniversalTime();
            if (friend != null)
            {
                friend.Type = friendRequest.Type;
                friend.TimeStamp = timeStamp;
            }
            else
            {
                friendRequest.TimeStamp = timeStamp;
                _context.FriendRequests.Add(friendRequest);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFriendRequest", new { id = friendRequest.TargetId }, friendRequest);
        }

        // DELETE: api/FriendRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendRequest([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            if (userId.Equals(id))
            {
                return BadRequest(id);
            }
            var friendRequest = await _context.FriendRequests
                                              .SingleOrDefaultAsync(m => m.InitiatorId.Equals(userId)
                                                                         && m.TargetId.Equals(id));
            if (friendRequest == null)
            {
                return NoContent();
            }
            _context.FriendRequests.Remove(friendRequest);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
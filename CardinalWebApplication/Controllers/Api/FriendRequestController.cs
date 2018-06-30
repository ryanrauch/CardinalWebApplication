using CardinalLibrary;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using CardinalWebApplication.Extensions;
using CardinalWebApplication.Models.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> PostFriendRequest([FromBody] FriendRequestContract friendRequestContract)
        {
            //TODO: create DataContract
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            if (!friendRequestContract.InitiatorId.Equals(userId))
            {
                return Unauthorized();
            }
            if (friendRequestContract.InitiatorId.Equals(friendRequestContract.TargetId))
            {
                return BadRequest(friendRequestContract.TargetId);
            }
            var userExists = await _context.ApplicationUsers
                                           .AnyAsync(a => a.Id.Equals(friendRequestContract.TargetId));
            var blocked = await _context.FriendRequests
                                        .AnyAsync(f => f.InitiatorId.Equals(friendRequestContract.TargetId)
                                                       && f.TargetId.Equals(userId)
                                                       && f.Type.HasValue
                                                       && f.Type.Value.Equals(FriendRequestType.Blocked));
            if (!userExists || blocked)
            {
                return BadRequest(friendRequestContract.TargetId);
            }
            var friend = await _context.FriendRequests
                                       .SingleOrDefaultAsync(f => f.InitiatorId.Equals(userId)
                                                                  && f.TargetId.Equals(friendRequestContract.TargetId));
            DateTime timeStamp = DateTime.Now.ToUniversalTime();
            if (friend != null)
            {
                friend.Type = friendRequestContract.Type;
                friend.TimeStamp = timeStamp;
            }
            else
            {
                FriendRequest fr = new FriendRequest()
                {
                    InitiatorId = userId,
                    TargetId = friendRequestContract.TargetId,
                    TimeStamp = timeStamp,
                    Type = friendRequestContract.Type
                };
                await _context.FriendRequests.AddAsync(fr);
            }
            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetFriendRequest", new { id = friendRequestContract.TargetId }, friendRequest);
            return Ok();
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
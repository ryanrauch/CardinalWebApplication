using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/LocationHistory")]
    [Authorize]
    public class LocationHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationHistoryController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/LocationHistory
        [HttpGet]
        public async Task<IActionResult> GetAllLocationHistory()
        {
            //TODO: create DataContract
            var target = _httpContextAccessor.CurrentUserId();
            var history = await _context.LocationHistories
                                        .Where(h => h.UserId.Equals(target))
                                        .ToListAsync();
            if (history.Count == 0)
            {
                return NoContent();
            }
            return Ok(history);
        }

        // GET: api/LocationHistory/5dab-cf3e-...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationHistory([FromRoute] Guid id)
        {
            //TODO: create DataContract
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            var locationHistory = await _context.LocationHistories
                                                .SingleOrDefaultAsync(m => m.HistoryID.ToString().Equals(id)
                                                                           && m.UserId.Equals(userId));
            if (locationHistory == null)
            {
                return NotFound();
            }
            return Ok(locationHistory);
        }

        // POST: api/LocationHistories
        [HttpPost]
        public async Task<IActionResult> PostLocationHistory([FromBody] LocationHistory locationHistory)
        {
            //TODO: create DataContract PostLocationHistory
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _httpContextAccessor.CurrentUserId();
            if (!locationHistory.UserId.Equals(userId))
            {
                return Unauthorized();
            }
            locationHistory.TimeStamp = DateTime.Now.ToUniversalTime();
            //TODO: use ILocationHistoryService
            await _context.LocationHistories
                          .AddAsync(locationHistory);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLocationHistory", new { id = locationHistory.HistoryID }, locationHistory);
        }
    }
}
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using CardinalWebApplication.Extensions;
using CardinalWebApplication.Models.DbContext;
using CardinalWebApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/UserLocation")]
    [Authorize]
    public class UserLocationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHexagonal _hexagonal;
        private readonly ILocationHistoryService _locationHistoryService;
        private readonly IZoneBoundaryService _zoneBoundaryService;

        public UserLocationController(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IHexagonal hexagonal,
            ILocationHistoryService locationHistoryService,
            IZoneBoundaryService zoneBoundaryService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hexagonal = hexagonal;
            _locationHistoryService = locationHistoryService;
            _zoneBoundaryService = zoneBoundaryService;
        }

        // GET: api/UserLocation
        [HttpGet]
        public async Task<IActionResult> GetAllUserLocations()
        {
            var target = _httpContextAccessor.CurrentUserId();
            var options = await _context.ApplicationOptions
                                        .OrderByDescending(a => a.OptionsDate)
                                        .FirstOrDefaultAsync();
            var layers = await _context.FriendRequests
                                       .Where(f => f.TargetId.Equals(target))
                                       .Join(
                                             inner: _context.CurrentLayers,
                                             outerKeySelector: f => f.InitiatorId,
                                             innerKeySelector: c => c.UserId,
                                             resultSelector: (f, c) => new CurrentLayerContract
                                             {
                                                 UserId = c.UserId,
                                                 TimeStamp = c.TimeStamp.ToUniversalTime(),
                                                 LayersDelimited = c.LayersDelimited,
                                                 CurrentZoneId = c.CurrentZoneId
                                             })
                                       .Where(d => d.TimeStamp >= (DateTime.Now.ToUniversalTime().Subtract(options.DataTimeWindow)))
                                       .ToListAsync();
            if (layers == null || layers.Count == 0)
            {
                return NoContent();
            }
            return Ok(layers);
        }

        // GET: api/UserLocation/5abc-d3e...
        [HttpGet("id")]
        public async Task<IActionResult> GetUserLocation([FromRoute] string id)
        {
            if (!ModelState.IsValid)
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
            var options = await _context.ApplicationOptions
                                        .OrderByDescending(a => a.OptionsDate)
                                        .FirstOrDefaultAsync();
            var layer = await _context.CurrentLayers
                                      .SingleOrDefaultAsync(s => s.UserId.Equals(id)
                                                                 && s.TimeStamp >= DateTime.Now.ToUniversalTime().Subtract(options.DataTimeWindow));
            if (layer == null)
            {
                return NoContent();
            }
            var contract = new CurrentLayerContract()
                            {
                                UserId = layer.UserId,
                                TimeStamp = layer.TimeStamp,
                                LayersDelimited = layer.LayersDelimited,
                                CurrentZoneId = layer.CurrentZoneId
                            };
            return Ok(contract);
        }

        // POST: api/UserLocation
        [HttpPost]
        public async Task<IActionResult> PostCurrentLocation([FromBody] CurrentLocationPost currentLocationPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var timeStamp = DateTime.Now.ToUniversalTime();
            var gid = _httpContextAccessor.CurrentUserId();
            var appUser = await _context.ApplicationUsers
                                        .SingleOrDefaultAsync(a => a.Id.Equals(gid));
            if (appUser == null)
            {
                return NotFound(gid);
            }
            await _locationHistoryService.CreateLocationHistoryAsync(gid, 
                                                                     currentLocationPost.Latitude, 
                                                                     currentLocationPost.Longitude, 
                                                                     timeStamp);

            _hexagonal.Initialize(currentLocationPost.Latitude, 
                                  currentLocationPost.Longitude, 
                                  _hexagonal.Layers[0]);
            String layers = _hexagonal.AllLayersDelimited();
            String currentZone = await _zoneBoundaryService.IsCoordinateInsideZone(currentLocationPost.Latitude, 
                                                                                   currentLocationPost.Longitude);
            var currentLayer = await _context.CurrentLayers
                                             .FirstOrDefaultAsync(c => c.UserId.Equals(gid));
            if (currentLayer == null)
            {
                await _context.CurrentLayers.AddAsync(new CurrentLayer()
                {
                    UserId = gid,
                    LayersDelimited = layers,
                    TimeStamp = timeStamp,
                    CurrentZoneId = currentZone
                });
            }
            else
            {
                currentLayer.LayersDelimited = layers;
                currentLayer.TimeStamp = timeStamp;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
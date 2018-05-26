using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;
using CardinalWebApplication.Data;
using CardinalWebApplication.Models.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CardinalWebApplication.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Zone")]
    [Authorize]
    public class ZoneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZoneController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Zone
        [HttpGet]
        public async Task<IActionResult> GetAllZones()
        {
            var zones = await _context.Zones
                                      .GroupJoin(
                                            inner: _context.ZoneShapes,
                                            outerKeySelector: z => z.ZoneID,
                                            innerKeySelector: s => s.ParentZoneId,
                                            resultSelector: (z, s) => new ZoneContract()
                                            {
                                                ZoneID = z.ZoneID.ToString(),
                                                Description = z.Description,
                                                Type = z.Type,
                                                ARGBFill = z.ARGBFill,
                                                VisibleLayersDelimited = z.VisibleToLayersDelimited,
                                                ZoneShapes = s.Select(h=> new ZoneShapeContract()
                                                                     {
                                                                        ZoneShapeID = h.ZoneShapeID.ToString(),
                                                                        Latitude = h.Latitude,
                                                                        Longitude = h.Longitude,
                                                                        Order = h.Order
                                                                     })
                                            }).ToListAsync();
            if(zones == null || zones.Count == 0)
            {
                return NoContent();
            }
            return Ok(zones);
        }
    }
}
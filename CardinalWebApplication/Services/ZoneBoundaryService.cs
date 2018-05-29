using CardinalLibrary;
using CardinalWebApplication.Data;
using CardinalWebApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardinalWebApplication.Services
{
    public class ZoneBoundaryService : IZoneBoundaryService
    {
        private readonly ApplicationDbContext _context;

        public ZoneBoundaryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsEmptyZone(Guid zId)
        {
            return zId.Equals(Guid.Empty);
        }

        public async Task<Guid> IsCoordinateInsideZone(ZoneType zType, double latitude, double longitude)
        {
            var zonesWithType = await _context.Zones
                                           .Where(z => z.Type.Equals(zType))
                                           .ToListAsync();
            if(zonesWithType == null || zonesWithType.Count == 0)
            {
                return Guid.Empty;
            }
            foreach(var zone in zonesWithType)
            {
                if(PointRoughlyInsidePolygon(zone.ZoneID, latitude, longitude))
                {
                    if(PointInsidePolygon(zone.ZoneID, latitude, longitude))
                    {
                        return zone.ZoneID;
                    }
                }
            }
            return Guid.Empty;
            //if(roughZones == null || roughZones.Count == 0)
            //{
            //    return Guid.Empty;
            //}
            //foreach (var rough in roughZones)
            //{
            //    var zone = await _context.Zones
            //                             .Where(z => z.ZoneID.Equals(rough.ZoneID)
            //                                         && PointInsidePolygon(z.ZoneID, latitude, longitude))
            //                             .FirstOrDefaultAsync();
            //    if (zone != null)
            //    {
            //        return zone.ZoneID;
            //    }
            //}
            //return Guid.Empty;
        }

        public bool PointRoughlyInsidePolygon(Guid zId, double latitude, double longitude)
        {
            var positions = _context.ZoneShapes
                                    .Where(z => z.ParentZoneId.Equals(zId))
                                    .ToList();
            return latitude <= positions.Max(p => p.Latitude)
                   && latitude >= positions.Min(p => p.Latitude)
                   && longitude <= positions.Max(p => p.Longitude)
                   && longitude >= positions.Min(p => p.Longitude);
        }

        public bool PointInsidePolygon(Guid zId, double latitude, double longitude)
        {
            //http:--//paulbourke.net/geometry/polygonmesh/#insidepoly
            //https:--//wrf.ecse.rpi.edu//Research/Short_Notes/pnpoly.html
            var positions = _context.ZoneShapes
                                    .Where(z => z.ParentZoneId.Equals(zId))
                                    .OrderBy(z => z.Order)
                                    .ToList();
            //TODO: MUST DISTINGUISH EACH POLYGON, AND/OR HOLES CONTAINED WITHIN
            int npol = positions.Count;
            bool c = false;
            for (int i = 0, j = npol - 1; i < npol; j = i++)
            {
                if ((((positions[i].Longitude <= longitude) && (longitude < positions[j].Longitude))
                    || ((positions[j].Longitude <= longitude) && (longitude < positions[i].Longitude)))
                    && (latitude < (positions[j].Latitude - positions[i].Latitude) * (longitude - positions[i].Longitude) / (positions[j].Longitude - positions[i].Longitude) + positions[i].Latitude))
                {
                    c = !c;
                }
            }
            return c;
        }
    }
}

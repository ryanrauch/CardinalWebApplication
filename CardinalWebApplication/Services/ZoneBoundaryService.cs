using CardinalWebApplication.Data;
using CardinalWebApplication.Services.Interfaces;
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

        public async Task<string> IsCoordinateInsideZone(double latitude, double longitude)
        {
            await Task.Delay(50);
            return null;
        }
    }
}

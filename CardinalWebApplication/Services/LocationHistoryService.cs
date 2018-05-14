using CardinalWebApplication.Data;
using CardinalWebApplication.Models.DbContext;
using CardinalWebApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CardinalWebApplication.Services
{
    public class LocationHistoryService : ILocationHistoryService
    {
        private readonly ApplicationDbContext _context;
        public LocationHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateLocationHistoryAsync(string userId, double latitude, double longitude, DateTime timeStamp)
        {
            LocationHistory history = new LocationHistory()
            {
                UserId = userId,
                Latitude = latitude,
                Longitude = longitude,
                TimeStamp = timeStamp
            };
            await _context.LocationHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllLocationHistoryAsync(string userId)
        {
            var history = await _context.LocationHistories
                                        .Where(h => h.UserId.Equals(userId))
                                        .ToListAsync();
            _context.LocationHistories.RemoveRange(history);
            await _context.SaveChangesAsync();
        }
    }
}

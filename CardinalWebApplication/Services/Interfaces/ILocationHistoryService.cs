using System;
using System.Threading.Tasks;

namespace CardinalWebApplication.Services.Interfaces
{
    public interface ILocationHistoryService
    {
        Task CreateLocationHistoryAsync(string userId, double latitude, double longitude, DateTime timeStamp);
        Task DeleteAllLocationHistoryAsync(string userId);
    }
}
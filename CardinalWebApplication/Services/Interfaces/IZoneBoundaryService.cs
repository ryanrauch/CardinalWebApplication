using CardinalLibrary;
using System;
using System.Threading.Tasks;

namespace CardinalWebApplication.Services.Interfaces
{
    public interface IZoneBoundaryService
    {
        Task<Guid> IsCoordinateInsideZone(ZoneType zType, double latitude, double longitude);
        bool IsEmptyZone(Guid zId);
    }
}
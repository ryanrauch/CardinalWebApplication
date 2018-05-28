using System.Threading.Tasks;

namespace CardinalWebApplication.Services.Interfaces
{
    public interface IZoneBoundaryService
    {
        Task<string> IsCoordinateInsideZone(double latitude, double longitude);
    }
}
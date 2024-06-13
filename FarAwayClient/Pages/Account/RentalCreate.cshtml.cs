using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using FarAwayClient.Tools.DbEnums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FarAwayClient.Pages.Account
{
    [CustomAuthorize]
    public class RentalCreateModel : PageModel
    {
        private readonly Db _context;
        private readonly Geocoder _geocoder;
        private readonly IConfiguration _configuration;
        public RentalCreateModel(Db context, Geocoder geocoder, IConfiguration configuration, ILogger<RentalCreateModel> logger)
        {
            _context = context;
            _geocoder = geocoder;
            _configuration = configuration;
        }
        public IList<Branch> Branches { get; set; }
        public string ApiKey { get; private set; }
        public async Task OnGetAsync()
        {
            Branches = await _context.Branches.Where(b => b.ParkingSpots.Where(s => s.IdParkingSpotStatus == ParkingSpotsStatuses.Available).Count() >= 1).ToListAsync();
            ApiKey = _configuration["YandexMaps:ApiKey"];
        }
        public async Task<string> GetCoordinatesAsync(string address)
        {
            (double longitude, double latitude) = await _geocoder.GetCoordinatesAsync(address);
            return $"{longitude} {latitude}".Replace(',', '.');
        }
    }
}

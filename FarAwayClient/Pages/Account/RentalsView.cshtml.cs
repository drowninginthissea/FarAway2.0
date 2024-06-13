using FarAwayClient.Models;
using FarAwayClient.Services;
using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;

namespace FarAwayClient.Pages.Account
{
    [CustomAuthorize]
    public class RentalsViewModel : PageModel
    {
        private readonly Db _context;
        private readonly Geocoder _geocoder;
        private readonly IConfiguration _configuration;
        public RentalsViewModel(Db context, Geocoder geocoder, IConfiguration configuration)
        {
            _context = context;
            _geocoder = geocoder;
            _configuration = configuration;
        }
        public IList<ParkingSpaceRental> Rents { get; set; }
        public string ApiKey { get; private set; }
        public async Task OnGetAsync()
        {
            User CurrentUser = await _context.Users.FindAsync(HttpContext.Session.GetInt32(Literals.UserSessionKey));
            Rents = CurrentUser.ParkingSpaceRentals.ToList();
            ApiKey = _configuration["YandexMaps:ApiKey"];
        }
        public IActionResult OnGetQRCode(int rentId)
        {
            using QRCodeGenerator generator = new QRCodeGenerator();
            using QRCodeData data = generator.CreateQrCode(rentId.ToString(), QRCodeGenerator.ECCLevel.Q);
            using Base64QRCode code = new Base64QRCode(data);
            Base64QRCode.ImageType imgType = Base64QRCode.ImageType.Png;
            string image = code.GetGraphic(20, Color.Black, Color.White, true, imgType);
            string imgSrc = $"data:image/{imgType.ToString().ToLower()};base64,{image}";
            return new JsonResult(new { qrCodeImage = imgSrc });
        }
        public async Task<string> GetCoordinatesAsync(string address)
        {
            (double longitude, double latitude) = await _geocoder.GetCoordinatesAsync(address);
            return $"{longitude} {latitude}".Replace(',', '.');
        }
    }
}

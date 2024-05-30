using FarAwayClient.Models;
using FarAwayClient.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FarAwayClient.Pages.Account
{
    public class CheckoutInputModel
    {
        public int Id { get; set; }

        public int CountOfDays { get; set; }

        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "���� ������ ������ �� �������!")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "���� ��������� ������ �� �������!")]
        public DateOnly? EndDate { get; set; }

        [Required(ErrorMessage = "��� ������ �� ������������ �� ��������!")]
        [Range(1, 3, ErrorMessage = "��� ������ �� ������������ ��������� �������!")]
        public int TypeOfRent { get; set; }

        
        [Required(ErrorMessage = "��������� ������� ������ ������!")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "��� ��������� ����� �� �������!")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "����� ����� �� �����!")]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "������������ ����� �����!")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "���� �������� ����� �� �����")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{4}|[0-9]{2})$", ErrorMessage = "������������ ���� �������� �����!")]
        public string CardExpiration { get; set; }

        [Required(ErrorMessage = "CVV ��� �� �����!")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "������������ CVV ���!")]
        public string CardCVV { get; set; }
    }

    [CustomAuthorize]
    public class CheckOutModel : PageModel
    {
        public Db Context;
        public CheckOutModel(Db context)
        {
            Context = context;
        }
        private Branch CurrentBranch { get; set; }
        private ParkingSpot SpotForRenting { get; set; }
        [BindProperty]
        public CheckoutInputModel Input { get; set; }
        public IActionResult OnGet(int id)
        {
            IEnumerable<int> AvailableIds = Context.Branches.Where(b => b.ParkingSpots.Where(s => s.IdParkingSpotStatus == ParkingSpotsStatuses.Available).Count() >= 1).Select(b => b.Id).ToList();
            if (!AvailableIds.Any(x => x == id))
            {
                return NotFound();
            }

            CurrentBranch = Context.Branches.Single(b => b.Id == id);
            SpotForRenting = CurrentBranch.ParkingSpots.Where(s => s.IdParkingSpotStatus == ParkingSpotsStatuses.Available).First();
            ViewData["id"] = id;
            ViewData["CostPerDay"] = CurrentBranch.BranchCharacteristic.TheCostOfAparkingSpacePerDay;
            ViewData["Address"] = CurrentBranch.Address;
            return Page();
        }

        public IActionResult OnGetRentDetails(int id, int duration)
        {
            if (duration <= 0)
            {
                return new JsonResult(null);
            }

            var rentType = Context.TypeOfRentByDurations.Single(t => t.MinDurationOfRentalDays <= duration && (t.MaxDurationOfRentalDays == null || t.MaxDurationOfRentalDays >= duration));

            if (rentType != null)
            {
                decimal pricePerDay = Context.Branches.Single(b => b.Id == id).BranchCharacteristic.TheCostOfAparkingSpacePerDay;
                decimal totalPrice = duration * pricePerDay * rentType.PriceCoefficient;

                pricePerDay = Math.Round(pricePerDay, 2);
                totalPrice = Math.Round(totalPrice, 2);

                return new JsonResult(new
                {
                    duration,
                    priceCoefficient = rentType.PriceCoefficient,
                    pricePerDay,
                    totalPrice,
                    typeOfRentId = rentType.Id
                });
            }
            return new JsonResult(null);
        }

        public IActionResult OnPost(int id)
        {
            CurrentBranch = Context.Branches.Single(b => b.Id == id);
            if (!ModelState.IsValid)
            {
                ViewData["id"] = id;
                ViewData["CostPerDay"] = CurrentBranch.BranchCharacteristic.TheCostOfAparkingSpacePerDay;
                ViewData["Address"] = CurrentBranch.Address;
                return Page();
            }
            return RedirectToPage("/Account/Profile");
        }
    }
}

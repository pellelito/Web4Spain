using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Web4Spain.Models;

namespace Web4Spain.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;

        public IndexModel(
            UserManager<ApplicationUserModel> userManager,
            SignInManager<ApplicationUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Street { get; set; }
            public string StreetNumber { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }

        }

        private async Task LoadAsync(ApplicationUserModel user)
        {

            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //var userId = await _userManager.GetUserIdAsync(user);


            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                StreetNumber = user.StreetNumber,
                ZipCode = user.ZipCode,
                City = user.City


            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            user.PhoneNumber = Input.PhoneNumber;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Street = Input.Street;
            user.StreetNumber = Input.StreetNumber;
            user.ZipCode = Input.ZipCode;
            user.City = Input.City;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update info.";
                return RedirectToPage();
            }

            StatusMessage = "Your profile has been updated";
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}

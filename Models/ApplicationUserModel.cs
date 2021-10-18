using Microsoft.AspNetCore.Identity;

namespace Web4Spain.Models

{
    public class ApplicationUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public string Phone { get; set; }

    }
}

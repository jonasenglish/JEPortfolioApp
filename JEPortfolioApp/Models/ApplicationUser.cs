namespace JEPortfolioApp.Models
{
    internal class ApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; internal set; }
    }
}
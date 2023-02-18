using System.ComponentModel.DataAnnotations;

namespace PrsBackEnd.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [StringLength(30)]
        public string Username { get; set; }

        [StringLength(30)]
        public string Password { get; set; }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(12)]
        public string? Phone { get; set; }

        [StringLength(255)]     // ex. 123-555-4466
        public string? Email { get; set; }

        public bool IsReviewer { get; set; }

        public bool IsAdmin { get; set; }

    }
}

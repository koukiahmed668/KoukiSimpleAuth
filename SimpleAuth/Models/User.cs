using System.ComponentModel.DataAnnotations;

namespace SimpleAuth.Models
{
    public class User : IUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)] 
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [EmailAddress] 
        public string? Email { get; set; } 

        [StringLength(50)] 
        public string? FirstName { get; set; }

        [StringLength(50)] 
        public string? LastName { get; set; } 

    }
}

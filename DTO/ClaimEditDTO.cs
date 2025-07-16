using System.ComponentModel.DataAnnotations;

namespace IntricomMVC.DTO
{
    public class ClaimEditDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}

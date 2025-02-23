using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PhotoTable
    {
        [Key]
        [Required]
        [MaxLength(18)]
        public string IDCardNumber { get; set; }

        [Required]
        public byte[] Photo { get; set; }
    }
}
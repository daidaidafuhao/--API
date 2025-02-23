using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class ImportHistory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(18)]
        public string IDCardNumber { get; set; }

        [Required]
        public string ImportCount { get; set; }

        [Required]
        public string ImportTime { get; set; }

        [ForeignKey("IDCardNumber")]
        public Employee? Employee { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class TrainingRecord
    {
        public int Id { get; set; }

        [Required]
        public int SerialNumber { get; set; }

        [Required]
        public DateTime TrainingDate { get; set; }

        [Required]
        public string TrainingContent { get; set; }

        [Required]
        public string TrainingUnit { get; set; }

        [Required]
        public string TrainingLocation { get; set; }

        public string? Assessment { get; set; }

        public decimal? Cost { get; set; }

        public string? Remarks { get; set; }

        [Required]
        public String EmployeeId { get; set; }

        public Employee? Employee { get; set; }
    }
}
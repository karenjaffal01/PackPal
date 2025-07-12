using System.ComponentModel.DataAnnotations;

namespace PackPal.ViewModels
{
    public class TripViewModel
    {
        [Key]
        public int TripId { get; set; }

        [Required]
        [StringLength(100)]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
    }
}

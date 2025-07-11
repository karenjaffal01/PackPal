using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackPal.Models
{
    public class Trip
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

        [Required]
        public string UserId { get; set; } // This will store the user's Id from AspNetUsers,ef will bind this to Users Id from identity 

        [ForeignKey("UserId")]
        public Users User { get; set; } // Navigation property to access user info


    }
}

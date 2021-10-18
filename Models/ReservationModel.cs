using System;
using System.ComponentModel.DataAnnotations;

namespace Web4Spain.Models
{
    public class ReservationModel
    {
        [Required]
        public int HousingId { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int UserId { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ReservationStart { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public int ReservationEnd { get; set; }
    }
}

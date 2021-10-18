using System;
using System.ComponentModel.DataAnnotations;


namespace Web4Spain.Models
{
    public class BookingModel
    {

        [Key]
        public Guid BookingId { get; set; }

        public int HousingId { get; set; }

        public int Status { get; set; }

        public string UserId { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ReservationStart { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ReservationEnd { get; set; }

    }
}

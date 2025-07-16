using System;
using System.Collections.Generic;

namespace IntricomMVC.Models;

public partial class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<HotelBooking> HotelBookings { get; set; } = new List<HotelBooking>();
}

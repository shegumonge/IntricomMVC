using System;
using System.Collections.Generic;

namespace IntricomMVC.Models;

public partial class HotelBooking
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreadeDate { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Hotel Hotel { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace IntricomMVC.Models;

public partial class ClientDTO
{

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime CreateDate { get; set; }

}

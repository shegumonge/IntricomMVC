using System;
using System.Collections.Generic;

namespace IntricomMVC.Models;

public partial class Configuration
{
    public int Id { get; set; }

    public string DataType { get; set; } = null!;

    public string FsFolder { get; set; } = null!;
}

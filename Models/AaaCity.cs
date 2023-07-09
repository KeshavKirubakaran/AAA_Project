using System;
using System.Collections.Generic;

namespace AAA_Project.Models;

public partial class AaaCity
{
    public int Id { get; set; }

    public string? CityName { get; set; }

    public virtual ICollection<AaaSalesInfo> AaaSalesInfos { get; set; } = new List<AaaSalesInfo>();
}

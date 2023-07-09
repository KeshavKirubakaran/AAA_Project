using System;
using System.Collections.Generic;

namespace AAA_Project.Models;

public partial class AaaMobileModel
{
    public int Id { get; set; }

    public string? ModelName { get; set; }

    public int? Amount { get; set; }

    public virtual ICollection<AaaSalesInfo> AaaSalesInfos { get; set; } = new List<AaaSalesInfo>();
}

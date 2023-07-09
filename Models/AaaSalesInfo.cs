using System;
using System.Collections.Generic;

namespace AAA_Project.Models;

public partial class AaaSalesInfo
{
    public int Id { get; set; }

    public int? CityId { get; set; }

    public int? ModelId { get; set; }

    public DateTime? DateOfSales { get; set; }

    public int? SalesCount { get; set; }

    public virtual AaaCity? City { get; set; }

    public virtual AaaMobileModel? Model { get; set; }
}

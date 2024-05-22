using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class InfiniaWebReportGroup
{
    public int AutoId { get; set; }

    public string? GroupName { get; set; }

    public string? ColorValue { get; set; }

    public int? DisplayOrderId { get; set; }

    public int? SecurityLevel { get; set; }

    public string? Svg { get; set; }
}

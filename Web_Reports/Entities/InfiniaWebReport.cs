using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class InfiniaWebReport
{
    public int AutoId { get; set; }

    public int? ReportId { get; set; }

    public int? GroupId { get; set; }

    public string? ReportName { get; set; }

    public string? ReportType { get; set; }

    public string? ShowDesktop { get; set; }

    public string? ShowMobile { get; set; }

    public int? DisplayOrderId { get; set; }

    public string? SecurityLevel { get; set; }

    public string? CustomUrl { get; set; }

    public string? CustomUrlMvc { get; set; }

    public string? FilterType { get; set; }

    public string? Query { get; set; }
}

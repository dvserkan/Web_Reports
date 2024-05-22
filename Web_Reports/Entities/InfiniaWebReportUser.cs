using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class InfiniaWebReportUser
{
    public int AutoId { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public string? NameSurname { get; set; }

    public string? İmageUrl { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Role { get; set; }
}

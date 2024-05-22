using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class OrderHeader
{
    public int? OrderId { get; set; }

    public Guid? OrderKey { get; set; }

    public int? OrderStatus { get; set; }

    public DateTime? OrderDateTime { get; set; }

    public double? AmountDue { get; set; }

    public double? SubTotal { get; set; }

    public double? DiscountLineAmount { get; set; }

    public double? DiscountCashAmount { get; set; }

    public double? DiscountOrderAmount { get; set; }

    public double? DiscountTotalAmount { get; set; }

    public bool? LineDeleted { get; set; }

    public string? DeleteReason { get; set; }
}

using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class OrderTransaction
{
    public Guid? TransactionKey { get; set; }

    public Guid? OrderKey { get; set; }

    public DateTime? TransactionDateTime { get; set; }

    public Guid? MenuItemKey { get; set; }

    public string? MenuItemText { get; set; }

    public string? AccountingCode { get; set; }

    public double? ExtendedPrice { get; set; }

    public double? DiscountLineAmount { get; set; }

    public double? DiscountCashAmount { get; set; }

    public double? DiscountTotalAmount { get; set; }

    public double? TaxPercent { get; set; }

    public bool? OrderByWeight { get; set; }

    public bool? LineDeleted { get; set; }

    public string? DeleteReason { get; set; }
}

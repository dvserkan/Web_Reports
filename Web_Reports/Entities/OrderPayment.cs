using System;
using System.Collections.Generic;

namespace Web_Reports.Entities;

public partial class OrderPayment
{
    public Guid? PaymentKey { get; set; }

    public Guid? OrderKey { get; set; }

    public DateTime? PaymentDateTime { get; set; }

    public string? PaymentMethodName { get; set; }

    public string? GlobalBankName { get; set; }

    public double? AmountPaid { get; set; }

    public bool? LineDeleted { get; set; }

    public string? DeleteReason { get; set; }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Web_Reports.Entities;

namespace Web_Reports.Contexts;

public partial class ReportsContext : DbContext
{
    public ReportsContext()
    {
    }

    public ReportsContext(DbContextOptions<ReportsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InfiniaWebReport> InfiniaWebReports { get; set; }

    public virtual DbSet<InfiniaWebReportCategory> InfiniaWebReportCategories { get; set; }

    public virtual DbSet<InfiniaWebReportGroup> InfiniaWebReportGroups { get; set; }

    public virtual DbSet<InfiniaWebReportUser> InfiniaWebReportUsers { get; set; }

    public virtual DbSet<OrderHeader> OrderHeaders { get; set; }

    public virtual DbSet<OrderPayment> OrderPayments { get; set; }

    public virtual DbSet<OrderTransaction> OrderTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D3UUTH7\\POSSQL;Initial Catalog=Reports;User ID=sa;Password=sql123_;trustservercertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InfiniaWebReport>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__infiniaW__6B23296506C90339");

            entity.ToTable("infiniaWebReports");

            entity.Property(e => e.AutoId).HasColumnName("AutoID");
            entity.Property(e => e.CustomUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomUrlMvc)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DisplayOrderId).HasColumnName("DisplayOrderID");
            entity.Property(e => e.FilterType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ReportName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReportType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SecurityLevel)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShowDesktop)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShowMobile)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InfiniaWebReportCategory>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__infiniaW__6B232965A68B47EB");

            entity.ToTable("infiniaWebReportCategory");

            entity.Property(e => e.AutoId).HasColumnName("AutoID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InfiniaWebReportGroup>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__infiniaW__6B232965EA614099");

            entity.ToTable("infiniaWebReportGroups");

            entity.Property(e => e.AutoId).HasColumnName("AutoID");
            entity.Property(e => e.ColorValue)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DisplayOrderId).HasColumnName("DisplayOrderID");
            entity.Property(e => e.GroupName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Svg)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InfiniaWebReportUser>(entity =>
        {
            entity.HasKey(e => e.AutoId).HasName("PK__infiniaW__6B232965EBADC14C");

            entity.ToTable("infiniaWebReportUsers");

            entity.Property(e => e.AutoId).HasColumnName("AutoID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NameSurname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.İmageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderHeader>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.OrderDateTime, "UIX_OrderHeaders_OrderDateTime");

            entity.HasIndex(e => e.OrderKey, "UIX_OrderHeaders_OrderKey");

            entity.Property(e => e.DeleteReason).HasMaxLength(500);
            entity.Property(e => e.OrderDateTime).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.OrderKey, "UIX_OrderPayments_OrderKey");

            entity.HasIndex(e => e.PaymentDateTime, "UIX_OrderPayments_PaymentDateTime");

            entity.Property(e => e.DeleteReason).HasMaxLength(500);
            entity.Property(e => e.GlobalBankName).HasMaxLength(200);
            entity.Property(e => e.PaymentDateTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethodName).HasMaxLength(500);
        });

        modelBuilder.Entity<OrderTransaction>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.OrderKey, "UIX_OrderTransactions_OrderKey");

            entity.HasIndex(e => e.TransactionDateTime, "UIX_OrderTransactions_TransactionDateTime");

            entity.Property(e => e.AccountingCode).HasMaxLength(150);
            entity.Property(e => e.DeleteReason).HasMaxLength(500);
            entity.Property(e => e.MenuItemText).HasMaxLength(500);
            entity.Property(e => e.TransactionDateTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

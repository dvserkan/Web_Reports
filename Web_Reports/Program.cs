using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Web_Reports.Contexts;
using Web_Reports.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();  //Session Ayaða Kaldýrma
builder.Services.AddSession();               //Session Ayaða Kaldýrma
builder.Services.AddHttpContextAccessor();   //Session Ayaða Kaldýrma


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())  //Session Ayaða Kaldýrma
{
    app.UseDeveloperExceptionPage();
}
app.UseSession();  //Session Ayaða Kaldýrma


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

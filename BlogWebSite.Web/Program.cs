using BlogWebSite.Data.Extensions;
using BlogWebSite.Service.Extensions;
using BlogWebSite.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebLayerExtension();
builder.Services.AddDataLayerExtension(builder.Configuration);
builder.Services.AddServiceLayerExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseWebLayerExtension();

app.Run();

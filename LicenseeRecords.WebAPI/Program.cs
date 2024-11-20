using System.Text.Json.Serialization;
using LicenseeRecords.WebAPI.Interfaces;
using LicenseeRecords.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var productsFilePath = builder.Configuration["ProductsJOSNFilePath"];
var accountsFilePath = builder.Configuration["AccountsJOSNFilePath"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>(provider => new ProductRepository(productsFilePath));
builder.Services.AddScoped<IAccountRepository, AccountRepository>(provider => new AccountRepository(accountsFilePath));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseDeveloperExceptionPage();

app.Run();

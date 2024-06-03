using Domain.DTOs.EmailDTOs;
using Infrastructure.Data;
using Infrastructure.Mapper;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApp.ExtensionMethods.AuthConfigurations;
using WebApp.ExtensionMethods.RegisterService;
using WebApp.ExtensionMethods.SwaggerConfigurations;

var builder = WebApplication.CreateBuilder(args);

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);
builder.Services.AddRegisterService(builder.Configuration);


builder.Services.SwaggerService();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddAuthConfigureService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.Initial();
}
catch (Exception)
{
}


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
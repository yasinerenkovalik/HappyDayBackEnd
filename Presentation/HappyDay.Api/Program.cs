using FluentValidation;
using FluentValidation.AspNetCore;
using HappyDay.Api.Extension;
using HappyDay.Application;
using HappyDay.Application.Validations.Company;
using HappyDay.Persistance;
using HappyDay.Persistance.Context;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot" // BU SATIR KRİTİK
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddFluentValidation(conf=>conf.RegisterValidatorsFromAssemblyContaining<CompanyCreateValidator>());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HappyDayContext>();

builder.Services.AddPersistanceLayerServices();
builder.Services.AddAplicationLayerServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

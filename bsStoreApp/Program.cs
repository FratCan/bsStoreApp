using Repositories.EFCore;
using Microsoft.EntityFrameworkCore;
using bsStoreApp.Extensions;
using NLog;
using Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

var builder = WebApplication.CreateBuilder(args);


LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nLog.config"));
// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
} 
)
    .AddCustomCsvFormatter()
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    .AddNewtonsoftJson();
//kayd�n� sonradan ServiceExtensiona ekledi�im i�in buray� yorum sat�r�na ald�m.
//builder.Services.AddScoped<ValidationFilterAttribute>(); //AddScoped her kullan�c� i�in ayr� bir nesne �retir
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlConnect(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureNLogService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();

var app = builder.Build();

var logger=app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if(app.Environment.IsProduction())
{
    app.UseHsts();  
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

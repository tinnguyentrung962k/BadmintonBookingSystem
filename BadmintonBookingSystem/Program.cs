using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using BadmintonBookingSystem.Configuration;
using Serilog;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateBuilder(args);

// Configure logging with Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var config = builder.Configuration;

// Add services to the container
builder.Services.AddSecurityConfiguration(config);
builder.Services.AddDatabaseConfiguration(config);
builder.Services.AddRepositoryConfiguration();
builder.Services.AddServiceConfiguration(config);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddJwtAuthenticationService(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

var app = builder.Build();

// Use Serilog for request logging
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed identity data if needed
app.SeedIdentity();
app.UseSecurityConfiguration();
app.MapControllers();

app.Run();

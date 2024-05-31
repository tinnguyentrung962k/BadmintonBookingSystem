using BadmintonBookingSystem.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Logging.AddSerilog();


// Add services to the container.
builder.Services.AddSecurityConfiguration(config);
builder.Services.AddDatabaseConfiguration(config);
builder.Services.AddRepositoryConfiguration();
builder.Services.AddServiceConfiguration(config);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Host.UseSerilog((ctx, config) =>
{
    config.WriteTo.Console().MinimumLevel.Information();
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.SeedIdentity();
app.UseSecurityConfiguration();
app.MapControllers();

app.Run();
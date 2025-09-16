using VisionaryAnalytics.Worker.Application.Extensions;
using VisionaryAnalytics.Worker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

// Add services to the container.
builder.Services.AddInternalServices();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();
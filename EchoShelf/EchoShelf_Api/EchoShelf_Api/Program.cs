var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://*:5000");
    app.MapOpenApi();

    app.UseSwaggerUI(a => a.SwaggerEndpoint("openapi/v1.json", "v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();

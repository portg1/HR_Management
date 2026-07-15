using HR_Management.Application;
using HR_Management.Infrastructure;
using HR_Management.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.AddDbContext<LeaveManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LeaveManagementConnectionString")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "HR Management API", 
        Version = "v1",
        Description = "API for HR Management system"
    });
});
builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",b => 
        b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Management API V1");
        c.RoutePrefix = "swagger"; // Optional: sets the URL to access Swagger UI
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();

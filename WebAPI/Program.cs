using Application;
using Common;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using TechnicalTest;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Infrastructure.Contexts.ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
#endregion

#region Register module services.
Boostraper boostraper = new Boostraper(builder.Services);
boostraper.SetConfiguration(builder.Configuration);
boostraper.RegisterModule<WebAPIModule>();
boostraper.RegisterModule<ApplicationModule>();
boostraper.RegisterModule<DomainModule>();
boostraper.RegisterModule<InfrastructureModule>();
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();

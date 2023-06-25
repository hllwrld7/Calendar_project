using CalendarAPI;
using CalendarAPI.Interfaces;
using CalendarAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IAppointmentManagementService, AppointmentManagementService>();
builder.Services.AddSingleton<IContactManagementService, ContactManagementService>();
builder.Services.AddSingleton<ISQLiteService, SQLiteService>();
builder.Services.AddSingleton<ISchedulingService, SchedulingService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var startup = new Startup();
startup.Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

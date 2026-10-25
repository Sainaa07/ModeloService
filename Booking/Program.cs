using Modules.Booking;
using Modules.Identity;
using Modules.Identity.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// module registration
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddBookingModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);
//controllers
builder.Services.AddControllers().AddApplicationPart(typeof(Modules.Identity.Controllers.UsersController).Assembly);//zalguulaad bichnee
//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<IdentityDbContext>("identity")
    .AddDbContextCheck<BookingDbContext>("booking")
    .AddDbContextCheck<NotificationsDbContext>("notifications");
var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    bool connected = await db.Database.CanConnectAsync();
    Console.WriteLine(
        connected
            ? "PostgreSQL connected successfully!"
            : "PostgreSQL connection failed!"
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//middleware
app.UseHttpsRedirection();
app.UseAuthorization();

//endpoints
app.MapControllers();
app.MapHealthChecks("/health");

//run app
app.Run();

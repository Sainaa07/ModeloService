using Booking.Infrastructure.Swagger;
using Modules.Booking;
using Modules.Identity;
using Modules.Notifications;
using SharedKernel.Tenancy;

var builder = WebApplication.CreateBuilder(args);

// Modules
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddBookingModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);

// Tenant context (scoped per request)
builder.Services.AddScoped<ITenantContext, TenantContext>();

// Controllers — бүх модулийн assembly нэмнэ
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Modules.Identity.Controllers.PublicController).Assembly)
    .AddApplicationPart(typeof(Modules.Booking.DependencyInjection).Assembly)
    .AddApplicationPart(typeof(Modules.Notifications.DependencyInjection).Assembly);

// OpenAPI / Swagger
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.OperationFilter<TenantHeaderOperationFilter>());

// Health check endpoint (модулиуд өөрсдийн DbContext-ийн check-ийг DI-дaa нэмсэн)
builder.Services.AddHealthChecks();

var app = builder.Build();

// Dev: migration + seed
if (app.Environment.IsDevelopment())
{
    await Modules.Identity.DependencyInjection.MigrateAndSeedAsync(app.Services);
    await Modules.Booking.DependencyInjection.MigrateAsync(app.Services);
    await Modules.Notifications.DependencyInjection.MigrateAsync(app.Services);
}

// Middleware дараалал (PDF 9-р алхам)
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseMiddleware<TenantResolutionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;

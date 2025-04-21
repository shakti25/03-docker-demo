using DockerDemo03.Web.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // Leer configuración desde appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()  // Registrar logs en la consola
    .CreateLogger();

// Reemplaza el proveedor de logs predeterminado por Serilog
builder.Host.UseSerilog();

// Configurar la conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// configurar Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => {
    var configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
    return ConnectionMultiplexer.Connect(configuration);
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Aplicacar migraciones automáticamente al iniciar la aplicación
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    } catch (Exception ex) {
        Log.Fatal(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

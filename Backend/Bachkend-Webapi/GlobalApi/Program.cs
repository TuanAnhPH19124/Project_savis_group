using GlobalApi.Configuration;
using GlobalApi.Data;
using GlobalApi.IRepositories;
using GlobalApi.Repositories;
using GlobalApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(option =>
{
    var connection = builder.Configuration.GetConnectionString("PostgreSql");
    option.UseNpgsql(connection);
});
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var secretKey = builder.Configuration.GetSection("JwtConfig:Secret").Value;

    if (string.IsNullOrEmpty(secretKey))
    {
        throw new Exception("JwtConfig:Secret is missing or empty");
    }
    var key = Encoding.ASCII.GetBytes(secretKey);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
    };
});

// builder.Services.AddWatchDogServices(option =>
// {
//     option.IsAutoClear = false;
//     option.SetExternalDbConnString = builder.Configuration.GetConnectionString("PostgreSql");
//     option.DbDriverOption = WatchDogDbDriverEnum.PostgreSql;

// });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

AppDbContextMigration.StartMigration(app);
app.UseCors(option =>
{
    option.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "GetImage",
        pattern: "Upload/{id}/{imageName}",
        defaults: new { controller = "Home", action = "GetImage" }
    );

   
});


// app.UseWatchDogExceptionLogger();

// app.UseWatchDog(option =>
// {
//     option.WatchPagePassword = "admin";
//     option.WatchPageUsername = "admin";
// });

// app.UseHttpsRedirection();

SeedRecords.SeedUserAndRolesAsync(app).Wait();

app.Run();




using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RailReserve.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using RailReserve.Service;
using RailReserve.Configurations;
using RailReserve.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(MongoDB.Bson.BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

//add mongoIdentityConfiguration...
var mongoDbIdentityConfig = new MongoDbIdentityConfiguration
{
    MongoDbSettings = new MongoDbSettings
    {
        ConnectionString = "mongodb+srv://hirusha:hirusha@e-commerce-system.zfvw1fj.mongodb.net/RailReserve?retryWrites=true&w=majority",
        DatabaseName = "RailReserve"
    },
    IdentityOptionsAction = options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireLowercase = false;

        //lockout
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        options.Lockout.MaxFailedAccessAttempts = 5;

        options.User.RequireUniqueEmail = true;

    }
};




builder.Services.ConfigureMongoDbIdentity<User, Role, Guid>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<User>>()
    .AddSignInManager<SignInManager<User>>()
    .AddRoleManager<RoleManager<Role>>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureMongoDbIdentityUserOnly<Traveler, Guid>(mongoDbIdentityConfig)
    .AddUserManager<UserManager<Traveler>>()
    .AddSignInManager<SignInManager<Traveler>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITravelerService, TravelerService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IReservationService , ReservationService>();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDatabase"));

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = "https://localhost:5001",
        ValidAudience = "https://localhost:5001",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e")),
        ClockSkew = TimeSpan.Zero

    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

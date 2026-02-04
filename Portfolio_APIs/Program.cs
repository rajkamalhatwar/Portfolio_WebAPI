using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Portfolio_APIs.Interfaces;
using Portfolio_APIs.Repository;
using Portfolio_APIs.ServiceInterfaces;
using Portfolio_APIs.Services;
using ProjectAPI.Interfaces;
using ProjectAPI.Repository;
using ProjectAPI.ServiceInterfaces;
using ProjectAPI.Services;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuth, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>(); 

builder.Services.AddScoped<IUserReg, UserRegRepo>();
builder.Services.AddScoped<IUserRegService, UserRegService>();

builder.Services.AddScoped<IEducationRepo, EducationRepo>();
builder.Services.AddScoped<IEducationService, EducationService>(); 

builder.Services.AddScoped<IExperianceRepo, ExperianceRepo>();
builder.Services.AddScoped<IExperianceService, ExperianceService>();

builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<ICreativeWorksRepo, CreativeWorksRepo>();
builder.Services.AddScoped<ICreativeWorksService, CreativeWorksService>();

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// ✅ 1. Add CORS 

// ✅ CORS configuration for local development with cookies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://127.0.0.1:5500",  // VS Code Live Server
                "http://localhost:5174", // Vite (React)
                "http://localhost:5173", // Vite (React)
                "http://localhost:5175" // Vite (React)
            )
            .AllowAnyHeader()     // allow all headers
            .AllowAnyMethod();    // allow GET, POST, etc.
           // .AllowCredentials();  // allow cookies to be sent
    });
});



// 1. Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],    // from appsettings.json
        ValidAudience = builder.Configuration["Jwt:Audience"],// from appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    //// ✅ This allows reading token from cookie instead of header
    //options.Events = new JwtBearerEvents
    //{
    //    OnMessageReceived = context =>
    //    {
    //        if (context.Request.Cookies.ContainsKey("jwt"))
    //        {
    //            context.Token = context.Request.Cookies["jwt"];
    //        }
    //        return Task.CompletedTask;
    //    }
    //};
});

// 2. Add Authorization
builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection(); // comment out for production

// ✅ 4. Enable CORS before Auth
app.UseCors("AllowFrontend");

app.UseAuthentication(); // 3. Use Authentication Middleware
app.UseAuthorization();

app.MapControllers();

app.Run();

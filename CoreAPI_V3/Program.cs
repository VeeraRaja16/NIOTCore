//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.OpenApi.Models;
//using CoreAPI_V3.Repository;
//using CoreAPI_V3.CommonMethods;
//using CoreAPI_V3;
//using CoreAPI_V3.Interface;
//using CoreAPI_V3.Models;
//using static CoreAPI_V3.Models.ContextModel;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();

//builder.Services.AddTransient<DashboardRepo>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAdminRepository, AdminRepository>();  
//builder.Services.AddScoped<ILoggerService, LoggerService>();
//builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddTransient<CommonHelper>();
//builder.Services.AddTransient<GetLocation>();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AddPolicy", corsBuilder =>
//    {
//        corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});

//// Configure JWT authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//});

//// Configure session management (3 days expiration) // ---------> if not needed remove it.
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(3);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//// Enable Swagger with JWT Authorization support
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

//    // Add JWT Authentication to Swagger
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer abcdef12345\"",
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            new string[] { }
//        }
//    });
//});

//var app = builder.Build();

//// Seed admin user during startup ---------> ********* Store the Admin value
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        SeedAdminUser(services);  // Call the seed method here
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the database.");
//    }
//}

//// Middleware
//app.UseRouting();
//app.UseCors("AddPolicy");
//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//    });
//}

//app.MapControllers();
//app.Run();


////---> store multiple admins in the DB -------->
//static void SeedAdminUser(IServiceProvider serviceProvider)
//{
//    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
//    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

//    var adminCredentials = configuration.GetSection("AdminCredentials").Get<List<AdminCredential>>();

//    foreach (var credential in adminCredentials)
//    {
//        if (!context.Admins.Any(a => a.Username == credential.Username))
//        {
//            byte[] passwordHash, passwordSalt;
//            CreatePasswordHash(credential.Password, out passwordHash, out passwordSalt);

//            var admin = new Admin
//            {
//                Username = credential.Username,
//                PasswordHash = passwordHash,
//                PasswordSalt = passwordSalt,
//                CreatedDate =  DateTime.Now,
//                Role = "Admin"
//            };

//            context.Admins.Add(admin);
//            context.SaveChanges();
//        }
//    }
//}

//static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//{
//    using (var hmac = new System.Security.Cryptography.HMACSHA512())
//    {
//        passwordSalt = hmac.Key;
//        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
//    }
//}


//// this is created for testing purpose, can i use this same in the production environment or i need to modify it?
//public class AdminCredential
//{
//    public string Username { get; set; }
//    public string Password { get; set; }
//}





using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using CoreAPI_V3.Repository;
using CoreAPI_V3.CommonMethods;
using CoreAPI_V3;
using CoreAPI_V3.Interface;
using CoreAPI_V3.Models;
using static CoreAPI_V3.Models.ContextModel;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Configure application to listen on all interfaces on port 5000
//builder.WebHost.UseUrls("http://0.0.0.0:5000");
//builder.WebHost.UseUrls("http://192.168.1.57:5000");
//builder.WebHost.UseUrls("http://localhost:7321");
//builder.WebHost.UseUrls("https://0.0.0.0:5179");
// builder.WebHost.UseUrls("https://0.0.0.0:7290");

builder.WebHost.UseUrls("https://0.0.0.0:7290");




// Add services to the container
builder.Services.AddControllers();


builder.Services.AddTransient<DashboardRepo>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddTransient<CommonHelper>();
builder.Services.AddTransient<GetLocation>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AddPolicy", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", corsBuilder =>
    {
        corsBuilder/*.WithOrigins("https://192.168.1.57") */
                    .WithOrigins("http://localhost:8081", "https://192.168.1.57:7290", "https://192.168.1.57:3000", "https://192.168.1.57:8081") // Replace with your React Native dev server port
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer abcdef12345\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

CommonHelper help = new CommonHelper();
// ---> to show error message for unAuthorized access.
app.UseStatusCodePages(async context =>  
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == StatusCodes.Status403Forbidden)
    {
        response.ContentType = "application/json";
        await response.WriteAsync("{\"error\": \"You are not authorized to access this resource.\"}");
    }
    else if (response.StatusCode == StatusCodes.Status401Unauthorized)
    {
        response.ContentType = "application/json";
        await response.WriteAsync("{\"error\": \"You must be logged in to access this resource.\", \"errorCode\": \"401 - unauthorized.\"} ");

    }
});



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedAdminUser(services);  
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseRouting();
app.UseCors("DevelopmentPolicy"); 
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}


// --> to show swagger in production (you can delete this) 
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});



app.MapControllers();
app.Run();

static void SeedAdminUser(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var adminCredentials = configuration.GetSection("AdminCredentials").Get<List<AdminCredential>>();

    foreach (var credential in adminCredentials)
    {
        if (!context.Admins.Any(a => a.Username == credential.Username))
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(credential.Password, out passwordHash, out passwordSalt);

            var admin = new Admin
            {
                Username = credential.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDate = DateTime.Now,
                Role = "Admin",
                SessionStart = DateTime.Now
            };

            context.Admins.Add(admin);
            context.SaveChanges();
        }
    }
}

static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
{
    using (var hmac = new System.Security.Cryptography.HMACSHA512())
    {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}

public class AdminCredential
{
    public string Username { get; set; }
    public string Password { get; set; }
}




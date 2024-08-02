using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Fyp.Interfaces;
using Fyp.Repository;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var services = builder.Services;

// Add controllers and API explorer
services.AddControllers();
services.AddEndpointsApiExplorer();

// Add SignalR for real-time communication
services.AddSignalR();

// Register repositories and services
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ICommunityRepository, CommunityRepository>();
services.AddScoped<IPostRepository, PostRepository>();
services.AddTransient<IEmailRepository, EmailRepository>();
services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IUniversityRepository, UniversityRepository>();
services.AddScoped<IDocumentRepository, DocumentRepository>();
services.AddScoped<BlobStorageService>();

// Add FCM service
services.AddScoped<IFcmService, FcmService>();

// Add Hangfire services
services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.FromSeconds(15),
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    }));

// Add the processing server as IHostedService
services.AddHangfireServer();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard authorization header using the Bearer scheme (\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// Configure DbContext
services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT authentication
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configure CORS policy
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOriginsPolicy",
        builder =>
        {
            builder.WithOrigins("http://192.168.0.106:7210", "http://localhost:3002")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("Firebase/testing-3a202-firebase-adminsdk-yu3bd-0901c73dd7.json"),
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

// Enable CORS policy before authentication and authorization
app.UseCors("AllowSpecificOriginsPolicy");

// Authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Log requests for debugging
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request for {context.Request.Path} received");
    await next.Invoke();
});

// Map controllers for HTTP APIs
app.MapControllers();

// Map SignalR hub(s)
app.MapHub<ChatHub>("/chatHub");

// Configure Hangfire dashboard
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<IUserRepository>("delete-old-stories", x => x.DeleteOldStoriesAsync(), Cron.Hourly);

// Run the application
app.Run();

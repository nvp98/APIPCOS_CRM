using APIPCOS_CRM.Data;
using APIPCOS_CRM.Helper;
using APIPCOS_CRM.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Configure DbContext
builder.Services.AddDbContext<PLCOS_Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB")));

var bkmis11Version = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Bkmis_11"));
builder.Services.AddDbContext<Bkmis11_Context>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Bkmis_11"), bkmis11Version));

var bkmis13Version = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Bkmis_13"));
builder.Services.AddDbContext<Bkmis13_Context>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Bkmis_13"), bkmis13Version));
builder.Services.AddScoped<UnitOfWork>();
// Add repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddHttpContextAccessor();
// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API PL-COS", Version = "v1" });

    // ??nh ngh?a Security Definition cho Basic Authentication
    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Basic Authorization header using the Basic scheme."
    });

    // ??nh ngh?a y�u c?u Security cho c�c endpoint
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add authentication
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API PL-COS");
    c.RoutePrefix = "api";
});

// Configure the HTTP request pipeline.

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
